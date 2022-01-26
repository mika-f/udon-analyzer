// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;

using NatsunekoLaboratory.UdonAnalyzer.Extensions;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;

public sealed class HeadingNode : BlockNode
{
    private readonly int _level;
    private readonly SyntaxNode _node;

    public override string Kind => "Heading";

    private HeadingNode(SyntaxNode node, int level)
    {
        switch (level)
        {
            case < 1:
                throw new ArgumentOutOfRangeException(nameof(level));

            case > 6:
                throw new ArgumentOutOfRangeException(nameof(level));

            default:
                _node = node;
                _level = level;
                break;
        }
    }

    public HeadingNode(HyperlinkNode node, int level) : this((SyntaxNode)node, level) { }

    public HeadingNode(ImageNode node, int level) : this((SyntaxNode)node, level) { }

    public HeadingNode(ItalicNode node, int level) : this((SyntaxNode)node, level) { }

    public HeadingNode(CodeNode node, int level) : this((SyntaxNode)node, level) { }

    public HeadingNode(string node, int level) : this(new StringNode(node, true), level) { }

    public override void WriteTo(MarkdownWriter writer)
    {
        writer.WriteInline("#");
        (_level - 1).Times(() => writer.WriteInline('#'));
        writer.WriteInline(' ');

        _node.WriteTo(writer);
        writer.WriteNewLine();
        writer.WriteNewLine();
    }

    public override string GetDebuggerDisplay()
    {
        return $"# ({_level}) {_node.GetDebuggerDisplay()}";
    }
}