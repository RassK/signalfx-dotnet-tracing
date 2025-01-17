// <copyright file="IDatadogOpenTracingTracer.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

// Modified by Splunk Inc.

using System;

namespace Datadog.Trace
{
    /// <summary>
    /// Internal interface for keeping a consistent Tracer API between the Datadog.Trace.OpenTracing assembly and the Datadog.Trace assemblies
    /// </summary>
    internal interface IDatadogOpenTracingTracer
    {
        string DefaultServiceName { get; }

        IScopeManager ScopeManager { get; }

        Span StartSpan(string operationName, ISpanContext parent, string serviceName, DateTimeOffset? startTime, bool ignoreActiveScope);
    }
}
