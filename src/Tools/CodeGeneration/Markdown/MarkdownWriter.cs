// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using NatsunekoLaboratory.UdonAnalyzer.Extensions;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown;

public sealed class MarkdownWriter
{
    private readonly Stack<string> _prefix;
    private readonly List<int> _rows;
    private readonly StringBuilder _sb;
    private int _indentLevel;
    private bool _isOrnamentAppendedOnThisLine;

    public int Length => _sb.Length;

    public MarkdownWriter()
    {
        _prefix = new Stack<string>();
        _rows = new List<int>();
        _sb = new StringBuilder();
    }

    public void PushPrefix(string prefix)
    {
        _prefix.Push(prefix);
    }

    public void PopPrefix()
    {
        _prefix.Pop();
    }

    public string? PeekPrefix()
    {
        return _prefix.Count > 0 ? _prefix.Peek() : null;
    }

    public void IncrementIndent()
    {
        _indentLevel++;
    }

    public void IncreaseIndent(int count)
    {
        _indentLevel += count;
    }

    public void DecrementIndent()
    {
        _indentLevel--;
    }

    public void DecreaseIndent(int count)
    {
        _indentLevel -= count;
    }

    public void ResetIndent()
    {
        _indentLevel = 0;
    }

    public void WriteIndent()
    {
        for (var i = 0; i < _indentLevel; i++)
            _sb.Append(' ');
    }

    public void WritePrefix()
    {
        var prefix = PeekPrefix();
        if (string.IsNullOrWhiteSpace(prefix))
            return;

        _sb.Append(prefix);
    }

    public void WriteOrnament()
    {
        if (_isOrnamentAppendedOnThisLine)
            return;

        WriteIndent();
        WritePrefix();
        _isOrnamentAppendedOnThisLine = true;
    }

    public void WriteInline(char c)
    {
        WriteOrnament();
        _sb.Append(c);
    }

    public void WriteInline(string str)
    {
        WriteOrnament();
        _sb.Append(str);
    }

    public void WriteNewLine()
    {
        WriteOrnament();
        _sb.AppendLine();
        _isOrnamentAppendedOnThisLine = false;
    }

    public void WriteNewLineToEol()
    {
        _sb.AppendLine("  ");
        _isOrnamentAppendedOnThisLine = false;
    }

    public void SetTableRowFills(params int[] rows)
    {
        _rows.Clear();
        _rows.AddRange(rows);
    }

    public void FillTo(int length, int row, char c = ' ')
    {
        if (_rows.Count <= row)
            throw new ArgumentOutOfRangeException(nameof(row));

        var fill = _rows[row];
        (fill - length).Times(() => WriteInline(c));
    }

    public override string ToString()
    {
        return _sb.ToString();
    }
}