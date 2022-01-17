// -----------------------------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the Microsoft Reference Source License. See LICENSE in the project root for license information.
// -----------------------------------------------------------------------------------------------------------------

using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace NatsunekoLaboratory.UdonAnalyzer.Extensions;

public static class TaskExtensions
{
    public static ConfiguredTaskAwaitable Stay(this Task obj)
    {
        return obj.ConfigureAwait(false);
    }

    public static ConfiguredTaskAwaitable<T> Stay<T>(this Task<T> obj)
    {
        return obj.ConfigureAwait(false);
    }
}