// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Attributes;

#pragma warning disable CS8618

namespace ConsoleCore.Tests.Entities;

public class ParseOrderedOptionsEntity
{
    [Option(Order = 0)]
    public string StrValue { get; set; }

    [Option(Order = 1)]
    public int IntValue { get; set; }
}