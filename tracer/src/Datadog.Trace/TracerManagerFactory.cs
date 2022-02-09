// <copyright file="TracerManagerFactory.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

// Modified by Splunk Inc.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Datadog.Trace.Abstractions;
using Datadog.Trace.Agent;
using Datadog.Trace.Agent.Zipkin;
using Datadog.Trace.ClrProfiler;
using Datadog.Trace.Configuration;
using Datadog.Trace.Conventions;
using Datadog.Trace.DogStatsd;
using Datadog.Trace.Logging;
using Datadog.Trace.PlatformHelpers;
using Datadog.Trace.Propagation;
using Datadog.Trace.RuntimeMetrics;
using Datadog.Trace.Sampling;
using Datadog.Trace.SignalFx.Metrics;
using Datadog.Trace.Util;
using Datadog.Trace.Vendors.StatsdClient;

namespace Datadog.Trace
{
    internal class TracerManagerFactory
    {
        private const string UnknownServiceName = "UnknownService";
        private static readonly IDatadogLogger Log = DatadogLogging.GetLoggerFor<TracerManagerFactory>();

        public static readonly TracerManagerFactory Instance = new();
        private const int MaxMetricsInAsyncQueue = 1000;

        /// <summary>
        /// The primary factory method, called by <see cref="TracerManager"/>,
        /// providing the previous global <see cref="TracerManager"/> instance (may be null)
        /// </summary>
        internal TracerManager CreateTracerManager(ImmutableTracerSettings settings, TracerManager previous)
        {
            // TODO: If relevant settings have not changed, continue using existing statsd/agent writer/runtime metrics etc
            return CreateTracerManager(
                settings,
                agentWriter: null,
                sampler: null,
                scopeManager: previous?.ScopeManager, // no configuration, so can always use the same one
                statsd: null,
                runtimeMetrics: null);
        }

        /// <summary>
        /// Internal for use in tests that create "standalone" <see cref="TracerManager"/> by
        /// </summary>
        /// <see cref="Tracer(TracerSettings, IAgentWriter, ISampler, IScopeManager, IDogStatsd)"/>
        internal TracerManager CreateTracerManager(
            ImmutableTracerSettings settings,
            IAgentWriter agentWriter,
            ISampler sampler,
            IScopeManager scopeManager,
            IDogStatsd statsd,
            RuntimeMetricsWriter runtimeMetrics)
        {
            settings ??= ImmutableTracerSettings.FromDefaultSources();

            var defaultServiceName = settings.ServiceName ??
                                     GetApplicationName() ??
                                     UnknownServiceName;

            var traceIdConvention = GetTraceIdConvention(settings.Convention);

            statsd = settings.TracerMetricsEnabled
                         ? (statsd ?? CreateDogStatsdClient(settings, defaultServiceName, settings.ExporterSettings.DogStatsdPort))
                         : null;
            sampler ??= GetSampler(settings);
            var propagator = CreateCompositePropagator(settings, traceIdConvention);
            agentWriter ??= GetAgentWriter(settings, statsd, sampler);
            scopeManager ??= new AsyncLocalScopeManager(settings.ThreadSamplingEnabled);

            if (settings.RuntimeMetricsEnabled && !DistributedTracer.Instance.IsChildTracer)
            {
                runtimeMetrics ??= new RuntimeMetricsWriter(statsd ?? CreateDogStatsdClient(settings, defaultServiceName, settings.ExporterSettings.DogStatsdPort), TimeSpan.FromSeconds(10));
            }

            var tracerManager = CreateTracerManagerFrom(settings, agentWriter, sampler, propagator, scopeManager, statsd, runtimeMetrics, traceIdConvention, defaultServiceName);
            return tracerManager;
        }

        /// <summary>
        ///  Can be overriden to create a different <see cref="TracerManager"/>, e.g. <see cref="Ci.CITracerManager"/>
        /// </summary>
        protected virtual TracerManager CreateTracerManagerFrom(
            ImmutableTracerSettings settings,
            IAgentWriter agentWriter,
            ISampler sampler,
            IPropagator propagator,
            IScopeManager scopeManager,
            IDogStatsd statsd,
            RuntimeMetricsWriter runtimeMetrics,
            ITraceIdConvention traceIdConvention,
            string defaultServiceName)
            => new TracerManager(settings, agentWriter, sampler, propagator, scopeManager, statsd, runtimeMetrics, traceIdConvention, defaultServiceName);

        protected virtual ISampler GetSampler(ImmutableTracerSettings settings)
        {
            var sampler = new RuleBasedSampler(new RateLimiter(settings.MaxTracesSubmittedPerSecond));

            if (!string.IsNullOrWhiteSpace(settings.CustomSamplingRules))
            {
                foreach (var rule in CustomSamplingRule.BuildFromConfigurationString(settings.CustomSamplingRules))
                {
                    sampler.RegisterRule(rule);
                }
            }

            if (settings.GlobalSamplingRate != null)
            {
                var globalRate = (float)settings.GlobalSamplingRate;

                if (globalRate < 0f || globalRate > 1f)
                {
                    Log.Warning("{ConfigurationKey} configuration of {ConfigurationValue} is out of range", ConfigurationKeys.GlobalSamplingRate, settings.GlobalSamplingRate);
                }
                else
                {
                    sampler.RegisterRule(new GlobalSamplingRule(globalRate));
                }
            }

            return sampler;
        }

