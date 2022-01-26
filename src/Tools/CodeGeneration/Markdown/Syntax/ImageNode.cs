// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;

public sealed class ImageNode : InlineNode
{
    private readonly string _url;

    public override string Kind => "Image";

    public ImageNode(string url)
    {
        _url = url;
    }

    public override string ToString()
    {
        return $"![]({_url})";
    }

    public override void WriteTo(MarkdownWriter writer)
    {
        writer.WriteInline($"![]({_url})");
    }

    public override string GetDebuggerDisplay()
    {
        return $"![]({_url})";
    }
}