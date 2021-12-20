// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;

namespace NatsunekoLaboratory.UdonAnalyzer.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class RequireUdonSharpCompilerVersionAttribute : Attribute
{
    public string VersionRange { get; }

    public RequireUdonSharpCompilerVersionAttribute(string versionRange)
    {
        VersionRange = versionRange;
    }
}