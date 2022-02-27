// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis;

using NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.CSharp;
using NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Templates;
using NatsunekoLaboratory.UdonAnalyzer.CodeGenerator.Extensions;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Helpers;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Interfaces;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Models;
using NatsunekoLaboratory.UdonAnalyzer.Models;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGenerator.Models;

public class GenerateRuntimeAnalyzerParameters : IValidatableEntity
{
    private readonly Regex _nameRegex = new("^[A-Z][A-Za-z0-9]+Analyzer$", RegexOptions.Compiled);

    [Option("src", IsRequired = false)]
    public string Source { get; set; } = GetDefaultPath();

    [Option("id", IsRequired = false)]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [Option("name", IsRequired = false)]
    public string Name { get; set; }

    [Option("title", IsRequired = false)]
    public string Title { get; set; }

    [Option("category", IsRequired = false)]
    public string Category { get; set; }

    [Option("severity", IsRequired = false)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DiagnosticSeverity Severity { get; set; }

    [Option("runtime-min-version", IsRequired = false)]
    public string? RuntimeMinVersion { get; set; }

    [Option("runtime-max-version", IsRequired = false)]
    public string? RuntimeMaxVersion { get; set; }

    [Option("description", IsRequired = false)]
    public string Description { get; set; }

    [Option("json", IsRequired = false)]
    [JsonIgnore]
    public string? JsonPath { get; set; }

    public virtual bool Validate(out List<IErrorMessage> errors)
    {
        errors = new List<IErrorMessage>();

        if (string.IsNullOrWhiteSpace(JsonPath))
        {
            if (Id == 0)
                errors.Add(new ErrorMessage("Id must be required"));

            if (string.IsNullOrWhiteSpace(Name))
                errors.Add(new ErrorMessage("Name must be required"));

            if (!_nameRegex.IsMatch(Name ?? ""))
                errors.Add(new ErrorMessage("Name must be valid classname"));

            if (string.IsNullOrWhiteSpace(Title))
                errors.Add(new ErrorMessage("Title must be required"));

            if (string.IsNullOrWhiteSpace(Category))
                errors.Add(new ErrorMessage("Category must be required"));

            if (string.IsNullOrWhiteSpace(Description))
                errors.Add(new ErrorMessage("Description must be required"));
        }
        else
        {
            if (!File.Exists(JsonPath))
                errors.Add(new ErrorMessage("Json is not found on filesystem"));
        }

        if (!string.IsNullOrWhiteSpace(RuntimeMinVersion) && GenericVersion.TryParse(RuntimeMinVersion, out _))
            errors.Add(new ErrorMessage("RuntimeMinVersion could not cast to GenericVersion, invalid format"));

        if (!string.IsNullOrWhiteSpace(RuntimeMaxVersion) && GenericVersion.TryParse(RuntimeMaxVersion, out _))
            errors.Add(new ErrorMessage("RuntimeMaxVersion could not cast to GenericVersion, invalid format"));

        return errors.Count == 0;
    }

    public async Task<int> GenerateRuntimeAnalyzerCode()
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
            var json = JsonSerializer.Deserialize<GenerateRuntimeAnalyzerParameters>(await sr.ReadToEndAsync(), options);
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

        var compilation = UdonRuntimeAnalyzerGenerator.CreateGeneratedAnalyzerCode(Name, RuntimeMinVersion, RuntimeMaxVersion);
        var path = Path.Combine(Source, "Analyzers", "Udon", $"{Name}.cs");
        await CodeGenerationHelper.WriteCompilationUnit(path, compilation);
        Console.WriteLine($"Writing analyzer C# source code to {path}");

        var descriptor = UdonRuntimeAnalyzerGenerator.CreateGeneratedDescriptorCode($"VRC{Id.ToString().PadLeft(4, '0')}", Name, Title, Description, Category, Severity);
        var template = TemplateGenerator.CreateFromTemplate(new DiagnosticDescriptorsTemplate(Source));
        template.GenerateAndFlush(new DiagnosticDescriptorsTemplate.VariableAccessor(DiagnosticDescriptorsTemplate.VariableAccessor.UdonRuntimeDescriptorKey, descriptor));

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