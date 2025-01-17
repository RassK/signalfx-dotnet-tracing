// <copyright file="TracerMetricNames.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

// Modified by Splunk Inc.

namespace Datadog.Trace.DogStatsd
{
    internal static class TracerMetricNames
    {
        public static class Api
        {
            /// <summary>
            /// Count: Total number of API requests made
            /// </summary>
            public const string Requests = "datadog.tracer.api.requests";

            /// <summary>
            /// Count: Count of API responses.
            /// This metric has an additional tag of "status: {code}" to group the responses by the HTTP response code.
            /// This is different from <seealso cref="Errors"/> in that this is all HTTP responses
            /// regardless of status code, and <seealso cref="Errors"/> is exceptions raised from making an API call.
            /// </summary>
            public const string Responses = "datadog.tracer.api.responses";

            /// <summary>
            /// Count: Total number of exceptions raised by API calls.
            /// This is different from receiving a 4xx or 5xx response.
            /// It is a "timeout error" or something from making the API call.
            /// </summary>
            public const string Errors = "datadog.tracer.api.errors";
        }

        public static class Queue
        {
            /// <summary>
            /// Count: Total number of traces pushed into the queue (does not include traces dropped due to a full queue)
            /// </summary>
            public const string EnqueuedTraces = "signalfx.tracer.queue.enqueued_traces";

            /// <summary>
            /// Count: Total number of spans pushed into the queue (does not include traces dropped due to a full queue)
            /// </summary>
            public const string EnqueuedSpans = "signalfx.tracer.queue.enqueued_spans";

            /// <summary>
            /// Count: Total size in bytes of traces pushed into the queue (does not include traces dropped due to a full queue)
            /// </summary>
            public const string EnqueuedBytes = "signalfx.tracer.queue.enqueued_bytes";

            /// <summary>
            /// Count: Total number of traces dropped due to a full queue
            /// </summary>
            public const string DroppedTraces = "signalfx.tracer.queue.dropped_traces";

            /// <summary>
            /// Count: Total number of spans dropped due to a full queue
            /// </summary>
            public const string DroppedSpans = "signalfx.tracer.queue.dropped_spans";

            /// <summary>
            /// Count: Number of traces pulled from the queue for flushing
            /// </summary>
            public const string DequeuedTraces = "signalfx.tracer.queue.dequeued_traces";

            /// <summary>
            /// Count: Total number of spans pulled from the queue for flushing
            /// </summary>
            public const string DequeuedSpans = "signalfx.tracer.queue.dequeued_spans";

            /// <summary>
            /// Count: Size in bytes of traces pulled from the queue for flushing
            /// </summary>
            public const string DequeuedBytes = "signalfx.tracer.queue.dequeued_bytes";
        }

        public static class Health
        {
            /// <summary>
            /// Gauge: Set to 1 by each Tracer instance.
            /// </summary>
            public const string Heartbeat = "signalfx.tracer.heartbeat";

            /// <summary>
            /// Count: The number of exceptions thrown by the Tracer.
            /// </summary>
            public const string Exceptions = "signalfx.tracer.health.exceptions";

            /// <summary>
            /// Count: The number of warnings generated by the Tracer.
            /// </summary>
            public const string Warnings = "signalfx.tracer.health.warnings";
        }
    }
}
