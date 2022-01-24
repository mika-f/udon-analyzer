// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;

using Microsoft.CodeAnalysis;

namespace NatsunekoLaboratory.UdonAnalyzer.Extensions;

// ReSharper disable once InconsistentNaming
public static class INamedTypeSymbolExtensions
{
    public static Type? InvokeAsType(this INamedTypeSymbol symbol)
    {
        return Type.GetType($"{symbol.ToDisplayString()}, {symbol.ContainingAssembly.ToDisplayString()}");
    }
}