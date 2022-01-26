// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;

public sealed class CodeBlockNode : BlockNode
{
    private readonly string _code;
    private readonly string? _lang;

    public override string Kind => "CodeBlock";

    public CodeBlockNode(string code, string? lang = null)
    {
        _code = code;
        _lang = lang;
    }

    public override void WriteTo(MarkdownWriter writer)
    {
        writer.WriteNewLine();
        writer.WriteInline(string.IsNullOrWhiteSpace(_lang) ? "```" : $"```{_lang}");
        writer.WriteNewLine();

        var lines = _code.Split(Environment.NewLine);
        foreach (var line in lines)
        {
            writer.WriteInline(line);
            writer.WriteNewLine();
        }

        writer.WriteInline("```");
        writer.WriteNewLine();
        writer.WriteNewLine();
    }

    public override string GetDebuggerDisplay()
    {
        return $@"
```{_lang}
{_code}
```
".Trim();
    }
}