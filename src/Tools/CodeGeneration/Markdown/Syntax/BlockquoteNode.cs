// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using NatsunekoLaboratory.UdonAnalyzer.Extensions;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;

public sealed class BlockquoteNode : SyntaxNode, IContainer
{
    private readonly List<SyntaxNode> _nodes;

    public BlockquoteNode(params object[] nodes)
    {
        if (nodes.All(w => w is string or SyntaxNode))
            _nodes = nodes.Select(w => w is string str ? new StringNode(str) : w as SyntaxNode).Cast<SyntaxNode>().ToList();
        else
            throw new ArgumentException(nameof(nodes));
    }

    public override string Kind => "Blockquote";

    public override void WriteTo(MarkdownWriter writer)
    {
        var isAlreadyInBlockquote = writer.PeekPrefix()?.StartsWith('>') ?? false;
        if (isAlreadyInBlockquote)
        {
            var c = writer.PeekPrefix()!.Length - 1;
            var s = ">";

            c.Times(() => s += ">");

            writer.PushPrefix(s + " ");
        }
        else
        {
            writer.PushPrefix("> ");
        }

        foreach (var node in _nodes)
            node.WriteTo(writer);

        writer.PopPrefix();
    }

    public override string GetDebuggerDisplay()
    {
        return string.Join(Environment.NewLine, _nodes.Select(w => w.GetDebuggerDisplay()));
    }

    public void Add(ISyntaxNode node)
    {
        if (node is SyntaxNode n)
            _nodes.Add(n);
    }
}