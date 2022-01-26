// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Diagnostics;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;

[DebuggerDisplay("{Kind}: {GetDebuggerDisplay(),nq}")]
public abstract class SyntaxNode : ISyntaxNode
{
    public abstract string Kind { get; }

    public abstract void WriteTo(MarkdownWriter writer);

    public abstract string GetDebuggerDisplay();
}