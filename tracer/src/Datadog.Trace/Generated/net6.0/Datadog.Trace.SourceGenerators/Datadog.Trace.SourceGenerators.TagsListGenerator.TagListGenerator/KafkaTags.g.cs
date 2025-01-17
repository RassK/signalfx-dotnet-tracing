﻿// <auto-generated/>
#nullable enable

using Datadog.Trace.Processors;
using Datadog.Trace.Tagging;

namespace Datadog.Trace.Tagging
{
    partial class KafkaTags
    {
        // MessageQueueTimeMsBytes = System.Text.Encoding.UTF8.GetBytes("message.queue_time_ms");
        private static readonly byte[] MessageQueueTimeMsBytes = new byte[] { 109, 101, 115, 115, 97, 103, 101, 46, 113, 117, 101, 117, 101, 95, 116, 105, 109, 101, 95, 109, 115 };
        // SpanKindBytes = System.Text.Encoding.UTF8.GetBytes("span.kind");
        private static readonly byte[] SpanKindBytes = new byte[] { 115, 112, 97, 110, 46, 107, 105, 110, 100 };
        // InstrumentationNameBytes = System.Text.Encoding.UTF8.GetBytes("component");
        private static readonly byte[] InstrumentationNameBytes = new byte[] { 99, 111, 109, 112, 111, 110, 101, 110, 116 };
        // PartitionBytes = System.Text.Encoding.UTF8.GetBytes("messaging.kafka.partition");
        private static readonly byte[] PartitionBytes = new byte[] { 109, 101, 115, 115, 97, 103, 105, 110, 103, 46, 107, 97, 102, 107, 97, 46, 112, 97, 114, 116, 105, 116, 105, 111, 110 };
        // OffsetBytes = System.Text.Encoding.UTF8.GetBytes("messaging.kafka.offset");
        private static readonly byte[] OffsetBytes = new byte[] { 109, 101, 115, 115, 97, 103, 105, 110, 103, 46, 107, 97, 102, 107, 97, 46, 111, 102, 102, 115, 101, 116 };
        // TombstoneBytes = System.Text.Encoding.UTF8.GetBytes("messaging.kafka.tombstone");
        private static readonly byte[] TombstoneBytes = new byte[] { 109, 101, 115, 115, 97, 103, 105, 110, 103, 46, 107, 97, 102, 107, 97, 46, 116, 111, 109, 98, 115, 116, 111, 110, 101 };

        public override string? GetTag(string key)
        {
            return key switch
            {
                "span.kind" => SpanKind,
                "component" => InstrumentationName,
                "messaging.kafka.partition" => Partition,
                "messaging.kafka.offset" => Offset,
                "messaging.kafka.tombstone" => Tombstone,
                _ => base.GetTag(key),
            };
        }

        public override void SetTag(string key, string value)
        {
            switch(key)
            {
                case "messaging.kafka.partition": 
                    Partition = value;
                    break;
                case "messaging.kafka.offset": 
                    Offset = value;
                    break;
                case "messaging.kafka.tombstone": 
                    Tombstone = value;
                    break;
                default: 
                    base.SetTag(key, value);
                    break;
            }
        }

        protected static Datadog.Trace.Tagging.IProperty<string?>[] KafkaTagsProperties => 
             Datadog.Trace.ExtensionMethods.ArrayExtensions.Concat(MessagingTagsProperties,
                new Datadog.Trace.Tagging.Property<KafkaTags, string?>("span.kind", t => t.SpanKind),
                new Datadog.Trace.Tagging.Property<KafkaTags, string?>("component", t => t.InstrumentationName),
                new Datadog.Trace.Tagging.Property<KafkaTags, string?>("messaging.kafka.partition", t => t.Partition),
                new Datadog.Trace.Tagging.Property<KafkaTags, string?>("messaging.kafka.offset", t => t.Offset),
                new Datadog.Trace.Tagging.Property<KafkaTags, string?>("messaging.kafka.tombstone", t => t.Tombstone)
        );

