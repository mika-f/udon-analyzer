// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Extensions;

public static class TextDecorationNodeExtensions
{
    public static ItalicNode Italic(this TextDecorationNode node)
    {
        return new ItalicNode(node);
    }

    public static BoldNode Bold(this TextDecorationNode node)
    {
        return new BoldNode(node);
    }

    public static StrikethroughNode Strikethrough(this TextDecorationNode node)
    {
        return new StrikethroughNode(node);
    }
}