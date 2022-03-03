// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Text.Encodings.Web;
using System.Text.Json;

using NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.CSharp;
using NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Templates;
using NatsunekoLaboratory.UdonAnalyzer.CodeGenerator.Extensions;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Helpers;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Interfaces;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Models;
using NatsunekoLaboratory.UdonAnalyzer.Models;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGenerator.Models;

public class GenerateCompilerAnalyzerParameters : GenerateRuntimeAnalyzerParameters
{
    [Option("compiler-min-version", IsRequired = false)]
    public string? CompilerMinVersion { get; set; }

    [Option("compiler-max-version", IsRequired = false)]
    public string? CompilerMaxVersion { get; set; }

    public override bool Validate(out List<IErrorMessage> errors)
    {
        if (!base.Validate(out errors))
            return false;

        if (!string.IsNullOrWhiteSpace(CompilerMinVersion) && GenericVersion.TryParse(CompilerMinVersion, out _))
        {
            errors.Add(new ErrorMessage("CompilerMinVersion could not cast to GenericVersion, invalid format"));
            return false;
        }

        if (!string.IsNullOrWhiteSpace(CompilerMaxVersion) && GenericVersion.TryParse(CompilerMaxVersion, out _))
        {
            errors.Add(new ErrorMessage("CompilerMaxVersion could not cast to GenericVersion, invalid format"));
            return false;
        }

        return true;
    }

    public async Task<int> GenerateCompilerAnalyzerCode()
    {
        if (!string.IsNullOrWhiteSpace(JsonPath))
        {
            using var sr = new StreamReader(JsonPath);
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var json = JsonSerializer.Deserialize<GenerateCompilerAnalyzerParameters>(await sr.ReadToEndAsync(), options);
            if (json == null)
            {
                Console.WriteLine("failed to parse JSON string");
                return ExitCodes.Failure;
            }

            if (!json.Validate(out var errors))
            {
                foreach (var error in errors)
                    Console.WriteLine(error.ToMessageString());
                return ExitCodes.Failure;
            }

            json.CopyTo(this);
        }

        var compilation = UdonSharpAnalyzerGenerator.CreateGeneratedAnalyzerCode(Name, RuntimeMinVersion, RuntimeMaxVersion, CompilerMinVersion, CompilerMaxVersion);
        var path = Path.Combine(Source, "Analyzers", "UdonSharp", $"{Name}.cs");
        await CodeGenerationHelper.WriteCompilationUnit(path, compilation);
        Console.WriteLine($"Writing analyzer C# source code to {path}");

        var descriptor = UdonSharpAnalyzerGenerator.CreateGeneratedDescriptorCode($"VSC{Id.ToString().PadLeft(4, '0')}", Name, Title, MessageFormat ?? Description, Category, Severity, Description);
        var template = TemplateGenerator.CreateFromTemplate(new DiagnosticDescriptorsTemplate(Source));
        template.GenerateAndFlush(new DiagnosticDescriptorsTemplate.VariableAccessor(DiagnosticDescriptorsTemplate.VariableAccessor.UdonSharpCompilerDescriptorKey, descriptor));

        return ExitCodes.Success;
    }
}