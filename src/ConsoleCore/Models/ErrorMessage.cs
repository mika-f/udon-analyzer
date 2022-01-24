// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Interfaces;

namespace NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Models;

public record ErrorMessage(string Message) : IErrorMessage
{
    public string ToMessageString()
    {
        return Message;
    }
}