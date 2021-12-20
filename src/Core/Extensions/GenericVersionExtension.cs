// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using NatsunekoLaboratory.UdonAnalyzer.Internals.Models;

namespace NatsunekoLaboratory.UdonAnalyzer.Extensions;

internal static class GenericVersionExtension
{
    public static bool IsGreaterThan(this GenericVersion obj1, GenericVersion obj2)
    {
        return obj1.CompareTo(obj2) > 0;
    }

    public static bool IsGreaterThanOrEquals(this GenericVersion obj1, GenericVersion obj2)
    {
        return obj1.CompareTo(obj2) >= 0;
    }

    public static bool IsSame(this GenericVersion obj1, GenericVersion obj2)
    {
        return obj1.CompareTo(obj2) == 0;
    }

    public static bool IsLessThan(this GenericVersion obj1, GenericVersion obj2)
    {
        return obj1.CompareTo(obj2) < 0;
    }

    public static bool IsLessThanOrEquals(this GenericVersion obj1, GenericVersion obj2)
    {
        return obj1.CompareTo(obj2) <= 0;
    }
}