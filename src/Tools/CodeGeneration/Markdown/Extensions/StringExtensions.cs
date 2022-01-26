using NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Extensions
{
    public static class StringExtensions
    {
        public static BoldNode Bold(this string str)
        {
            return new BoldNode(str);
        }

        public static ItalicNode Italic(this string str)
        {
            return new ItalicNode(str);
        }

        public static StrikethroughNode Strikethrough(this string str)
        {
            return new StrikethroughNode(str);
        }

        public static ParagraphNode Paragraph(this string str)
        {
            return new ParagraphNode(str);
        }

        public static CodeNode Code(this string str)
        {
            return new CodeNode(str);
        }
    }
}
