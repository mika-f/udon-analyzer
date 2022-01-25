// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NatsunekoLaboratory.UdonAnalyzer.Extensions;

public static class AttributeSyntaxExtensions
{
    public static bool IsEquivalentType<TAttribute>(this AttributeSyntax syntax, SemanticModel model) where TAttribute : Attribute
    {
        return IsEquivalentType(syntax, typeof(TAttribute), model);
    }

    public static bool IsEquivalentType(this AttributeSyntax syntax, Type t, SemanticModel model)
    {
        return IsEquivalentType(syntax, t.FullName ?? throw new InvalidOperationException(), model);
    }

    public static bool IsEquivalentType(this AttributeSyntax syntax, string fullyQualifiedMetadataName, SemanticModel model)
    {
        return IsEquivalentType(syntax, model.Compilation.GetTypeByMetadataName(fullyQualifiedMetadataName) ?? throw new InvalidOperationException(), model);
    }

    public static bool IsEquivalentType(this AttributeSyntax syntax, INamedTypeSymbol symbol, SemanticModel model)
    {
        var s = model.GetSymbolInfo(syntax);
        if (s.Symbol is not IMethodSymbol method)
            return false;

        return method.ReceiverType?.Equals(symbol, SymbolEqualityComparer.Default) == true;
    }

    public static TReturn? GetAttributeValue<TAttribute, TReturn>(this AttributeSyntax syntax, string name, SemanticModel model) where TReturn : class where TAttribute : Attribute
    {
        var info = model.GetSymbolInfo(syntax);
        if (info.Symbol is not IMethodSymbol symbol)
            return default;

        var constructor = typeof(TAttribute).FindBestMatchingConstructor(symbol, model);
        if (constructor == null)
            return default;

        var arg = constructor.GetParameters().Select((w, i) => (Item: w, Index: i)).FirstOrDefault(w => w.Item.Name == name);
        return arg == default ? default : GetAttributeValue<TReturn>(syntax, arg.Index, model);
    }

    public static TReturn? GetAttributeValue<TReturn>(this AttributeSyntax syntax, int index, SemanticModel model) where TReturn : class
    {
        return syntax.ArgumentList?.Arguments[index].InvokeConstantValue(model) as TReturn;
    }
}