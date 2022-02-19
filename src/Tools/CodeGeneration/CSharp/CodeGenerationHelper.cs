// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.CSharp;

public static class CodeGenerationHelper
{
    public static async Task WriteCompilationUnit(string path, CompilationUnitSyntax compilationUnit)
    {
        if (File.Exists(path))
            return;

        var unit = compilationUnit.NormalizeWhitespace();
        var header = new List<SyntaxTrivia>
        {
            SingleLineComment(" ------------------------------------------------------------------------------------------"),
            SingleLineComment("  Copyright (c) Natsuneko. All rights reserved."),
            SingleLineComment("  Licensed under the MIT License. See LICENSE in the project root for license information."),
            SingleLineComment(" ------------------------------------------------------------------------------------------")
        }.Aggregate(SyntaxFactory.TriviaList(), (current, next) => current.Add(next).Add(SyntaxFactory.CarriageReturnLineFeed));

        var finalized = unit.WithLeadingTrivia(header);
        var source = finalized.ToFullString();

        await File.WriteAllTextAsync(path, source);
    }

    private static SyntaxTrivia SingleLineComment(string comment)
    {
        return SyntaxFactory.Comment($"// {comment}");
    }
}