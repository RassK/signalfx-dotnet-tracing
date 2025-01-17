// <copyright file="IRequestData.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

// Modified by Splunk Inc.

using System;

namespace Datadog.Trace.ClrProfiler.AutoInstrumentation.Elasticsearch
{
    /// <summary>
    /// Version-agnostic interface for Elasticsearch RequestData
    /// </summary>
    internal interface IRequestData
    {
        /// <summary>
        /// Gets the path of the request
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Gets the URI of the request
        /// </summary>
        Uri Uri { get; }

        /// <summary>
        /// Gets the HTTP method of the request
        /// </summary>
        string Method { get; }

        /// <summary>
        /// Gets the ConnectionSettings of the request
        /// </summary>
        object ConnectionSettings { get; }

        /// <summary>
        /// Gets the data to post of the request
        /// </summary>
        public IPostData PostData { get; }
    }
}
