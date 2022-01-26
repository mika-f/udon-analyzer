// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;

public sealed class TableNode : BlockNode
{
    private readonly List<TableBodyNode> _bodies;
    private readonly TableHeadNode _head;

    public override string Kind => "Table";

    public TableNode(TableHeadNode header, params TableBodyNode[] bodies)
    {
        _head = header;
        _bodies = bodies.ToList();
    }

    public override void WriteTo(MarkdownWriter writer)
    {
        var rows = _head.GetRows();
        var fills = new List<int>();
        for (var i = 0; i < rows; i++)
        {
            var hm = _head.GetRowLength(i);
            var bm = _bodies.Max(w => w.GetRowLength(i));

            fills.Add(bm >= hm ? bm : hm);
        }

        writer.SetTableRowFills(fills.ToArray());
        _head.WriteTo(writer);
        foreach (var body in _bodies)
            body.WriteTo(writer);
        writer.WriteNewLine();
    }

    public override string GetDebuggerDisplay()
    {
        return "OMITTED";
    }
}