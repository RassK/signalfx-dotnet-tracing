// <copyright file="ConfigureCiCommandTests.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

// Modified by Splunk Inc.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Datadog.Trace.Tools.Runner.IntegrationTests
{
    [Collection(nameof(ConsoleTestsCollection))]
    public class ConfigureCiCommandTests
    {
        [Fact(Skip = "We are not using this tool in SingalFx")]
        public void ConfigureCi()
        {
            var commandLine = "ci configure azp --dd-env TestEnv --dd-service TestService --dd-version TestVersion --tracer-home TestTracerHome --agent-url TestAgentUrl";

            using var console = ConsoleHelper.Redirect();

            var result = Program.Main(commandLine.Split(' '));

            result.Should().Be(0);

            var environmentVariables = new Dictionary<string, string>();

            foreach (var line in console.ReadLines())
            {
                // ##vso[task.setvariable variable=SIGNALFX_DOTNET_TRACER_HOME]TestTracerHome
                var match = Regex.Match(line, @"##vso\[task.setvariable variable=(?<name>[A-Z1-9_]+)\](?<value>.*)");

                if (match.Success)
                {
                    environmentVariables.Add(match.Groups["name"].Value, match.Groups["value"].Value);
                }
            }

            environmentVariables.Should().Contain("SIGNALFX_ENV", "TestEnv");
            environmentVariables.Should().Contain("SIGNALFX_SERVICE_NAME", "TestService");
            environmentVariables.Should().Contain("SIGNALFX_VERSION", "TestVersion");
            environmentVariables.Should().Contain("SIGNALFX_DOTNET_TRACER_HOME", "TestTracerHome");
            environmentVariables.Should().Contain("SIGNALFX_DOTNET_TRACER_HOME", "TestAgentUrl");
        }

        [Theory(Skip = "We are not using this tool in SingalFx")]
        [InlineData("TF_BUILD", "1", 0, "Detected CI AzurePipelines.")]
        [InlineData("Nope", "0", 1, "Failed to autodetect CI.")]
        public void AutodetectCi(string key, string value, int expectedStatusCode, string expectedMessage)
        {
            var originalEnvVars = Environment.GetEnvironmentVariables();

            // Clear all environment variables
            foreach (string envKey in originalEnvVars.Keys)
            {
                Environment.SetEnvironmentVariable(envKey, null);
            }

            try
            {
                Environment.SetEnvironmentVariable(key, value);

                var commandLine = "ci configure --tracer-home tracerHome";

                using var console = ConsoleHelper.Redirect();

                var result = Program.Main(commandLine.Split(' '));

                result.Should().Be(expectedStatusCode);

                console.Output.Should().Contain(expectedMessage);
            }
            finally
            {
                // Restore all environment variables
                // Clear all environment variables
                foreach (string envKey in originalEnvVars.Keys)
                {
                    Environment.SetEnvironmentVariable(envKey, (string)originalEnvVars[envKey]);
                }
            }
        }
    }
}