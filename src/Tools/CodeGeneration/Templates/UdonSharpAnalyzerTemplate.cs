// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Immutable;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Templates;

internal class UdonSharpAnalyzerTemplate : ITemplate
{
    private readonly string _root;

    public UdonSharpAnalyzerTemplate(string root)
    {
        _root = root;
    }

    public string Key => "Analyzers/Udon";
    public string Path => System.IO.Path.Combine(_root, "analyzers/udon/TEMPLATE.md");
    public ImmutableArray<string> Variables => ImmutableArray.Create("ID", "TITLE", "CATEGORY", "SEVERITY", "RUNTIME_VERSION", "COMPILER_VERSION", "DESCRIPTION", "CODE_WITH_DIAGNOSTIC", "CODE_WITH_FIX");
}