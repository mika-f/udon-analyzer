// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NatsunekoLaboratory.UdonAnalyzer.Extensions;

public static class TypeSyntaxExtensions
{
    public static bool IsInterface(this TypeSyntax obj, SemanticModel model)
    {
        var info = model.GetSymbolInfo(obj);
        if (info.Symbol is not INamedTypeSymbol symbol)
            return false;

        return symbol.TypeKind == TypeKind.Interface;
    }
}