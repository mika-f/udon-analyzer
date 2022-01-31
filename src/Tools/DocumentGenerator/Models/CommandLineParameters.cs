// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Interfaces;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Models;

namespace NatsunekoLaboratory.UdonAnalyzer.DocumentGenerator.Models;

public class CommandLineParameters : IValidatableEntity
{
    [Option(Order = 0)]
    public string Path { get; set; } = GetDefaultPath();

    public bool Validate(out List<IErrorMessage> errors)
    {
        errors = new List<IErrorMessage>();

        if (!Directory.Exists(Path))
            errors.Add(new ErrorMessage("specified directory is not found on filesystem"));

        return errors.Count == 0;
    }

    private static string GetDefaultPath()
    {
#if DEBUG
        return "../../../../../../";
#else
        return Environment.CurrentDirectory;

#endif
    }
}