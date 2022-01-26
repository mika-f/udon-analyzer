// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;

public sealed class HorizontalNode : BlockNode
{
    public override string Kind => "HorizontalLine";

    public override void WriteTo(MarkdownWriter writer)
    {
        writer.WriteNewLine();
        writer.WriteInline("----\n");
        writer.WriteNewLine();
    }

    public override string GetDebuggerDisplay()
    {
        return "<hr />";
    }
}