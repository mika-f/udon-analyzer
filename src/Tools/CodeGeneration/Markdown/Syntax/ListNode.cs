// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;

public sealed class ListNode : SyntaxNode, IContainer
{
    private readonly List<ListItemNode> _nodes;

    public override string Kind => "List";

    public ListNode(params ListItemNode[] nodes)
    {
        _nodes = nodes.ToList();
    }

    public void Add(ISyntaxNode node)
    {
        if (node is ListItemNode item)
            _nodes.Add(item);
        else
            throw new ArgumentException();
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var node in _nodes)
            sb.AppendLine(node.ToString());

        return sb.ToString();
    }

    public override void WriteTo(MarkdownWriter writer)
    {
        foreach (var node in _nodes)
            node.WriteTo(writer);
    }

    public override string GetDebuggerDisplay()
    {
        return string.Join(Environment.NewLine, _nodes.Select(w => w.GetDebuggerDisplay()));
    }
}