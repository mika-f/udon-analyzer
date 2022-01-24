// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;

namespace NatsunekoLaboratory.UdonAnalyzer.AnalyzerSpec.Attributes;

[AttributeUsage(AttributeTargets.Class)]
[DebuggerDisplay("Describe(Analyzer={Analyzer}, Category={Category})")]
public class DescribeAttribute : Attribute
{
    public Type Analyzer { get; }

    public string Category { get; }

    public DescribeAttribute(Type analyzer, string category)
    {
        Analyzer = analyzer;
        Category = category;
    }
}