﻿// <auto-generated/>
#nullable enable

using Datadog.Trace.Processors;

namespace Datadog.Trace.ClrProfiler.AutoInstrumentation.Redis
{
    partial class RedisTags
    {
        private static readonly byte[] SpanKindBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("span.kind");
        private static readonly byte[] InstrumentationNameBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("component");
        private static readonly byte[] DbTypeBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("db.system");
        private static readonly byte[] RawCommandBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("db.statement");
        private static readonly byte[] HostBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("net.peer.name");
        private static readonly byte[] PortBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("net.peer.port");

        public override string? GetTag(string key)
        {
            return key switch
            {
                "span.kind" => SpanKind,
                "component" => InstrumentationName,
                "db.system" => DbType,
                "db.statement" => RawCommand,
                "net.peer.name" => Host,
                "net.peer.port" => Port,
                _ => base.GetTag(key),
            };
        }

        public override void SetTag(string key, string value)
        {
            switch(key)
            {
                case "db.statement": 
                    RawCommand = value;
                    break;
                case "net.peer.name": 
                    Host = value;
                    break;
                case "net.peer.port": 
                    Port = value;
                    break;
                default: 
                    base.SetTag(key, value);
                    break;
            }
        }

        protected static Datadog.Trace.Tagging.IProperty<string?>[] RedisTagsProperties => 
             Datadog.Trace.ExtensionMethods.ArrayExtensions.Concat(InstrumentationTagsProperties,
                new Datadog.Trace.Tagging.Property<RedisTags, string?>("span.kind", t => t.SpanKind),
                new Datadog.Trace.Tagging.Property<RedisTags, string?>("component", t => t.InstrumentationName),
                new Datadog.Trace.Tagging.Property<RedisTags, string?>("db.system", t => t.DbType),
                new Datadog.Trace.Tagging.Property<RedisTags, string?>("db.statement", t => t.RawCommand),
                new Datadog.Trace.Tagging.Property<RedisTags, string?>("net.peer.name", t => t.Host),
                new Datadog.Trace.Tagging.Property<RedisTags, string?>("net.peer.port", t => t.Port)
);

        protected override Datadog.Trace.Tagging.IProperty<string?>[] GetAdditionalTags()
        {
             return RedisTagsProperties;
        }

        protected override int WriteAdditionalTags(ref byte[] bytes, ref int offset, ITagProcessor[] tagProcessors)
        {
            var count = 0;
            if (SpanKind != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, SpanKindBytes, SpanKind, tagProcessors);
            }

            if (InstrumentationName != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, InstrumentationNameBytes, InstrumentationName, tagProcessors);
            }

            if (DbType != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, DbTypeBytes, DbType, tagProcessors);
            }

            if (RawCommand != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, RawCommandBytes, RawCommand, tagProcessors);
            }

            if (Host != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, HostBytes, Host, tagProcessors);
            }

            if (Port != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, PortBytes, Port, tagProcessors);
            }

            return count + base.WriteAdditionalTags(ref bytes, ref offset, tagProcessors);
        }

        protected override void WriteAdditionalTags(System.Text.StringBuilder sb)
        {
            if (SpanKind != null)
            {
                sb.Append("span.kind (tag):")
                  .Append(SpanKind)
                  .Append(',');
            }

            if (InstrumentationName != null)
            {
                sb.Append("component (tag):")
                  .Append(InstrumentationName)
                  .Append(',');
            }

            if (DbType != null)
            {
                sb.Append("db.system (tag):")
                  .Append(DbType)
                  .Append(',');
            }

            if (RawCommand != null)
            {
                sb.Append("db.statement (tag):")
                  .Append(RawCommand)
                  .Append(',');
            }

            if (Host != null)
            {
                sb.Append("net.peer.name (tag):")
                  .Append(Host)
                  .Append(',');
            }

            if (Port != null)
            {
                sb.Append("net.peer.port (tag):")
                  .Append(Port)
                  .Append(',');
            }

            base.WriteAdditionalTags(sb);
        }
    }
}
