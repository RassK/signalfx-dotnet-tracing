﻿// <auto-generated/>
#nullable enable

using Datadog.Trace.Processors;

namespace Datadog.Trace.Tagging
{
    partial class AzureFunctionsTags
    {
        private static readonly byte[] SpanKindBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("span.kind");
        private static readonly byte[] InstrumentationNameBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("component");
        private static readonly byte[] LanguageBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("language");
        private static readonly byte[] ShortNameBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("aas.function.name");
        private static readonly byte[] FullNameBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("aas.function.method");
        private static readonly byte[] BindingSourceBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("aas.function.binding");
        private static readonly byte[] TriggerTypeBytes = Datadog.Trace.Vendors.MessagePack.StringEncoding.UTF8.GetBytes("aas.function.trigger");

        public override string? GetTag(string key)
        {
            return key switch
            {
                "span.kind" => SpanKind,
                "component" => InstrumentationName,
                "language" => Language,
                "aas.function.name" => ShortName,
                "aas.function.method" => FullName,
                "aas.function.binding" => BindingSource,
                "aas.function.trigger" => TriggerType,
                _ => base.GetTag(key),
            };
        }

        public override void SetTag(string key, string value)
        {
            switch(key)
            {
                case "aas.function.name": 
                    ShortName = value;
                    break;
                case "aas.function.method": 
                    FullName = value;
                    break;
                case "aas.function.binding": 
                    BindingSource = value;
                    break;
                case "aas.function.trigger": 
                    TriggerType = value;
                    break;
                default: 
                    base.SetTag(key, value);
                    break;
            }
        }

        protected static Datadog.Trace.Tagging.IProperty<string?>[] AzureFunctionsTagsProperties => 
             Datadog.Trace.ExtensionMethods.ArrayExtensions.Concat(InstrumentationTagsProperties,
                new Datadog.Trace.Tagging.Property<AzureFunctionsTags, string?>("span.kind", t => t.SpanKind),
                new Datadog.Trace.Tagging.Property<AzureFunctionsTags, string?>("component", t => t.InstrumentationName),
                new Datadog.Trace.Tagging.Property<AzureFunctionsTags, string?>("language", t => t.Language),
                new Datadog.Trace.Tagging.Property<AzureFunctionsTags, string?>("aas.function.name", t => t.ShortName),
                new Datadog.Trace.Tagging.Property<AzureFunctionsTags, string?>("aas.function.method", t => t.FullName),
                new Datadog.Trace.Tagging.Property<AzureFunctionsTags, string?>("aas.function.binding", t => t.BindingSource),
                new Datadog.Trace.Tagging.Property<AzureFunctionsTags, string?>("aas.function.trigger", t => t.TriggerType)
);

        protected override Datadog.Trace.Tagging.IProperty<string?>[] GetAdditionalTags()
        {
             return AzureFunctionsTagsProperties;
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

            if (Language != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, LanguageBytes, Language, tagProcessors);
            }

            if (ShortName != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, ShortNameBytes, ShortName, tagProcessors);
            }

            if (FullName != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, FullNameBytes, FullName, tagProcessors);
            }

            if (BindingSource != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, BindingSourceBytes, BindingSource, tagProcessors);
            }

            if (TriggerType != null)
            {
                count++;
                WriteTag(ref bytes, ref offset, TriggerTypeBytes, TriggerType, tagProcessors);
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

            if (Language != null)
            {
                sb.Append("language (tag):")
                  .Append(Language)
                  .Append(',');
            }

            if (ShortName != null)
            {
                sb.Append("aas.function.name (tag):")
                  .Append(ShortName)
                  .Append(',');
            }

            if (FullName != null)
            {
                sb.Append("aas.function.method (tag):")
                  .Append(FullName)
                  .Append(',');
            }

            if (BindingSource != null)
            {
                sb.Append("aas.function.binding (tag):")
                  .Append(BindingSource)
                  .Append(',');
            }

            if (TriggerType != null)
            {
                sb.Append("aas.function.trigger (tag):")
                  .Append(TriggerType)
                  .Append(',');
            }

            base.WriteAdditionalTags(sb);
        }
    }
}
