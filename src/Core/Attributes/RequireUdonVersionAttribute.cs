// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;

namespace NatsunekoLaboratory.UdonAnalyzer.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class RequireUdonVersionAttribute : Attribute
{
    public string VersionRange { get; }

    public RequireUdonVersionAttribute(string versionRange)
    {
        VersionRange = versionRange;
    }
}