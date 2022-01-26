// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.IO;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;

public sealed class StringNode : SyntaxNode
{
    private readonly bool _escape;
    private readonly string _str;

    public override string Kind => "String";

    public StringNode(string str, bool escape = false)
    {
        _str = str;
        _escape = escape;
    }


    private static string EscapeChar(char ch)
    {
        return ch switch
        {
            '<' => "&lt;",
            '>' => "&gt;",
            '\\' => @"\\",
            '`' => @"\`",
            '*' => @"\*",
            '_' => @"\_",
            '{' => @"\{",
            '}' => @"\}",
            '[' => @"\[",
            ']' => @"\]",
            '(' => @"\(",
            ')' => @"\)",
            '#' => @"\#",
            '+' => @"\+",
            '-' => @"\-",
            '.' => @"\.",
            '!' => @"\!",
            '|' => @"\|",
            '~' => @"\~",
            '"' => @"\""",
            _ => ch.ToString()
        };
    }

    public override void WriteTo(MarkdownWriter writer)
    {
        if (_escape)
        {
            using var sr = new StringReader(_str);

            while (sr.Peek() > -1)
                writer.WriteInline(EscapeChar((char)sr.Read()));
            return;
        }

        writer.WriteInline(_str);
    }

    public override string GetDebuggerDisplay()
    {
        return _str;
    }
}