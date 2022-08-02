// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NatsunekoLaboratory.UdonAnalyzer.Extensions;

public static class ClassDeclarationSyntaxExtensions
{
    public static bool IsInheritOf<TClass>(this ClassDeclarationSyntax syntax, SemanticModel model)
    {
        return IsInheritOf(syntax, typeof(TClass), model);
    }

    public static bool IsInheritOf(this ClassDeclarationSyntax syntax, Type t, SemanticModel model)
    {
        return IsInheritOf(syntax, t.FullName ?? throw new InvalidOperationException(), model);
    }

    public static bool IsInheritOf(this ClassDeclarationSyntax syntax, string fullyQualifiedMetadataName, SemanticModel model)
    {
        if (model.GetDeclaredSymbol(syntax) is not INamedTypeSymbol @class)
            return false;

        var symbol = model.Compilation.GetTypeByMetadataName(fullyQualifiedMetadataName);
        return @class.BaseType?.Equals(symbol, SymbolEqualityComparer.Default) == true;
    }

    public static bool HasAttribute<TAttribute>(this ClassDeclarationSyntax syntax, SemanticModel model) where TAttribute : Attribute
    {
        return HasAttribute(syntax, typeof(TAttribute), model);
    }

    public static bool HasAttribute(this ClassDeclarationSyntax syntax, Type t, SemanticModel model)
    {
        return HasAttribute(syntax, t.FullName ?? throw new InvalidOperationException(), model);
    }

    public static bool HasAttribute(this ClassDeclarationSyntax syntax, string fullyQualifiedMetadataName, SemanticModel model)
    {
        var symbol = model.Compilation.GetTypeByMetadataName(fullyQualifiedMetadataName);
        if (symbol == null)
            return false;

        return syntax.AttributeLists
                     .SelectMany(w => w.Attributes)
                     .Any(w => w.IsEquivalentType(symbol, model));
    }

    public static AttributeSyntax? GetAttribute<TAttribute>(this ClassDeclarationSyntax syntax, SemanticModel model) where TAttribute : Attribute
    {
        return GetAttribute(syntax, typeof(TAttribute), model);
    }

    public static AttributeSyntax? GetAttribute(this ClassDeclarationSyntax syntax, Type t, SemanticModel model)
    {
        var fullyQualifiedMetadataName = t.FullName ?? throw new InvalidOperationException();
        return GetAttribute(syntax, fullyQualifiedMetadataName, model);
    }

    public static AttributeSyntax? GetAttribute(this ClassDeclarationSyntax syntax, string fullyQualifiedMetadataName, SemanticModel model)
    {
        var symbol = model.Compilation.GetTypeByMetadataName(fullyQualifiedMetadataName);
        if (symbol == null)
            return null;

        return syntax.AttributeLists
                     .SelectMany(w => w.Attributes)
                     .FirstOrDefault(w => w.IsEquivalentType(symbol, model));
    }
}