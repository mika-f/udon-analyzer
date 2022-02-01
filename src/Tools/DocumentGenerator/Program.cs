// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using NatsunekoLaboratory.UdonAnalyzer.CodeGeneration;
using NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.CSharp.Workspace;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Helpers;
using NatsunekoLaboratory.UdonAnalyzer.DocumentGenerator.Models;

static async Task WriteTemplateAsync(string path, string id, string category, string content)
{
    await using var sw = new StreamWriter(Path.Combine(path, "analyzers", category.ToLower(), $"{id}.md"));
    await sw.WriteLineAsync(content);
}

static async Task<int> RunDefaultAsync(CommandLineParameters args)
{
    var metadata = new UdonAnalyzerMetadata(args.Path);
    var isAnalyzeSuccess = await metadata.TryAnalyzingAllAsync();
    if (isAnalyzeSuccess)
    {
        var path = Path.GetFullPath(Path.Combine(args.Path, "docs"));
        var runtimeAnalyzers = new List<AnalyzerMetadata>();
        var compilerAnalyzers = new List<AnalyzerMetadata>();

        foreach (var data in metadata.Metadata)
        {
            var id = data.Id;
            var category = id.StartsWith("VRC") ? "Udon" : "UdonSharp";
            var content = category switch
            {
                "Udon" => UdonAnalyzerMarkdown.CreateAnalyzerDocument(data),
                "UdonSharp" => UdonSharpAnalyzerMarkdown.CreateAnalyzerDocument(data),
                _ => throw new ArgumentOutOfRangeException()
            };

            if (category == "Udon")
                runtimeAnalyzers.Add(data);
            else
                compilerAnalyzers.Add(data);

            await WriteTemplateAsync(path, id, category, content);

            Console.Write($"Writing UdonAnalyzer documentation for {category}:{id} to {category.ToLower()}/{id}.md");
        }

        await WriteTemplateAsync(path, "README", "Udon", UdonAnalyzerMarkdown.CreateIndexDocument(runtimeAnalyzers));
        await WriteTemplateAsync(path, "README", "UdonSharp", UdonSharpAnalyzerMarkdown.CreateIndexDocument(compilerAnalyzers));

        return ExitCodes.Success;
    }

    return ExitCodes.Failure;
}

return await ConsoleHost.Create<CommandLineParameters>(RunDefaultAsync)
                        .RunAsync(args);