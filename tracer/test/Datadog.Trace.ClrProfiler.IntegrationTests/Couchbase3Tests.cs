// <copyright file="Couchbase3Tests.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

// Modified by Splunk Inc.

using System.Collections.Generic;
using System.Linq;
using Datadog.Trace.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace Datadog.Trace.ClrProfiler.IntegrationTests
{
    public class Couchbase3Tests : TestHelper
    {
        public Couchbase3Tests(ITestOutputHelper output)
            : base("Couchbase3", output)
        {
            SetServiceVersion("1.0.0");
        }

        public static System.Collections.Generic.IEnumerable<object[]> GetCouchbase()
        {
            foreach (var item in PackageVersions.Couchbase3)
            {
                yield return item.ToArray();
            }
        }

        // [SkippableTheory]
        [Theory]
        [MemberData(nameof(GetCouchbase), Skip = "TODO enable Couchbase3 tests")]
        [Trait("Category", "EndToEnd")]
        [Trait("Category", "ArmUnsupported")]
        public void SubmitTraces(string packageVersion)
        {
            int agentPort = TcpPortProvider.GetOpenPort();
            using (var agent = new MockTracerAgent(agentPort))
            using (RunSampleAndWaitForExit(agent.Port, packageVersion: packageVersion))
            {
                var spans = agent.WaitForSpans(9, 500)
                                 .Where(s => s.Type == "db")
                                 .ToList();

                Assert.True(spans.Count >= 9, $"Expecting at least 9 spans, only received {spans.Count}");

                foreach (var span in spans)
                {
                    Assert.Equal("couchbase.query", span.Name);
                    Assert.Equal("Samples.Couchbase3", span.Service);
                    Assert.Contains(Tags.Version, (IDictionary<string, string>)span.Tags);
                }

                var expected = new List<string>
                {
                    "Hello", "Hello", "GetErrorMap", "GetErrorMap", "SelectBucket", "SelectBucket",
                    "Set", "Get", "Delete"
                };

                if (packageVersion == "3.0.7")
                {
                    expected.Remove("Get");
                    expected.Add("MultiLookup");
                }

                ValidateSpans(spans, (span) => span.Resource, expected);
            }
        }
    }
}