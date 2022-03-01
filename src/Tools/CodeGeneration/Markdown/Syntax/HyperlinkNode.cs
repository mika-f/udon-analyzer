// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;

public sealed class HyperlinkNode : InlineNode
{
    private readonly SyntaxNode _node;
    private readonly string _url;

    public override string Kind => "Hyperlink";

    private HyperlinkNode(string url, SyntaxNode node)
    {
        _url = url;
        _node = node;
    }

    public HyperlinkNode(string url, CodeNode text) : this(url, (SyntaxNode)text) { }

    public HyperlinkNode(string url, TextDecorationNode text) : this(url, (SyntaxNode)text) { }

    public HyperlinkNode(string url, ImageNode text) : this(url, (SyntaxNode)text) { }

    public HyperlinkNode(string url, string text) : this(url, new StringNode(text, true)) { }

    public override void WriteTo(MarkdownWriter writer)
    {
        writer.WriteInline("[");
        _node.WriteTo(writer);
        writer.WriteInline($"]({_url})");
    }

    public override string GetDebuggerDisplay()
    {
        return $"[{_node.GetDebuggerDisplay()}]({_url})";
    }
}