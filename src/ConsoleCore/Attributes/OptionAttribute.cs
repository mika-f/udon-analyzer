// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;

namespace NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class OptionAttribute : Attribute
{
    public string ShortName { get; }

    public string LongName { get; internal set; }

    public bool AutomaticSetLongName { get; }

    public bool IsRequired { get; init; }

    public string HelpText { get; init; } = string.Empty;

    public char SeparatorChar { get; init; } = ',';

    public uint Order { get; init; } = uint.MaxValue;

    public OptionAttribute(string shortName, string longName)
    {
        ShortName = shortName;
        LongName = longName;
        AutomaticSetLongName = false;
    }

    public OptionAttribute(char shortName) : this(shortName.ToString(), string.Empty)
    {
        AutomaticSetLongName = true;
    }

    public OptionAttribute(string longName) : this(string.Empty, longName)
    {
        AutomaticSetLongName = false;
    }

    public OptionAttribute() : this(string.Empty, string.Empty)
    {
        AutomaticSetLongName = true;
    }
}