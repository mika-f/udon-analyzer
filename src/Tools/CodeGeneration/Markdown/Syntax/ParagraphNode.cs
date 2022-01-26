// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;

public class ParagraphNode : BlockNode
{
    private readonly SyntaxNode _node;

    public override string Kind => "Paragraph";

    private ParagraphNode(SyntaxNode node)
    {
        _node = node;
    }

    public ParagraphNode(InlineNode node) : this((SyntaxNode)node) { }

    public ParagraphNode(string text) : this(new StringNode(text)) { }

    public override void WriteTo(MarkdownWriter writer)
    {
        _node.WriteTo(writer);
        writer.WriteNewLineToEol();
    }

    public override string GetDebuggerDisplay()
    {
        return $"{_node.GetDebuggerDisplay()}<br />";
    }
}