// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;

using NatsunekoLaboratory.UdonAnalyzer.CodeGeneration;
using NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Templates;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Helpers;
using NatsunekoLaboratory.UdonAnalyzer.DocumentGenerator;

static async Task WriteTemplateAsync(string path, string id, string category, string content)
{
    await using var sw = new StreamWriter(Path.Combine(path, "analyzers", category.ToLower(), $"{id}.md"));
    await sw.WriteLineAsync(content);
}

static async Task<int> RunDefaultAsync(CommandLineParameters args)
{
    var metadata = new UdonAnalyzerMetadata(args.Path);
    var isAnalyzeSuccess = await metadata.TryAnalyzingAsync();
    if (isAnalyzeSuccess)
    {
        var path = Path.GetFullPath(Path.Combine(args.Path, "docs"));
        foreach (var data in metadata.Metadata)
        {
            var id = data.Id;
            var category = id.StartsWith("VRC") ? "Udon" : "UdonSharp";
            var accessor = new MetadataVariableAccessor(data);
            var content = category switch
            {
                "Udon" => (await TemplateGenerator.CreateForUdonAnalyzerAsync(path)).Generate(accessor),
                "UdonSharp" => (await TemplateGenerator.CreateForUdonSharpAnalyzerAsync(path)).Generate(accessor),
                _ => throw new ArgumentOutOfRangeException(category)
            };

            await WriteTemplateAsync(path, id, category, content);

            Console.Write($"Writing UdonAnalyzer documentation for {category}:{id} to {category.ToLower()}/{id}.md");
        }

        return ExitCodes.Success;
    }

    return ExitCodes.Failure;
}

return await ConsoleHost.Create<CommandLineParameters>(args, RunDefaultAsync)
                        .RunAsync();