        protected virtual IAgentWriter GetAgentWriter(ImmutableTracerSettings settings, IDogStatsd statsd, ISampler sampler)
        {
            IMetrics metrics = statsd != null
                ? new DogStatsdMetrics(statsd)
                : new NullMetrics();

            switch (settings.Exporter)
            {
                case ExporterType.Zipkin:
                    return new ExporterWriter(new ZipkinExporter(settings), metrics);
                default:
                    var apiRequestFactory = TransportStrategy.Get(settings.ExporterSettings);
                    var api = new Api(settings.ExporterSettings.AgentUri, apiRequestFactory, statsd, rates => sampler.SetDefaultSampleRates(rates), settings.ExporterSettings.PartialFlushEnabled);
                    return new AgentWriter(api, metrics, maxBufferSize: settings.TraceBufferSize);
            }
        }

        private static CompositeTextMapPropagator CreateCompositePropagator(ImmutableTracerSettings settings, ITraceIdConvention traceIdConvention)
        {
            var compositeProvider = new CompositePropagatorsProvider();
            compositeProvider.RegisterProvider(new OTelPropagatorsProvider());

            var propagators = compositeProvider
               .GetPropagators(settings.Propagators, traceIdConvention)
               .ToList();

            return new CompositeTextMapPropagator(propagators);
        }

        private static IDogStatsd CreateDogStatsdClient(ImmutableTracerSettings settings, string serviceName, int port)
        {
            try
            {
                var constantTags = new List<string>
                                   {
                                       "lang:.NET",
                                       $"lang_interpreter:{FrameworkDescription.Instance.Name}",
                                       $"lang_version:{FrameworkDescription.Instance.ProductVersion}",
                                       $"tracer_version:{TracerConstants.AssemblyVersion}",
                                       $"service:{serviceName}",
                                       $"{Tags.RuntimeId}:{Tracer.RuntimeId}"
                                   };

                if (settings.Environment != null)
                {
                    constantTags.Add($"env:{settings.Environment}");
                }

                if (settings.ServiceVersion != null)
                {
                    constantTags.Add($"version:{settings.ServiceVersion}");
                }

                if (settings.MetricsExporter == MetricsExporterType.SignalFx)
                {
                    var metricExporter = new SignalFxMetricExporter(settings.ExporterSettings.MetricsEndpointUrl, settings.SignalFxAccessToken);
                    var metricSender = new SignalFxMetricSender(new AsyncSignalFxMetricWriter(metricExporter, MaxMetricsInAsyncQueue), constantTags.ToArray());
                    // TODO splunk: consider registering dependencies and disposing them inside SignalFxStats
                    return new SignalFxStats(metricSender);
                }

                var statsd = new DogStatsdService();
                if (AzureAppServices.Metadata.IsRelevant)
                {
                    // Environment variables set by the Azure App Service extension are used internally.
                    // Setting the server name will force UDP, when we need named pipes.
                    statsd.Configure(new StatsdConfig
                    {
                        ConstantTags = constantTags.ToArray()
                    });
                }
                else
                {
                    statsd.Configure(new StatsdConfig
                    {
                        StatsdServerName = settings.ExporterSettings.AgentUri.DnsSafeHost,
                        StatsdPort = port,
                        ConstantTags = constantTags.ToArray()
                    });
                }

                return statsd;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unable to instantiate StatsD client.");
                return new NoOpStatsd();
            }
        }

        /// <summary>
        /// Gets an "application name" for the executing application by looking at
        /// the hosted app name (.NET Framework on IIS only), assembly name, and process name.
        /// </summary>
        /// <returns>The default service name.</returns>
        private static string GetApplicationName()
        {
            try
            {
                if (AzureAppServices.Metadata.IsRelevant)
                {
                    return AzureAppServices.Metadata.SiteName;
                }

                try
                {
                    if (TryLoadAspNetSiteName(out var siteName))
                    {
                        return siteName;
                    }
                }
                catch (Exception ex)
                {
                    // Unable to call into System.Web.dll
                    Log.Error(ex, "Unable to get application name through ASP.NET settings");
                }

                return Assembly.GetEntryAssembly()?.GetName().Name ??
                       ProcessHelpers.GetCurrentProcessName();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating default service name.");
                return null;
            }
        }

        private static bool TryLoadAspNetSiteName(out string siteName)
        {
#if NETFRAMEWORK
            // System.Web.dll is only available on .NET Framework
            if (System.Web.Hosting.HostingEnvironment.IsHosted)
            {
                // if this app is an ASP.NET application, return "SiteName/ApplicationVirtualPath".
                // note that ApplicationVirtualPath includes a leading slash.
                siteName = (System.Web.Hosting.HostingEnvironment.SiteName + System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath).TrimEnd('/');
                return true;
            }

#endif
            siteName = default;
            return false;
        }

        private ITraceIdConvention GetTraceIdConvention(ConventionType convention)
        {
            switch (convention)
            {
                case ConventionType.Datadog:
                    return new DatadogTraceIdConvention();
                case ConventionType.OpenTelemetry:
                default:
                    return new OtelTraceIdConvention();
            }
        }
    }
}
