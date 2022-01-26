// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;

public sealed class ContainerNode : SyntaxNode, IContainer
{
    private readonly List<SyntaxNode> _nodes;

    public ContainerNode(params object[] contents)
    {
        if (contents.All(w => w is string or SyntaxNode))
            _nodes = contents.Select(w => w is string str ? new StringNode(str) : w as SyntaxNode).Cast<SyntaxNode>().ToList();
        else
            throw new ArgumentException(nameof(contents));
    }

    public override string Kind => "Container";

    public override void WriteTo(MarkdownWriter writer)
    {
        foreach (var node in _nodes)
            node.WriteTo(writer);
    }

    public override string GetDebuggerDisplay()
    {
        return string.Join("", _nodes.Select(w => w.GetDebuggerDisplay()));
    }

    public void Add(ISyntaxNode node)
    {
        if (node is SyntaxNode n)
            _nodes.Add(n);
    }
}