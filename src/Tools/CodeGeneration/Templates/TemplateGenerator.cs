// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Templates;

public class TemplateGenerator
{
    private static readonly Dictionary<string, string> Templates;
    private static readonly Dictionary<string, ImmutableArray<string>> Variables;
    private static readonly Dictionary<string, string> Formats;
    private static readonly Dictionary<string, string> OutPaths;

    private readonly string _template;
    private readonly ImmutableArray<string> _variables;
    private readonly string _format;
    private readonly string _outPath;

    static TemplateGenerator()
    {
        Templates = new Dictionary<string, string>();
        Variables = new Dictionary<string, ImmutableArray<string>>();
        Formats = new Dictionary<string, string>();
        OutPaths = new Dictionary<string, string>();
    }

    private TemplateGenerator(string template, ImmutableArray<string> variables, string format, string outPath)
    {
        _template = template;
        _variables = variables;
        _format = string.IsNullOrWhiteSpace(format) ? "%{0}%" : format;
        _outPath = outPath;
    }

    public static TemplateGenerator CreateFromTemplate(ITemplate template)
    {
        if (Templates.ContainsKey(template.Key))
            return new TemplateGenerator(Templates[template.Key], Variables[template.Key], Formats[template.Key], OutPaths[template.Key]);

        Templates.Add(template.Key, template.Content);
        Variables.Add(template.Key, template.Variables);
        Formats.Add(template.Key, template.VariableFormat);
        OutPaths.Add(template.Key, template.OutPath);

        return new TemplateGenerator(Templates[template.Key], Variables[template.Key], Formats[template.Key], OutPaths[template.Key]);
    }

    public string Generate(IVariableAccessor variables)
    {
        var content = _template;

        foreach (var variable in _variables)
            content = content.Replace(string.Format(_format, variable), variables.GetVariableFor(variable));

        return content;
    }

    public void GenerateAndFlush(IVariableAccessor variable)
    {
        var content = Generate(variable);
        using var sw = new StreamWriter(_outPath);
        sw.Write(content);
    }
}