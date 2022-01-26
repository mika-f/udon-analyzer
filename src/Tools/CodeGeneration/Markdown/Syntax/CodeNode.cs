// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.IO;

using NatsunekoLaboratory.UdonAnalyzer.Extensions;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;

public sealed class CodeNode : InlineNode
{
    private readonly string _code;

    public override string Kind => "Code";

    public CodeNode(string code)
    {
        _code = code;
    }

    private int GetBacktickLength()
    {
        var count = 0;
        var len = -1;

        using var sr = new StringReader(_code);
        while (sr.Peek() > -1)
        {
            var c = (char)sr.Read();
            if (c == '`')
            {
                len = 1;

                while (sr.Read() == '`')
                    len++;
            }

            if (len > 0 && len > count)
                count = len;
        }

        return count + 1;
    }

    public override void WriteTo(MarkdownWriter writer)
    {
        var count = GetBacktickLength();
        count.Times(() => writer.WriteInline('`'));
        writer.WriteInline(_code);
        count.Times(() => writer.WriteInline('`'));
    }

    public override string GetDebuggerDisplay()
    {
        return $"`{_code}`";
    }
}