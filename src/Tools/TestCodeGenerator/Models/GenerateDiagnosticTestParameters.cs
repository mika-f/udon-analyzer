// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using NatsunekoLaboratory.UdonAnalyzer.CodeGeneration;
using NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.CSharp;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Helpers;

namespace NatsunekoLaboratory.UdonAnalyzer.TestCodeGenerator.Models;

public class GenerateDiagnosticTestParameters
{
    [Option("src", IsRequired = false)]
    public string Source { get; set; } = GetDefaultPath();

    [Option("id")]
    public string Id { get; set; }

    [Option("with-code-fix")]
    public bool WithCodeFix { get; set; }

    public async Task<int> GenerateTestCodeAsync()
    {
        var analyzer = new UdonAnalyzerMetadata(Source);
        if (!await analyzer.TryAnalyzingImplementationsOnlyAsync())
        {
            await Console.Error.WriteLineAsync("failed to analyze source code");
            return ExitCodes.Failure;
        }

        var metadata = analyzer.Metadata.FirstOrDefault(w => w.Id == Id);
        if (metadata == null)
        {
            await Console.Error.WriteLineAsync($"could not find analyzer metadata for {Id}");
            return ExitCodes.Failure;
        }

        var category = Id.StartsWith("VRC") ? "Udon" : "UdonSharp";
        var compilation = UdonSharpAnalyzerTestGenerator.CreateGeneratedTestCode(metadata.ClassName!, category);
        await CodeGenerationHelper.WriteCompilationUnit(Path.Combine(Source, "Tests", "Analyzers.Tests", category, $"{metadata.ClassName!}Test.cs"), compilation);

        if (WithCodeFix)
        {
            //
        }

        return ExitCodes.Success;
    }


    private static string GetDefaultPath()
    {
#if DEBUG
        return "../../../../../../src";
#else
        return Environment.CurrentDirectory;

#endif
    }
}