// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Attributes;

#pragma warning disable CS8618

namespace ConsoleCore.Tests.Entities;

public class ParseOptionsWithDoubleDashEntity
{
    [Option(Order = 0)]
    public int IntValue { get; set; }

    [Option(Order = 1)]
    public string StrValue1 { get; set; }

    [Option(Order = 2)]
    public string StrValue2 { get; set; }

    [Option(Order = 3)]
    public string StrValue3 { get; set; }

    [Option(Order = 4)]
    public long LongValue { get; set; }

    [Option("stringValue")]
    public string StrValue { get; set; }
}