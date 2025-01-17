﻿// <auto-generated/>
#nullable enable

using Datadog.Trace.Processors;
using Datadog.Trace.Tagging;

namespace Datadog.Trace.Tagging
{
    partial class RabbitMQTags
    {
        // SpanKindBytes = System.Text.Encoding.UTF8.GetBytes("span.kind");
        private static readonly byte[] SpanKindBytes = new byte[] { 115, 112, 97, 110, 46, 107, 105, 110, 100 };
        // InstrumentationNameBytes = System.Text.Encoding.UTF8.GetBytes("component");
        private static readonly byte[] InstrumentationNameBytes = new byte[] { 99, 111, 109, 112, 111, 110, 101, 110, 116 };
        // CommandBytes = System.Text.Encoding.UTF8.GetBytes("amqp.command");
        private static readonly byte[] CommandBytes = new byte[] { 97, 109, 113, 112, 46, 99, 111, 109, 109, 97, 110, 100 };
        // DeliveryModeBytes = System.Text.Encoding.UTF8.GetBytes("amqp.delivery_mode");
        private static readonly byte[] DeliveryModeBytes = new byte[] { 97, 109, 113, 112, 46, 100, 101, 108, 105, 118, 101, 114, 121, 95, 109, 111, 100, 101 };
        // ExchangeBytes = System.Text.Encoding.UTF8.GetBytes("amqp.exchange");
        private static readonly byte[] ExchangeBytes = new byte[] { 97, 109, 113, 112, 46, 101, 120, 99, 104, 97, 110, 103, 101 };
        // RoutingKeyBytes = System.Text.Encoding.UTF8.GetBytes("messaging.rabbitmq.routing_key");
        private static readonly byte[] RoutingKeyBytes = new byte[] { 109, 101, 115, 115, 97, 103, 105, 110, 103, 46, 114, 97, 98, 98, 105, 116, 109, 113, 46, 114, 111, 117, 116, 105, 110, 103, 95, 107, 101, 121 };
        // QueueBytes = System.Text.Encoding.UTF8.GetBytes("amqp.queue");
        private static readonly byte[] QueueBytes = new byte[] { 97, 109, 113, 112, 46, 113, 117, 101, 117, 101 };

        public override string? GetTag(string key)
        {
            return key switch
            {
                "span.kind" => SpanKind,
                "component" => InstrumentationName,
                "amqp.command" => Command,
                "amqp.delivery_mode" => DeliveryMode,
                "amqp.exchange" => Exchange,
                "messaging.rabbitmq.routing_key" => RoutingKey,
                "amqp.queue" => Queue,
                _ => base.GetTag(key),
            };
        }

        public override void SetTag(string key, string value)
        {
            switch(key)
            {
                case "component": 
                    InstrumentationName = value;
                    break;
                case "amqp.command": 
                    Command = value;
                    break;
                case "amqp.delivery_mode": 
                    DeliveryMode = value;
                    break;
                case "amqp.exchange": 
                    Exchange = value;
                    break;
                case "messaging.rabbitmq.routing_key": 
                    RoutingKey = value;
                    break;
                case "amqp.queue": 
                    Queue = value;
                    break;
                default: 
                    base.SetTag(key, value);
                    break;
            }
        }

        protected static Datadog.Trace.Tagging.IProperty<string?>[] RabbitMQTagsProperties => 
             Datadog.Trace.ExtensionMethods.ArrayExtensions.Concat(MessagingTagsProperties,
                new Datadog.Trace.Tagging.Property<RabbitMQTags, string?>("span.kind", t => t.SpanKind),
                new Datadog.Trace.Tagging.Property<RabbitMQTags, string?>("component", t => t.InstrumentationName),
                new Datadog.Trace.Tagging.Property<RabbitMQTags, string?>("amqp.command", t => t.Command),
                new Datadog.Trace.Tagging.Property<RabbitMQTags, string?>("amqp.delivery_mode", t => t.DeliveryMode),
                new Datadog.Trace.Tagging.Property<RabbitMQTags, string?>("amqp.exchange", t => t.Exchange),
                new Datadog.Trace.Tagging.Property<RabbitMQTags, string?>("messaging.rabbitmq.routing_key", t => t.RoutingKey),
                new Datadog.Trace.Tagging.Property<RabbitMQTags, string?>("amqp.queue", t => t.Queue)
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

            if (Command is not null)
            {
                processor.Process(new TagItem<string>("amqp.command", Command, CommandBytes));
            }

            if (DeliveryMode is not null)
            {
                processor.Process(new TagItem<string>("amqp.delivery_mode", DeliveryMode, DeliveryModeBytes));
            }

            if (Exchange is not null)
            {
                processor.Process(new TagItem<string>("amqp.exchange", Exchange, ExchangeBytes));
            }

            if (RoutingKey is not null)
            {
                processor.Process(new TagItem<string>("messaging.rabbitmq.routing_key", RoutingKey, RoutingKeyBytes));
            }

            if (Queue is not null)
            {
                processor.Process(new TagItem<string>("amqp.queue", Queue, QueueBytes));
            }

            base.EnumerateTags(ref processor);
        }

        protected override Datadog.Trace.Tagging.IProperty<string?>[] GetAdditionalTags()
        {
             return RabbitMQTagsProperties;
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

            if (Command is not null)
            {
                count++;
                WriteTag(ref bytes, ref offset, CommandBytes, Command, tagProcessors);
            }

            if (DeliveryMode is not null)
            {
                count++;
                WriteTag(ref bytes, ref offset, DeliveryModeBytes, DeliveryMode, tagProcessors);
            }

            if (Exchange is not null)
            {
                count++;
                WriteTag(ref bytes, ref offset, ExchangeBytes, Exchange, tagProcessors);
            }

            if (RoutingKey is not null)
            {
                count++;
                WriteTag(ref bytes, ref offset, RoutingKeyBytes, RoutingKey, tagProcessors);
            }

            if (Queue is not null)
            {
                count++;
                WriteTag(ref bytes, ref offset, QueueBytes, Queue, tagProcessors);
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

            if (Command is not null)
            {
                sb.Append("amqp.command (tag):")
                  .Append(Command)
                  .Append(',');
            }

            if (DeliveryMode is not null)
            {
                sb.Append("amqp.delivery_mode (tag):")
                  .Append(DeliveryMode)
                  .Append(',');
            }

            if (Exchange is not null)
            {
                sb.Append("amqp.exchange (tag):")
                  .Append(Exchange)
                  .Append(',');
            }

            if (RoutingKey is not null)
            {
                sb.Append("messaging.rabbitmq.routing_key (tag):")
                  .Append(RoutingKey)
                  .Append(',');
            }

            if (Queue is not null)
            {
                sb.Append("amqp.queue (tag):")
                  .Append(Queue)
                  .Append(',');
            }

            base.WriteAdditionalTags(sb);
        }
    }
}
