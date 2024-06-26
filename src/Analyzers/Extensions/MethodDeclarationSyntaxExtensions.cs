﻿// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NatsunekoLaboratory.UdonAnalyzer.Extensions;

public static class MethodDeclarationSyntaxExtensions
{
    public static bool HasAttribute<TAttribute>(this MethodDeclarationSyntax syntax, SemanticModel model) where TAttribute : Attribute
    {
        if (ModelExtensions.GetDeclaredSymbol(model, syntax) is not IMethodSymbol declaration)
            return false;

        var fullyQualifiedMetadataName = typeof(TAttribute).FullName;
        var attr = model.Compilation.GetTypeByMetadataName(fullyQualifiedMetadataName ?? throw new ArgumentException());
        return declaration.GetAttributes().Any(w => w.AttributeClass?.Equals(attr, SymbolEqualityComparer.Default) == true);
    }

    public static bool HasModifiersExact(this MethodDeclarationSyntax node, params SyntaxKind[] modifiers)
    {
        return node.Modifiers.All(w => modifiers.Contains(w.Kind()));
    }

    public static bool HasModifier(this MethodDeclarationSyntax node, params SyntaxKind[] modifiers)
    {
        return node.Modifiers.Any(w => modifiers.Contains(w.Kind()));
    }
}