        public override void EnumerateTags<TProcessor>(ref TProcessor processor)
        {
            if (SpanKind is not null)
            {
                processor.Process(new TagItem<string>("span.kind", SpanKind, SpanKindBytes));
            }

            if (InstrumentationName is not null)
            {
                processor.Process(new TagItem<string>("component", InstrumentationName, InstrumentationNameBytes));
            }

            if (Partition is not null)
            {
                processor.Process(new TagItem<string>("messaging.kafka.partition", Partition, PartitionBytes));
            }

            if (Offset is not null)
            {
                processor.Process(new TagItem<string>("messaging.kafka.offset", Offset, OffsetBytes));
            }

            if (Tombstone is not null)
            {
                processor.Process(new TagItem<string>("messaging.kafka.tombstone", Tombstone, TombstoneBytes));
            }

            base.EnumerateTags(ref processor);
        }

        protected override Datadog.Trace.Tagging.IProperty<string?>[] GetAdditionalTags()
        {
             return KafkaTagsProperties;
        }

        protected override int WriteAdditionalTags(ref byte[] bytes, ref int offset, ITagProcessor[] tagProcessors)
        {
            var count = 0;
            if (SpanKind is not null)
            {
                count++;
                WriteTag(ref bytes, ref offset, SpanKindBytes, SpanKind, tagProcessors);
            }

            if (InstrumentationName is not null)
            {
                count++;
                WriteTag(ref bytes, ref offset, InstrumentationNameBytes, InstrumentationName, tagProcessors);
            }

            if (Partition is not null)
            {
                count++;
                WriteTag(ref bytes, ref offset, PartitionBytes, Partition, tagProcessors);
            }

            if (Offset is not null)
            {
                count++;
                WriteTag(ref bytes, ref offset, OffsetBytes, Offset, tagProcessors);
            }

            if (Tombstone is not null)
            {
                count++;
                WriteTag(ref bytes, ref offset, TombstoneBytes, Tombstone, tagProcessors);
            }

            return count + base.WriteAdditionalTags(ref bytes, ref offset, tagProcessors);
        }

        protected override void WriteAdditionalTags(System.Text.StringBuilder sb)
        {
            if (SpanKind is not null)
            {
                sb.Append("span.kind (tag):")
                  .Append(SpanKind)
                  .Append(',');
            }

            if (InstrumentationName is not null)
            {
                sb.Append("component (tag):")
                  .Append(InstrumentationName)
                  .Append(',');
            }

            if (Partition is not null)
            {
                sb.Append("messaging.kafka.partition (tag):")
                  .Append(Partition)
                  .Append(',');
            }

            if (Offset is not null)
            {
                sb.Append("messaging.kafka.offset (tag):")
                  .Append(Offset)
                  .Append(',');
            }

            if (Tombstone is not null)
            {
                sb.Append("messaging.kafka.tombstone (tag):")
                  .Append(Tombstone)
                  .Append(',');
            }

            base.WriteAdditionalTags(sb);
        }
        public override double? GetMetric(string key)
        {
            return key switch
            {
                "message.queue_time_ms" => MessageQueueTimeMs,
                _ => base.GetMetric(key),
            };
        }

        public override void SetMetric(string key, double? value)
        {
            switch(key)
            {
                case "message.queue_time_ms": 
                    MessageQueueTimeMs = value;
                    break;
                default: 
                    base.SetMetric(key, value);
                    break;
            }
        }

        public override void EnumerateMetrics<TProcessor>(ref TProcessor processor)
        {
            if (MessageQueueTimeMs is not null)
            {
                processor.Process(new TagItem<double>("message.queue_time_ms", MessageQueueTimeMs.Value, MessageQueueTimeMsBytes));
            }

            base.EnumerateMetrics(ref processor);
        }

        protected override void WriteAdditionalMetrics(System.Text.StringBuilder sb)
        {
            if (MessageQueueTimeMs is not null)
            {
                sb.Append("message.queue_time_ms (metric):")
                  .Append(MessageQueueTimeMs.Value)
                  .Append(',');
            }

            base.WriteAdditionalMetrics(sb);
        }
    }
}
