// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Linq;

public static class MarkdownFactory
{
    public static MarkdownDocument Document(params object[] nodes)
    {
        return new MarkdownDocument(nodes);
    }

    public static BlockquoteNode Blockquote(params object[] nodes)
    {
        return new BlockquoteNode(nodes);
    }

    public static BoldNode Bold(string text)
    {
        return new BoldNode(text);
    }

    public static BoldNode Bold(TextDecorationNode text)
    {
        return new BoldNode(text);
    }

    public static CodeBlockNode CodeBlock(string code, string? lang = null)
    {
        return new CodeBlockNode(code, lang);
    }

    public static CodeNode Code(string code)
    {
        return new CodeNode(code);
    }

    public static ContainerNode Container(params object[] contents)
    {
        return new ContainerNode(contents);
    }

    public static HeadingNode Heading1(CodeNode code)
    {
        return new HeadingNode(code, 1);
    }

    public static HeadingNode Heading1(ImageNode image)
    {
        return new HeadingNode(image, 1);
    }

    public static HeadingNode Heading1(HyperlinkNode hyperlink)
    {
        return new HeadingNode(hyperlink, 1);
    }

    public static HeadingNode Heading1(ItalicNode italic)
    {
        return new HeadingNode(italic, 1);
    }

    public static HeadingNode Heading1(string text)
    {
        return new HeadingNode(text, 1);
    }

    public static HeadingNode Heading2(CodeNode code)
    {
        return new HeadingNode(code, 2);
    }

    public static HeadingNode Heading2(ImageNode image)
    {
        return new HeadingNode(image, 2);
    }

    public static HeadingNode Heading2(HyperlinkNode hyperlink)
    {
        return new HeadingNode(hyperlink, 2);
    }

    public static HeadingNode Heading2(ItalicNode italic)
    {
        return new HeadingNode(italic, 2);
    }

    public static HeadingNode Heading3(CodeNode code)
    {
        return new HeadingNode(code, 3);
    }

    public static HeadingNode Heading3(ImageNode image)
    {
        return new HeadingNode(image, 3);
    }

    public static HeadingNode Heading3(HyperlinkNode hyperlink)
    {
        return new HeadingNode(hyperlink, 3);
    }

    public static HeadingNode Heading3(ItalicNode italic)
    {
        return new HeadingNode(italic, 3);
    }

    public static HeadingNode Heading3(string text)
    {
        return new HeadingNode(text, 3);
    }

    public static HeadingNode Heading2(string text)
    {
        return new HeadingNode(text, 2);
    }

    public static HeadingNode Heading4(CodeNode code)
    {
        return new HeadingNode(code, 4);
    }

    public static HeadingNode Heading4(ImageNode image)
    {
        return new HeadingNode(image, 4);
    }

    public static HeadingNode Heading4(HyperlinkNode hyperlink)
    {
        return new HeadingNode(hyperlink, 4);
    }

    public static HeadingNode Heading4(ItalicNode italic)
    {
        return new HeadingNode(italic, 4);
    }

    public static HeadingNode Heading4(string text)
    {
        return new HeadingNode(text, 4);
    }

    public static HeadingNode Heading5(CodeNode code)
    {
        return new HeadingNode(code, 5);
    }

    public static HeadingNode Heading5(ImageNode image)
    {
        return new HeadingNode(image, 5);
    }

    public static HeadingNode Heading5(HyperlinkNode hyperlink)
    {
        return new HeadingNode(hyperlink, 5);
    }

    public static HeadingNode Heading5(ItalicNode italic)
    {
        return new HeadingNode(italic, 5);
    }

    public static HeadingNode Heading5(string text)
    {
        return new HeadingNode(text, 5);
    }

    public static HeadingNode Heading6(CodeNode code)
    {
        return new HeadingNode(code, 6);
    }

    public static HeadingNode Heading6(ImageNode image)
    {
        return new HeadingNode(image, 6);
    }

    public static HeadingNode Heading6(HyperlinkNode hyperlink)
    {
        return new HeadingNode(hyperlink, 6);
    }

    public static HeadingNode Heading6(ItalicNode italic)
    {
        return new HeadingNode(italic, 6);
    }

    public static HeadingNode Heading6(string text)
    {
        return new HeadingNode(text, 6);
    }

    public static HorizontalNode Horizontal()
    {
        return new HorizontalNode();
    }

    public static HyperlinkNode Hyperlink(string url, CodeNode text)
    {
        return new HyperlinkNode(url, text);
    }

    public static HyperlinkNode Hyperlink(string url, ImageNode image)
    {
        return new HyperlinkNode(url, image);
    }

    public static HyperlinkNode Hyperlink(string url, TextDecorationNode text)
    {
        return new HyperlinkNode(url, text);
    }

    public static HyperlinkNode Hyperlink(string url, string text)
    {
        return new HyperlinkNode(url, text);
    }

    public static ImageNode Image(string url)
    {
        return new ImageNode(url);
    }

    public static ItalicNode Italic(TextDecorationNode text)
    {
        return new ItalicNode(text);
    }

    public static ItalicNode Italic(string text)
    {
        return new ItalicNode(text);
    }

    public static ListItemNode ListItem(ListNode list)
    {
        return new ListItemNode(list);
    }

    public static ListItemNode ListItem(TextDecorationNode text)
    {
        return new ListItemNode(text);
    }

    public static ListItemNode ListItem(string text)
    {
        return new ListItemNode(text);
    }

    public static ListNode List(params ListItemNode[] items)
    {
        return new ListNode(items);
    }

    public static ParagraphNode Paragraph(string text)
    {
        return new ParagraphNode(text);
    }

    public static ParagraphNode Paragraph(InlineNode text)
    {
        return new ParagraphNode(text);
    }

    public static NewLineNode NewLine()
    {
        return new NewLineNode();
    }

    public static StrikethroughNode Strikethrough(TextDecorationNode text)
    {
        return new StrikethroughNode(text);
    }

    public static StrikethroughNode Strikethrough(string text)
    {
        return new StrikethroughNode(text);
    }

    public static StringNode String(string str)
    {
        return new StringNode(str);
    }

    public static StringNode EscapedString(string str)
    {
        return new StringNode(str, true);
    }

    public static TableBodyNode TableBody(params object[] items)
    {
        return new TableBodyNode(items);
    }

    public static TableHeadNode TableHeader(params object[] items)
    {
        return new TableHeadNode(items);
    }

    public static TableNode Table(TableHeadNode header, params TableBodyNode[] items)
    {
        return new TableNode(header, items);
    }
}