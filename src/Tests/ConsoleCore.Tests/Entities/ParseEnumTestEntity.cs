// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Attributes;

namespace ConsoleCore.Tests.Entities;

public class ParseEnumTestEntity
{
    [Option]
    public ErrorLevel Value { get; set; }
}

public enum ErrorLevel
{
    Error,

    Warning
}