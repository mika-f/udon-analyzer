// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Linq;

public class MarkdownDocument
{
    private readonly ContainerNode _container;

    public MarkdownDocument(params object[] nodes)
    {
        _container = new ContainerNode(nodes);
    }

    public override string ToString()
    {
        var mw = new MarkdownWriter();
        _container.WriteTo(mw);

        return mw.ToString();
    }
}