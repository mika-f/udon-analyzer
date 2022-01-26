// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;

public sealed class NewLineNode : InlineNode
{
    public override string Kind => "NL";

    public override void WriteTo(MarkdownWriter writer)
    {
        writer.WriteNewLine();
    }

    public override string GetDebuggerDisplay()
    {
        return "<br />";
    }
}