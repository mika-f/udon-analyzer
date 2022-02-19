// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.CSharp.Syntax;

internal static class SyntaxFactory2
{
    public static SyntaxList<TNode> CreateSyntaxList<TNode>(params TNode[] nodes) where TNode : SyntaxNode
    {
        return CreateSyntaxList(nodes.ToList());
    }

    public static SyntaxList<TNode> CreateSyntaxList<TNode>(List<TNode> nodes) where TNode : SyntaxNode
    {
        return new SyntaxList<TNode>(nodes);
    }

    public static SeparatedSyntaxList<TNode> CreateSeparatedSyntaxList<TNode>(params TNode[] nodes) where TNode : SyntaxNode
    {
        return new SeparatedSyntaxList<TNode>().AddRange(nodes);
    }


    public static TWrapper Create<TWrapper, TNode>(Func<SeparatedSyntaxList<TNode>, TWrapper> initializer, params TNode[] nodes) where TWrapper : SyntaxNode where TNode : SyntaxNode
    {
        return initializer.Invoke(CreateSeparatedSyntaxList(nodes));
    }

    public static SyntaxList<TNode> Empty<TNode>() where TNode : SyntaxNode
    {
        return new SyntaxList<TNode>();
    }

    public static AttributeSyntax Attribute(string identifier, params ExpressionSyntax[] expressions)
    {
        var args = SyntaxFactory.AttributeArgumentList(CreateSeparatedSyntaxList(expressions.Select(SyntaxFactory.AttributeArgument).ToArray()));
        return SyntaxFactory.Attribute(SyntaxFactory.ParseName(identifier), args);
    }

    public static SyntaxTokenList Modifiers(params SyntaxKind[] kinds)
    {
        return new SyntaxTokenList(kinds.Select(SyntaxFactory.Token).ToArray());
    }
}