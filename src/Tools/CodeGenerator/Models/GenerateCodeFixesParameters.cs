// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Helpers;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGenerator.Models;

public class GenerateCodeFixesParameters
{
    public Task<int> GenerateCodeFixCode()
    {
        return Task.FromResult(ExitCodes.Success);
    }
}