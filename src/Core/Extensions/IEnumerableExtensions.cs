// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// -------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace NatsunekoLaboratory.UdonAnalyzer.Extensions;

// ReSharper disable once InconsistentNaming
public static class IEnumerableExtensions
{
    public static bool None<TSource>(this IEnumerable<TSource> obj)
    {
        return !obj.Any();
    }

    public static bool None<TSource>(this IEnumerable<TSource> obj, Func<TSource, bool> predicate)
    {
        return !obj.Any(predicate);
    }

    public static IEnumerable<TSource> NotNull<TSource>(this IEnumerable<TSource?> obj) where TSource : class
    {
        return obj.Where(w => w != null).Select(w => w!);
    }
}