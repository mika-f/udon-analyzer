// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;

using NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.CSharp;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Templates;

public class MetadataVariableAccessor : IVariableAccessor
{
    private readonly AnalyzerMetadata _metadata;

    public MetadataVariableAccessor(AnalyzerMetadata metadata)
    {
        _metadata = metadata;
    }

    public string? GetVariableFor(string key)
    {
        return key.ToLower() switch
        {
            "id" => _metadata.Id,
            "title" => _metadata.Title,
            "description" => _metadata.Description,
            "category" => _metadata.Category,
            "severity" => _metadata.Severity.ToString(),
            "runtime_version" => _metadata.RuntimeVersion,
            "compiler_version" => _metadata.CompilerVersion,
            "code_with_diagnostic" => _metadata.CodeWithDiagnostic,
            "code_with_fix" => _metadata.CodeWithFix,
            _ => throw new ArgumentOutOfRangeException(key)
        };
    }
}