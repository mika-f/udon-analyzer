// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Attributes;

namespace ConsoleCore.Tests.Entities;

public class ParseStringTestEntity
{
    [Option("v", "stringValue")]
    public string Value { get; set; }
}