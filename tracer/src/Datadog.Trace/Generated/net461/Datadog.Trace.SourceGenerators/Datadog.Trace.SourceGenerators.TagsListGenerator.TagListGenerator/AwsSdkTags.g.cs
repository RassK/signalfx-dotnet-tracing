﻿// <auto-generated/>
#nullable enable

using Datadog.Trace.Processors;

namespace Datadog.Trace.Tagging
{
    partial class AwsSdkTags
    {
        private static readonly byte[] InstrumentationNameBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("component");
        private static readonly byte[] AgentNameBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("aws.agent");
        private static readonly byte[] OperationBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("aws.operation");
        private static readonly byte[] RegionBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("aws.region");
        private static readonly byte[] RequestIdBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("aws.requestId");
        private static readonly byte[] ServiceBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("aws.service");
        private static readonly byte[] HttpMethodBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("http.method");
        private static readonly byte[] HttpUrlBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("http.url");
        private static readonly byte[] HttpStatusCodeBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("http.status_code");

        public override string? GetTag(string key)
        {
            return key switch
            {
                "component" => InstrumentationName,
                "aws.agent" => AgentName,
                "aws.operation" => Operation,
                "aws.region" => Region,
                "aws.requestId" => RequestId,
                "aws.service" => Service,
                "http.method" => HttpMethod,
                "http.url" => HttpUrl,
                "http.status_code" => HttpStatusCode,
                _ => base.GetTag(key),
            };
        }

        public override void SetTag(string key, string value)
        {
            switch(key)
            {
                case "aws.operation": 
                    Operation = value;
                    break;
                case "aws.region": 
                    Region = value;
                    break;
                case "aws.requestId": 
                    RequestId = value;
                    break;
                case "aws.service": 
                    Service = value;
                    break;
                case "http.method": 
                    HttpMethod = value;
                    break;
                case "http.url": 
                    HttpUrl = value;
                    break;
                case "http.status_code": 
                    HttpStatusCode = value;
                    break;
                default: 
                    base.SetTag(key, value);
                    break;
            }
        }

        protected static Datadog.Trace.Tagging.IProperty<string?>[] AwsSdkTagsProperties => 
             Datadog.Trace.ExtensionMethods.ArrayExtensions.Concat(InstrumentationTagsProperties,
                new Datadog.Trace.Tagging.Property<AwsSdkTags, string?>("component", t => t.InstrumentationName),
                new Datadog.Trace.Tagging.Property<AwsSdkTags, string?>("aws.agent", t => t.AgentName),
                new Datadog.Trace.Tagging.Property<AwsSdkTags, string?>("aws.operation", t => t.Operation),
                new Datadog.Trace.Tagging.Property<AwsSdkTags, string?>("aws.region", t => t.Region),
                new Datadog.Trace.Tagging.Property<AwsSdkTags, string?>("aws.requestId", t => t.RequestId),
                new Datadog.Trace.Tagging.Property<AwsSdkTags, string?>("aws.service", t => t.Service),
                new Datadog.Trace.Tagging.Property<AwsSdkTags, string?>("http.method", t => t.HttpMethod),
                new Datadog.Trace.Tagging.Property<AwsSdkTags, string?>("http.url", t => t.HttpUrl),
                new Datadog.Trace.Tagging.Property<AwsSdkTags, string?>("http.status_code", t => t.HttpStatusCode)
);

        protected override Datadog.Trace.Tagging.IProperty<string?>[] GetAdditionalTags()
        {
             return AwsSdkTagsProperties;
        }

        protected override int WriteAdditionalTags(ref byte[] bytes, ref int offset, ITagProcessor[] tagProcessors)
        {
            var count = 0;
            if (InstrumentationName != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, InstrumentationNameBytes, InstrumentationName, tagProcessors);
            }

            if (AgentName != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, AgentNameBytes, AgentName, tagProcessors);
            }

            if (Operation != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, OperationBytes, Operation, tagProcessors);
            }

            if (Region != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, RegionBytes, Region, tagProcessors);
            }

            if (RequestId != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, RequestIdBytes, RequestId, tagProcessors);
            }

            if (Service != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, ServiceBytes, Service, tagProcessors);
            }

            if (HttpMethod != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, HttpMethodBytes, HttpMethod, tagProcessors);
            }

            if (HttpUrl != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, HttpUrlBytes, HttpUrl, tagProcessors);
            }

            if (HttpStatusCode != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, HttpStatusCodeBytes, HttpStatusCode, tagProcessors);
            }

            return count + base.WriteAdditionalTags(ref bytes, ref offset, tagProcessors);
        }

        protected override void WriteAdditionalTags(System.Text.StringBuilder sb)
        {
            if (InstrumentationName != null)
            {
                sb.Append("component (tag):")
                  .Append(InstrumentationName)
                  .Append(',');
            }

            if (AgentName != null)
            {
                sb.Append("aws.agent (tag):")
                  .Append(AgentName)
                  .Append(',');
            }

            if (Operation != null)
            {
                sb.Append("aws.operation (tag):")
                  .Append(Operation)
                  .Append(',');
            }

            if (Region != null)
            {
                sb.Append("aws.region (tag):")
                  .Append(Region)
                  .Append(',');
            }

            if (RequestId != null)
            {
                sb.Append("aws.requestId (tag):")
                  .Append(RequestId)
                  .Append(',');
            }

            if (Service != null)
            {
                sb.Append("aws.service (tag):")
                  .Append(Service)
                  .Append(',');
            }

            if (HttpMethod != null)
            {
                sb.Append("http.method (tag):")
                  .Append(HttpMethod)
                  .Append(',');
            }

            if (HttpUrl != null)
            {
                sb.Append("http.url (tag):")
                  .Append(HttpUrl)
                  .Append(',');
            }

            if (HttpStatusCode != null)
            {
                sb.Append("http.status_code (tag):")
                  .Append(HttpStatusCode)
                  .Append(',');
            }

            base.WriteAdditionalTags(sb);
        }
    }
}
