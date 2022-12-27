// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// -------------------------------------------------------------------------------------------

namespace NatsunekoLaboratory.UdonAnalyzer.Extensions;

internal static class StringExtensions
{
    public static int CountOf(this string obj, string str)
    {
        return (obj.Length - obj.Replace(str, "").Length) / str.Length;
    }
}