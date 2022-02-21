// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NatsunekoLaboratory.UdonAnalyzer.Extensions;

public static class BaseTypeSyntaxExtensions
{
    public static bool IsInterface(this BaseTypeSyntax obj, SemanticModel model)
    {
        return obj.Type.IsInterface(model);
    }

    public static bool IsClassOf<T>(this BaseTypeSyntax obj, SemanticModel model)
    {
        var symbol = model.Compilation.GetTypeByMetadataName(typeof(T).FullName);
        if (symbol == null)
            return false;
        return obj.IsClassOf(symbol, model);
    }

    public static bool IsClassOf(this BaseTypeSyntax obj, string fullyQualifiedClassName, SemanticModel model)
    {
        return obj.Type.IsClassOf(fullyQualifiedClassName, model);
    }

    public static bool IsClassOf(this BaseTypeSyntax obj, INamedTypeSymbol symbol, SemanticModel model)
    {
        return obj.Type.IsClassOf(symbol, model);
    }
}