// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;

public sealed class ItalicNode : TextDecorationNode
{
    private readonly SyntaxNode _node;

    public override string Kind => "Italic";

    private ItalicNode(SyntaxNode node)
    {
        _node = node;
    }

    public ItalicNode(TextDecorationNode node) : this((SyntaxNode)node) { }

    public ItalicNode(string node) : this(new StringNode(node, true)) { }

    public override void WriteTo(MarkdownWriter writer)
    {
        writer.WriteInline("_");
        _node.WriteTo(writer);
        writer.WriteInline("_");
    }

    public override string GetDebuggerDisplay()
    {
        return $"_{_node.GetDebuggerDisplay()}_";
    }
}