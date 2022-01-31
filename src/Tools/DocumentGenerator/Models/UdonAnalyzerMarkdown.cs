// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.CSharp.Workspace;

using static NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Linq.MarkdownFactory;


namespace NatsunekoLaboratory.UdonAnalyzer.DocumentGenerator.Models;

internal static class UdonAnalyzerMarkdown
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
                TableBody("Runtime Version", metadata.RuntimeVersion)
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

    public static string CreateIndexDocument(List<AnalyzerMetadata> metadata)
    {
        var items = metadata.Select(w => TableBody(w.Id, w.Title, w.Severity.ToString()));

        return Document(
            Heading2("List of Runtime Analyzers in UdonAnalyzers"),
            Table(
                TableHeader("ID", "Title", "Severity"),
                items.ToArray()
            )
        ).ToString();
    }
}