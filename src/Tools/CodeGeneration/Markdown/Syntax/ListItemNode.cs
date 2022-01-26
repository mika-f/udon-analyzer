// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;

public sealed class ListItemNode : BlockNode
{
    private readonly SyntaxNode _node;

    public override string Kind => "ListItem";


    private ListItemNode(SyntaxNode node)
    {
        _node = node;
    }

    public ListItemNode(TextDecorationNode node) : this((SyntaxNode)node) { }

    public ListItemNode(ListNode node) : this((SyntaxNode)node) { }

    public ListItemNode(string node) : this(new StringNode(node)) { }

    public override void WriteTo(MarkdownWriter writer)
    {
        if (_node is ListNode)
        {
            writer.IncreaseIndent(4);
            _node.WriteTo(writer);
            writer.DecreaseIndent(4);
            return;
        }

        writer.PushPrefix("* ");
        _node.WriteTo(writer);
        writer.PopPrefix();
    }

    public override string GetDebuggerDisplay()
    {
        return $"* {_node.GetDebuggerDisplay()}";
    }
}