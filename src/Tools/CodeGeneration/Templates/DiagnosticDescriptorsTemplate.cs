// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Immutable;
using System.IO;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Templates;

public class DiagnosticDescriptorsTemplate : ITemplate
{
    public DiagnosticDescriptorsTemplate(string root)
    {
        var path = Path.Combine(root, "Analyzers", "Internal", "DiagnosticDescriptors.cs");
        using var sr = new StreamReader(path);
        Content = sr.ReadToEnd();
        OutPath = path;
    }

    public string Key => "NatsunekoLaboratory.UdonAnalyzer.Internal";
    public string Content { get; }
    public string VariableFormat => "// {0}";
    public ImmutableArray<string> Variables => ImmutableArray.Create("INSERT_VSC_DESCRIPTOR_HERE", "INSERT_VRC_DESCRIPTOR_HERE");
    public string OutPath { get; }

    public class VariableAccessor : IVariableAccessor
    {
        public const string UdonSharpCompilerDescriptorKey = "INSERT_VSC_DESCRIPTOR_HERE";
        public const string UdonRuntimeDescriptorKey = "INSERT_VRC_DESCRIPTOR_HERE";
        private readonly FieldDeclarationSyntax _declaration;

        private readonly string _key;

        public VariableAccessor(string key, FieldDeclarationSyntax declaration)
        {
            _key = key;
            _declaration = declaration;
        }

        public string? GetVariableFor(string key)
        {
            if (_key != key)
                return $"// {key}";

            var sb = new StringBuilder();
            sb.AppendLine(_declaration.NormalizeWhitespace().ToFullString());
            sb.AppendLine();
            sb.Append("    // ").AppendLine(key);

            return sb.ToString();
        }
    }
}