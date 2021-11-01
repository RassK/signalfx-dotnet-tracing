// <copyright file="RabbitMQTests.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

// Modified by Splunk Inc.

#if !NET452
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Datadog.Trace.ExtensionMethods;
using Datadog.Trace.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace Datadog.Trace.ClrProfiler.IntegrationTests
{
    public class RabbitMQTests : TestHelper
    {
        private const string ExpectedServiceName = "Samples.RabbitMQ";

        public RabbitMQTests(ITestOutputHelper output)
            : base("RabbitMQ", output)
        {
            SetServiceVersion("1.0.0");
        }

        public static IEnumerable<object[]> GetRabbitMQVersions()
        {
            foreach (object[] item in PackageVersions.RabbitMQ)
            {
                yield return item.Concat(new object[] { false, }).ToArray();
                yield return item.Concat(new object[] { true }).ToArray();
            }
        }

        [SkippableTheory]
        [MemberData(nameof(GetRabbitMQVersions))]
        [Trait("Category", "EndToEnd")]
        public void SubmitsTraces(string packageVersion, bool enableCallTarget)
        {
            SetCallTargetSettings(enableCallTarget);

            var expectedSpanCount = 26;

            int basicPublishCount = 0;
            int basicGetCount = 0;
            int basicDeliverCount = 0;
            int exchangeDeclareCount = 0;
            int queueDeclareCount = 0;
            int queueBindCount = 0;
            var distributedParentSpans = new Dictionary<ulong, int>();

            int emptyBasicGetCount = 0;

            int agentPort = TcpPortProvider.GetOpenPort();
            using (var agent = new MockTracerAgent(agentPort))
            using (RunSampleAndWaitForExit(agent.Port, arguments: $"{TestPrefix}", packageVersion: packageVersion))
            {
                var spans = agent.WaitForSpans(expectedSpanCount); // Do not filter on operation name because they will vary depending on instrumented method
                Assert.True(spans.Count >= expectedSpanCount, $"Expecting at least {expectedSpanCount} spans, only received {spans.Count}");

                var rabbitmqSpans = spans.Where(span => span.Tags.ContainsKey(Tags.InstrumentationName));
                var manualSpans = spans.Where(span => !span.Tags.ContainsKey(Tags.InstrumentationName));

                foreach (var span in rabbitmqSpans)
                {
                    Assert.Equal(SpanTypes.Queue, span.Type);
                    Assert.Equal("rabbitmq", span.Tags[Tags.Messaging.System]);
                    Assert.Equal("RabbitMQ", span.Tags[Tags.InstrumentationName]);
                    Assert.Equal(ExpectedServiceName, span.Service);

                    span.Tags.TryGetValue(Tags.AmqpCommand, out string command);
                    Assert.NotNull(command);

                    if (command.StartsWith("basic.", StringComparison.OrdinalIgnoreCase))
                    {
                        span.Tags.TryGetValue(Tags.Messaging.Operation, out string operation);

                        if (string.Equals(command, "basic.publish", StringComparison.OrdinalIgnoreCase))
                        {
                            basicPublishCount++;

                            Assert.Equal(SpanKinds.Producer, span.Tags[Tags.SpanKind]);
                            Assert.NotNull(span.Tags[Tags.AmqpExchange]);
                            Assert.NotNull(span.Tags[Tags.RabbitMq.RoutingKey]);
                            Assert.NotNull(span.Tags[Tags.Messaging.DestinationKind]);
                            Assert.Null(operation); // We must not set 'send' as operation name

                            AssertSpanName(span, "send");

                            Assert.NotNull(span.Tags[Tags.Messaging.MessageSize]);
                            Assert.True(int.TryParse(span.Tags[Tags.Messaging.MessageSize], out _));

                            // Enforce that the resource name has the following structure: "basic.publish [<default>|{actual exchangeName}] -> [<all>|<generated>|{actual routingKey}]"
                            string regexPattern = @"basic\.publish (?<exchangeName>\S*) -> (?<routingKey>\S*)";
                            var match = Regex.Match(span.Resource, regexPattern);
                            Assert.True(match.Success);

                            var exchangeName = match.Groups["exchangeName"].Value;
                            Assert.True(string.Equals(exchangeName, "<default>") || string.Equals(exchangeName, span.Tags[Tags.AmqpExchange]));

                            var routingKey = match.Groups["routingKey"].Value;
                            Assert.True(string.Equals(routingKey, "<all>") || string.Equals(routingKey, "<generated>") || string.Equals(routingKey, span.Tags[Tags.RabbitMq.RoutingKey]));
                        }
                        else if (string.Equals(command, "basic.get", StringComparison.OrdinalIgnoreCase))
                        {
                            basicGetCount++;

                            Assert.NotNull(span.Tags[Tags.Messaging.DestinationKind]);
                            Assert.NotNull(operation);

                            // Successful responses will have the "message.size" tag
                            // Empty responses will not
                            if (span.Tags.TryGetValue(Tags.Messaging.MessageSize, out string messageSize))
                            {
                                Assert.NotNull(span.ParentId);
                                Assert.True(int.TryParse(messageSize, out _));

                                // Add the parent span ID to a dictionary so we can later assert 1:1 mappings
                                if (distributedParentSpans.TryGetValue(span.ParentId.Value, out int count))
                                {
                                    distributedParentSpans[span.ParentId.Value] = count + 1;
                                }
                                else
                                {
                                    distributedParentSpans[span.ParentId.Value] = 1;
                                }
                            }
                            else
                            {
                                emptyBasicGetCount++;
                            }

                            Assert.NotNull(span.Tags[Tags.AmqpQueue]);

                            // Enforce that the resource name has the following structure: "basic.get [<generated>|{actual queueName}]"
                            string regexPattern = @"basic\.get (?<queueName>\S*)";
                            var match = Regex.Match(span.Resource, regexPattern);
                            Assert.True(match.Success);

                            var queueName = match.Groups["queueName"].Value;
                            Assert.True(string.Equals(queueName, "<generated>") || string.Equals(queueName, span.Tags[Tags.AmqpQueue]));

                            Assert.Equal(SpanKinds.Consumer, span.Tags[Tags.SpanKind]);
                            AssertSpanName(span, "receive");
                        }
                        else if (string.Equals(command, "basic.deliver", StringComparison.OrdinalIgnoreCase))
                        {
                            basicDeliverCount++;

                            Assert.NotNull(span.ParentId);
                            Assert.NotNull(operation);

                            // Add the parent span ID to a dictionary so we can later assert 1:1 mappings
                            if (distributedParentSpans.TryGetValue(span.ParentId.Value, out int count))
                            {
                                distributedParentSpans[span.ParentId.Value] = count + 1;
                            }
                            else
                            {
                                distributedParentSpans[span.ParentId.Value] = 1;
                            }

                            Assert.Equal(SpanKinds.Consumer, span.Tags[Tags.SpanKind]);
                            // Assert.NotNull(span.Tags[Tags.AmqpQueue]); // Java does this but we're having difficulty doing this. Push to v2?
                            Assert.NotNull(span.Tags[Tags.AmqpExchange]);
                            Assert.NotNull(span.Tags[Tags.RabbitMq.RoutingKey]);
                            Assert.NotNull(span.Tags[Tags.Messaging.DestinationKind]);

                            Assert.NotNull(span.Tags[Tags.Messaging.MessageSize]);
                            Assert.True(int.TryParse(span.Tags[Tags.Messaging.MessageSize], out _));

                            // Enforce that the resource name has the following structure: "basic.deliver [<generated>|{actual queueName}]"
                            string regexPattern = @"basic\.deliver (?<queueName>\S*)";
                            var match = Regex.Match(span.Resource, regexPattern);
                            // Assert.True(match.Success); // Enable once we can get the queue name included

                            var queueName = match.Groups["queueName"].Value;
                            // Assert.True(string.Equals(queueName, "<generated>") || string.Equals(queueName, span.Tags[Tags.AmqpQueue])); // Enable once we can get the queue name included

                            AssertSpanName(span, "receive");
                        }
                        else
                        {
                            throw new Xunit.Sdk.XunitException($"amqp.command {command} not recognized.");
                        }
                    }
                    else
                    {
                        /* Setup commands, not supported by OTel */

                        Assert.Equal(SpanKinds.Client, span.Tags[Tags.SpanKind]);
                        Assert.Equal(command, span.Resource);

                        if (string.Equals(command, "exchange.declare", StringComparison.OrdinalIgnoreCase))
                        {
                            exchangeDeclareCount++;

                            Assert.NotNull(span.Tags[Tags.AmqpExchange]);
                        }
                        else if (string.Equals(command, "queue.declare", StringComparison.OrdinalIgnoreCase))
                        {
                            queueDeclareCount++;

                            Assert.NotNull(span.Tags[Tags.AmqpQueue]);
                        }
                        else if (string.Equals(command, "queue.bind", StringComparison.OrdinalIgnoreCase))
                        {
                            queueBindCount++;

                            Assert.NotNull(span.Tags[Tags.AmqpExchange]);
                            Assert.NotNull(span.Tags[Tags.AmqpQueue]);
                            Assert.NotNull(span.Tags[Tags.RabbitMq.RoutingKey]);
                        }
                        else
                        {
                            throw new Xunit.Sdk.XunitException($"amqp.command {command} not recognized.");
                        }
                    }
                }

                foreach (var span in manualSpans)
                {
                    Assert.Equal(ExpectedServiceName, span.Service);
                    Assert.Equal("1.0.0", span.Tags[Tags.Version]);
                }
            }

            // Assert that all empty get results are expected
            Assert.Equal(2, emptyBasicGetCount);

            // Assert that each span that started a distributed trace (basic.publish)
            // has only one child span (basic.deliver or basic.get)
            Assert.All(distributedParentSpans, kvp => Assert.Equal(1, kvp.Value));

            Assert.Equal(5, basicPublishCount);
            Assert.Equal(4, basicGetCount);
            Assert.Equal(3, basicDeliverCount);

            Assert.Equal(1, exchangeDeclareCount);
            Assert.Equal(1, queueBindCount);
            Assert.Equal(4, queueDeclareCount);
        }

        private void AssertSpanName(MockTracerAgent.Span span, string operation)
        {
            span.Tags.TryGetValue(Tags.Messaging.Destination, out string destination);

            if (string.IsNullOrWhiteSpace(destination))
            {
                Assert.Equal(span.Name, operation);
            }
            else
            {
                Assert.Equal(span.Name, $"{destination} {operation}");
            }
        }
    }
}
#endif
