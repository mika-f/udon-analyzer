// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Templates;

public class TemplateGenerator
{
    private static readonly Dictionary<string, string> Templates;
    private static readonly Dictionary<string, ImmutableArray<string>> Variables;

    private readonly string _template;
    private readonly ImmutableArray<string> _variables;

    static TemplateGenerator()
    {
        Templates = new Dictionary<string, string>();
        Variables = new Dictionary<string, ImmutableArray<string>>();
    }

    private TemplateGenerator(string template, ImmutableArray<string> variables)
    {
        _template = template;
        _variables = variables;
    }

    public static TemplateGenerator CreateFromTemplate(ITemplate template)
    {
        if (Templates.ContainsKey(template.Key))
            return new TemplateGenerator(Templates[template.Key], Variables[template.Key]);

        Templates.Add(template.Key, template.Content);
        Variables.Add(template.Key, template.Variables);

        return new TemplateGenerator(Templates[template.Key], Variables[template.Key]);
    }

    public string Generate(IVariableAccessor variables)
    {
        var content = _template;

        foreach (var variable in _variables)
            content = content.Replace($"%{variable}%", variables.GetVariableFor(variable));

        return content;
    }
}