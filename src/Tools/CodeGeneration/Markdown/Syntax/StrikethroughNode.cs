// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;

public sealed class StrikethroughNode : TextDecorationNode
{
    private readonly SyntaxNode _node;
    public override string Kind => "Strikethrough";

    private StrikethroughNode(SyntaxNode node)
    {
        _node = node;
    }

    public StrikethroughNode(TextDecorationNode node) : this((SyntaxNode)node) { }

    public StrikethroughNode(string node) : this(new StringNode(node, true)) { }

    public override void WriteTo(MarkdownWriter writer)
    {
        writer.WriteInline(" ~~");
        _node.WriteTo(writer);
        writer.WriteInline("~~ ");
    }

    public override string GetDebuggerDisplay()
    {
        return $"~~{_node.GetDebuggerDisplay()}~~";
    }
}