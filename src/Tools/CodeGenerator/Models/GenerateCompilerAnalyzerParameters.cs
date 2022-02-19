// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

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
        var path = Path.GetFullPath(Path.Combine(Source, "src"));


        return ExitCodes.Success;
    }
}