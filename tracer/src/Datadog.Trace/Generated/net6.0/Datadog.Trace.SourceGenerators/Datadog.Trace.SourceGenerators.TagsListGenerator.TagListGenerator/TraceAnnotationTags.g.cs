﻿// <auto-generated/>
#nullable enable

using Datadog.Trace.Processors;
using Datadog.Trace.Tagging;

namespace Datadog.Trace.Tagging
{
    partial class TraceAnnotationTags
    {
        // InstrumentationNameBytes = System.Text.Encoding.UTF8.GetBytes("component");
        private static readonly byte[] InstrumentationNameBytes = new byte[] { 99, 111, 109, 112, 111, 110, 101, 110, 116 };

        public override string? GetTag(string key)
        {
            return key switch
            {
                "component" => InstrumentationName,
                _ => base.GetTag(key),
            };
        }

        public override void SetTag(string key, string value)
        {
            switch(key)
            {
                default: 
                    base.SetTag(key, value);
                    break;
            }
        }

        protected static Datadog.Trace.Tagging.IProperty<string?>[] TraceAnnotationTagsProperties => 
             Datadog.Trace.ExtensionMethods.ArrayExtensions.Concat(CommonTagsProperties,
                new Datadog.Trace.Tagging.Property<TraceAnnotationTags, string?>("component", t => t.InstrumentationName)
        );

        public override void EnumerateTags<TProcessor>(ref TProcessor processor)
        {
            if (InstrumentationName is not null)
            {
                processor.Process(new TagItem<string>("component", InstrumentationName, InstrumentationNameBytes));
            }

            base.EnumerateTags(ref processor);
        }

        protected override Datadog.Trace.Tagging.IProperty<string?>[] GetAdditionalTags()
        {
             return TraceAnnotationTagsProperties;
        }

        protected override int WriteAdditionalTags(ref byte[] bytes, ref int offset, ITagProcessor[] tagProcessors)
        {
            var count = 0;
            if (InstrumentationName is not null)
            {
                count++;
                WriteTag(ref bytes, ref offset, InstrumentationNameBytes, InstrumentationName, tagProcessors);
            }

            return count + base.WriteAdditionalTags(ref bytes, ref offset, tagProcessors);
        }

        protected override void WriteAdditionalTags(System.Text.StringBuilder sb)
        {
            if (InstrumentationName is not null)
            {
                sb.Append("component (tag):")
                  .Append(InstrumentationName)
                  .Append(',');
            }

            base.WriteAdditionalTags(sb);
        }
    }
}
