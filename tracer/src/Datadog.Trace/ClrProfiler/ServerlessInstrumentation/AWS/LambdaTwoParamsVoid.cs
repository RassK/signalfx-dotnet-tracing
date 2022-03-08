// <copyright file="LambdaTwoParamsVoid.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>

using System;
using System.ComponentModel;

using Datadog.Trace.ClrProfiler.CallTarget;

namespace Datadog.Trace.ClrProfiler.ServerlessInstrumentation.AWS
{
    /// <summary>
    /// Lambda customer handler calltarget instrumentation
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class LambdaTwoParamsVoid
    {
        private static readonly ILambdaExtensionRequest RequestBuilder = new LambdaRequestBuilder();

        /// <summary>
        /// OnMethodBegin callback
        /// </summary>
        /// <typeparam name="TTarget">Type of the target</typeparam>
        /// <typeparam name="TArg1">Type of the incommingEvent</typeparam>
        /// <typeparam name="TArg2">Type of the context</typeparam>
        /// <param name="instance">Instance value, aka `this` of the instrumented method.</param>
        /// <param name="incommingEvent">IncommingEvent value</param>
        /// <param name="context">Context value.</param>
        /// <returns>Calltarget state value</returns>
        internal static CallTargetState OnMethodBegin<TTarget, TArg1, TArg2>(TTarget instance, TArg1 incommingEvent, TArg2 context)
        {
            Serverless.Debug("OnMethodBeginOK - two params");
            return LambdaCommon.StartInvocation(incommingEvent, RequestBuilder);
        }

        /// <summary>
        /// OnMethodEnd callback
        /// </summary>
        /// <typeparam name="TTarget">Type of the target</typeparam>
        /// <param name="instance">Instance value, aka `this` of the instrumented method.</param>
        /// <param name="exception">Exception instance in case the original code threw an exception.</param>
        /// <param name="state">Calltarget state value</param>
        internal static CallTargetReturn OnMethodEnd<TTarget>(TTarget instance, Exception exception, in CallTargetState state)
        {
            Serverless.Debug("OnMethodEnd - two params");
            return LambdaCommon.EndInvocationVoid(exception, state.Scope, RequestBuilder);
        }
    }
}