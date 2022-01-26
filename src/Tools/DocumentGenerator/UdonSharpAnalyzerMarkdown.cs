// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.CSharp;

using static NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Linq.MarkdownFactory;


namespace NatsunekoLaboratory.UdonAnalyzer.DocumentGenerator;

internal static class UdonSharpAnalyzerMarkdown
{
    public static string CreateAnalyzerDocument(AnalyzerMetadata metadata)
    {
        return Document(
            Heading2($"{metadata.Id}: {metadata.Title}"),
            Table(
                TableHeader("Property", "Value"),
                TableBody("ID", metadata.Id),
                TableBody("Category", metadata.Category),
                TableBody("Severity", metadata.Severity.ToString()),
                TableBody("Runtime Version", metadata.RuntimeVersion),
                TableBody("Compiler Version", metadata.CompilerVersion)
            ),
            Paragraph(metadata.Description),
            NewLine(),
            Heading3("Example"),
            Heading4("Code with Diagnostic"),
            CodeBlock(metadata.CodeWithDiagnostic!, "csharp"),
            Heading4("Code with Fix"),
            CodeBlock(metadata.CodeWithFix ?? "// NOT YET PROVIDED", "csharp")
        ).ToString();
    }
}