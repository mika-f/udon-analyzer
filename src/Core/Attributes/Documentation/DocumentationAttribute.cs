// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// -------------------------------------------------------------------------------------------

using System;

namespace NatsunekoLaboratory.UdonAnalyzer.Attributes.Documentation;

[AttributeUsage(AttributeTargets.Class)]
public class DocumentationAttribute : Attribute
{
    public Type Analyzer { get; }

    public DocumentationAttribute(Type analyzer)
    {
        Analyzer = analyzer;
    }
}