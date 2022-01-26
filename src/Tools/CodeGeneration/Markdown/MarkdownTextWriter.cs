// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;
using NatsunekoLaboratory.UdonAnalyzer.Models;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown;

public class MarkdownTextWriter
{
    private readonly SafeStack<IContainer> _containerStack;
    private readonly ContainerNode _rootContainer;
    private readonly List<TableBodyNode> _tableColumns;
    private TableNode? _table;
    private TableHeadNode? _tableHeader;
    private TableBodyNode? _tableBodies;
    private IContainer? _container;

    private IContainer CurrentContainer => _container ?? _rootContainer;

    public MarkdownTextWriter()
    {
        _rootContainer = new ContainerNode();
        _tableColumns = new List<TableBodyNode>();
        _containerStack = new SafeStack<IContainer>();
        _containerStack.Push(_rootContainer);
    }

    public void StartBlockquote()
    {
        StartContainer(new BlockquoteNode());
    }

    public void EndBlockquote()
    {
        EndContainer();
    }

    public void StartList()
    {
        StartContainer(new ListNode());
    }

    public void EndList()
    {
        EndContainer();
    }

    public void WriteInline(InlineNode node)
    {
        if (CurrentContainer is ListNode)
        {
            if (node is TextDecorationNode text)
                CurrentContainer.Add(new ListItemNode(text));
        }
        else
        {
            CurrentContainer.Add(node);
        }
    }

    public void WriteInline(string str)
    {
        if (CurrentContainer is ListNode)
            CurrentContainer.Add(new ListItemNode(str));
        else
            CurrentContainer.Add(new StringNode(str, true));
    }

    public void WriteNewLine()
    {
        CurrentContainer.Add(new NewLineNode());
    }

    public void WriteHeading1(string title)
    {
        CurrentContainer.Add(new HeadingNode(title, 1));
    }

    public void WriteHeading2(string title)
    {
        CurrentContainer.Add(new HeadingNode(title, 2));
    }

    public void WriteHeading3(string title)
    {
        CurrentContainer.Add(new HeadingNode(title, 3));
    }

    public void WriteHeading4(string title)
    {
        CurrentContainer.Add(new HeadingNode(title, 4));
    }

    public void WriteHeading5(string title)
    {
        CurrentContainer.Add(new HeadingNode(title, 5));
    }

    public void WriteHeading6(string title)
    {
        CurrentContainer.Add(new HeadingNode(title, 6));
    }

    public void WriteCodeBlock(string code, string? lang = null)
    {
        CurrentContainer.Add(new CodeBlockNode(code, lang));
    }

    public void WriteHorizontal()
    {
        CurrentContainer.Add(new HorizontalNode());
    }

    public void WriteHyperlink(string url, string text)
    {
        CurrentContainer.Add(new HyperlinkNode(url, text));
    }

    public void WriteHyperlink(string url, TextDecorationNode text)
    {
        CurrentContainer.Add(new HyperlinkNode(url, text));
    }

    public void WriteImage(string url)
    {
        CurrentContainer.Add(new ImageNode(url));
    }

    public void StartTable()
    {
        // MARKER
    }

    public void EndTable()
    {
        if (_tableHeader == null)
            throw new InvalidOperationException();

        _table = new TableNode(_tableHeader, _tableColumns.ToArray());
        CurrentContainer.Add(_table);
    }

    public void StartTableHeader()
    {
        _tableHeader = new TableHeadNode();
        StartContainer(_tableHeader);
    }

    public void EndTableHeader()
    {
        if (_tableHeader == null)
            throw new InvalidOperationException();

        EndContainer(false);
    }

    public void StartTableBody()
    {
        _tableBodies = new TableBodyNode();
        StartContainer(_tableBodies);
    }

    public void EndTableBody()
    {
        if (_tableBodies == null)
            throw new InvalidOperationException();

        EndContainer(false);

        _tableColumns.Add(_tableBodies);
    }

    private void StartContainer(IContainer container)
    {
        if (_container != null)
            _containerStack.Push(_container);
        _container = container;
    }

    private void EndContainer(bool add = true)
    {
        if (_container == null)
            throw new InvalidOperationException();

        var container = _containerStack.CanPop() ? _containerStack.Pop() : _rootContainer;
        if (add)
            container.Add(_container);
    }

    public override string ToString()
    {
        var mw = new MarkdownWriter();
        _rootContainer.WriteTo(mw);

        return mw.ToString();
    }
}