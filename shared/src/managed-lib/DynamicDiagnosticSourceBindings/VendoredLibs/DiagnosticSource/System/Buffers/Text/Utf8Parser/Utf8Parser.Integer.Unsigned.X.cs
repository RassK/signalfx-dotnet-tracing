// <auto-generated/> (not auto-generated but a hack to exclude from StyleCop)
#nullable enable annotations
#pragma warning disable CS1591


// Modified excerpt from dotnet/runtime. This version contains only
// the types, methods, and interfaces used by Vendored.System.Diagnostics.DiagnosticSource.csproj

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Vendored.System.Buffers.Text
{
    public static partial class Utf8Parser
    {
        internal static bool TryParseUInt64X(byte[] source, out ulong value, out int bytesConsumed)
        {
            if (source.Length < 1)
            {
                bytesConsumed = 0;
                value = default;
                return false;
            }
            byte nextCharacter;
            byte nextDigit;

            byte[] hexLookup = HexConverter.CharToHexLookup;

            // Parse the first digit separately. If invalid here, we need to return false.
            nextCharacter = source[0];
            nextDigit = hexLookup[nextCharacter];
            if (nextDigit == 0xFF)
            {
                bytesConsumed = 0;
                value = default;
                return false;
            }
            ulong parsedValue = nextDigit;

            if (source.Length <= ParserHelpers.Int64OverflowLengthHex)
            {
                // Length is less than or equal to Parsers.Int64OverflowLengthHex; overflow is not possible
                for (int index = 1; index < source.Length; index++)
                {
                    nextCharacter = source[index];
                    nextDigit = hexLookup[nextCharacter];
                    if (nextDigit == 0xFF)
                    {
                        bytesConsumed = index;
                        value = parsedValue;
                        return true;
                    }
                    parsedValue = (parsedValue << 4) + nextDigit;
                }
            }
            else
            {
                // Length is greater than Parsers.Int64OverflowLengthHex; overflow is only possible after Parsers.Int64OverflowLengthHex
                // digits. There may be no overflow after Parsers.Int64OverflowLengthHex if there are leading zeroes.
                for (int index = 1; index < ParserHelpers.Int64OverflowLengthHex; index++)
                {
                    nextCharacter = source[index];
                    nextDigit = hexLookup[nextCharacter];
                    if (nextDigit == 0xFF)
                    {
                        bytesConsumed = index;
                        value = parsedValue;
                        return true;
                    }
                    parsedValue = (parsedValue << 4) + nextDigit;
                }
                for (int index = ParserHelpers.Int64OverflowLengthHex; index < source.Length; index++)
                {
                    nextCharacter = source[index];
                    nextDigit = hexLookup[nextCharacter];
                    if (nextDigit == 0xFF)
                    {
                        bytesConsumed = index;
                        value = parsedValue;
                        return true;
                    }
                    // If we try to append a digit to anything larger than ulong.MaxValue / 0x10, there will be overflow
                    if (parsedValue > ulong.MaxValue / 0x10)
                    {
                        bytesConsumed = 0;
                        value = default;
                        return false;
                    }
                    parsedValue = (parsedValue << 4) + nextDigit;
                }
            }

            bytesConsumed = source.Length;
            value = parsedValue;
            return true;
        }
    }
}