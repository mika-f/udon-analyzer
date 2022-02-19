// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis;

using NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.CSharp;
using NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Templates;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Helpers;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Interfaces;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Models;
using NatsunekoLaboratory.UdonAnalyzer.Models;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGenerator.Models;

public class GenerateRuntimeAnalyzerParameters : IValidatableEntity
{
    private readonly Regex _nameRegex = new("^[A-Z][A-Za-z]+Analyzer$", RegexOptions.Compiled);

    [Option("src", IsRequired = false)]
    public string Source { get; set; } = GetDefaultPath();

    [Option("id", IsRequired = true)]
    public int Id { get; set; }

    [Option("name", IsRequired = true)]
    public string Name { get; set; }

    [Option("title", IsRequired = true)]
    public string Title { get; set; }

    [Option("category", IsRequired = true)]
    public string Category { get; set; }

    [Option("severity", IsRequired = true)]
    public DiagnosticSeverity Severity { get; set; }

    [Option("runtime-min-version", IsRequired = false)]
    public string? RuntimeMinVersion { get; set; }

    [Option("runtime-max-version", IsRequired = false)]
    public string? RuntimeMaxVersion { get; set; }

    [Option("description", IsRequired = true)]
    public string Description { get; set; }

    public virtual bool Validate(out List<IErrorMessage> errors)
    {
        errors = new List<IErrorMessage>();

        if (!_nameRegex.IsMatch(Name))
        {
            errors.Add(new ErrorMessage("Name must be valid classname"));
            return false;
        }

        if (!string.IsNullOrWhiteSpace(RuntimeMinVersion) && GenericVersion.TryParse(RuntimeMinVersion, out _))
        {
            errors.Add(new ErrorMessage("RuntimeMinVersion could not cast to GenericVersion, invalid format"));
            return false;
        }

        if (!string.IsNullOrWhiteSpace(RuntimeMaxVersion) && GenericVersion.TryParse(RuntimeMaxVersion, out _))
        {
            errors.Add(new ErrorMessage("RuntimeMaxVersion could not cast to GenericVersion, invalid format"));
            return false;
        }

        return true;
    }

    public async Task<int> GenerateRuntimeAnalyzerCode()
    {
        var compilation = UdonRuntimeAnalyzerGenerator.CreateGeneratedAnalyzerCode(Name, RuntimeMinVersion, RuntimeMaxVersion);
        await CodeGenerationHelper.WriteCompilationUnit(Path.Combine(Source, "Analyzers", "Udon", $"{Name}.cs"), compilation);

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