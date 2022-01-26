// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;

public sealed class TableBodyNode : InlineNode, IContainer
{
    private readonly List<SyntaxNode> _rows;

    public TableBodyNode(params object[] rows)
    {
        if (rows.All(w => w is string or InlineNode))
            _rows = rows.Select(w => w is string str ? new StringNode(str) : w as SyntaxNode).Cast<SyntaxNode>().ToList();
        else
            throw new ArgumentException(nameof(rows));
    }

    public override string Kind => "TB";

    public override void WriteTo(MarkdownWriter writer)
    {
        writer.WriteInline("| ");

        foreach (var (node, row) in _rows.Select((w, i) => (w, i)))
        {
            node.WriteTo(writer);
            writer.FillTo(GetRowLength(row), row);
            writer.WriteInline(" | ");
        }

        writer.WriteNewLine();
    }

    public override string GetDebuggerDisplay()
    {
        return "OMITTED";
    }

    public void Add(ISyntaxNode node)
    {
        if (node is SyntaxNode n)
            _rows.Add(n);
    }

    public int GetRowLength(int row)
    {
        if (_rows.Count <= row)
            throw new ArgumentOutOfRangeException(nameof(row));

        var mw = new MarkdownWriter();
        _rows[row].WriteTo(mw);

        return mw.Length;
    }
}