// <auto-generated/> (not auto-generated but a hack to exclude from StyleCop)
#nullable enable annotations
#pragma warning disable CS1591


// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;

namespace Vendored.System.Diagnostics
{
    partial class Activity
    {
        private static string GenerateRootId()
        {
            // It is important that the part that changes frequently be first, because
            // some sampling functions don't sample from the high entropy part of their hash function.
            // This makes sampling based on this produce poor samples.
            return '|' + Interlocked.Increment(ref s_currentRootId).ToString("x") + s_uniqSuffix;
        }
    }
}