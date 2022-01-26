// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;

public interface ISyntaxNode
{
    string Kind { get; }

    void WriteTo(MarkdownWriter writer);

    string GetDebuggerDisplay();
}