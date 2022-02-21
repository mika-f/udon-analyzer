// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Linq;

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

    public static bool IsClassOf(this TypeSyntax obj, string fullyQualifiedClassName, SemanticModel model)
    {
        var info = model.GetSymbolInfo(obj);
        if (info.Symbol is not INamedTypeSymbol t)
        {
            if (info.CandidateSymbols.None())
                return false;
            return info.CandidateSymbols.Any(w => w.ToDisplayString() == fullyQualifiedClassName);
        }

        return t.ToDisplayString() == fullyQualifiedClassName;
    }


    public static bool IsClassOf(this TypeSyntax obj, INamedTypeSymbol symbol, SemanticModel model)
    {
        var info = model.GetSymbolInfo(obj);
        if (info.Symbol is not INamedTypeSymbol t)
        {
            if (info.CandidateSymbols.None())
                return false;
            return info.CandidateSymbols.Any(w => w.Equals(symbol, SymbolEqualityComparer.Default));
        }

        return t.Equals(symbol, SymbolEqualityComparer.Default);
    }
}