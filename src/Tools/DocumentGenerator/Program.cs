// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;

using NatsunekoLaboratory.UdonAnalyzer.CodeGeneration;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Helpers;
using NatsunekoLaboratory.UdonAnalyzer.DocumentGenerator;

static async Task<string> ReadTemplateAsync(string path)
{
    using var sr = new StreamReader(path);
    return await sr.ReadToEndAsync();
}

static async Task WriteTemplateAsync(string path, string id, string category, string content)
{
    await using var sw = new StreamWriter(Path.Combine(path, category.ToLower(), $"{id}.md"));
    await sw.WriteLineAsync(content);
}

static async Task<int> RunDefaultAsync(CommandLineParameters args)
{
    var metadata = new UdonAnalyzerMetadata(args.Path);
    var isAnalyzeSuccess = await metadata.TryAnalyzingAsync();
    if (isAnalyzeSuccess)
    {
        var path = Path.Combine(args.Path, "docs", "analyzers");
        var templateForUdon = await ReadTemplateAsync(Path.GetFullPath(Path.Combine(path, "udon", "TEMPLATE.md")));
        var templateForUdonSharp = await ReadTemplateAsync(Path.GetFullPath(Path.Combine(path, "udonsharp", "TEMPLATE.md")));

        foreach (var data in metadata.Metadata)
        {
            var id = data.Id;
            var category = id.StartsWith("VRC") ? "Udon" : "UdonSharp";

            var content = (id.StartsWith("VRC") ? templateForUdon : templateForUdonSharp)
                          .Replace("%ID%", id)
                          .Replace("%TITLE%", data.Title)
                          .Replace("%DESCRIPTION%", data.Description)
                          .Replace("%CATEGORY%", data.Category)
                          .Replace("%SEVERITY%", data.Severity.ToString())
                          .Replace("%RUNTIME_VERSION%", data.RuntimeVersion)
                          .Replace("%COMPILER_VERSION%", data.CompilerVersion)
                          .Replace("%CODE_WITH_DIAGNOSTIC%", data.CodeWithDiagnostic)
                          .Replace("%CODE_WITH_FIX%", data.CodeWithFix);

            await WriteTemplateAsync(path, id, category, content);

            Console.Write($"Writing UdonAnalyzer documentation for {category}:{id} to {category.ToLower()}/{id}.md");
        }

        return ExitCodes.Success;
    }

    return ExitCodes.Failure;
}

return await ConsoleHost.Create<CommandLineParameters>(args, RunDefaultAsync)
                        .RunAsync();