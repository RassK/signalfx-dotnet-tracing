// <copyright file="IBasicProperties.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

// Modified by Splunk Inc.

using System.Collections.Generic;

namespace Datadog.Trace.ClrProfiler.AutoInstrumentation.RabbitMQ
{
    /// <summary>
    /// BasicProperties interface for ducktyping
    /// </summary>
    internal interface IBasicProperties
    {
        /// <summary>
        /// Gets or sets the headers of the message
        /// </summary>
        /// <returns>Message headers</returns>
        IDictionary<string, object> Headers { get; set; }

        /// <summary>
        /// Gets the delivery mode of the message
        /// </summary>
        byte DeliveryMode { get; }

        /// <summary>
        /// Gets or sets the correlation id
        /// </summary>
        string CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the message id
        /// </summary>
        string MessageId { get; set; }

        /// <summary>
        /// Returns true if the DeliveryMode property is present
        /// </summary>
        /// <returns>true if the DeliveryMode property is present</returns>
        bool IsDeliveryModePresent();
    }
}
