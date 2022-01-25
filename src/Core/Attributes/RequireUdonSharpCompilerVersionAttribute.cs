// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// -------------------------------------------------------------------------------------------

using System;

using NatsunekoLaboratory.UdonAnalyzer.Models;

namespace NatsunekoLaboratory.UdonAnalyzer.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class RequireUdonSharpCompilerVersionAttribute : Attribute
{
    private readonly VersionRange _version;

    public string VersionStr => _version.ToString();

    public RequireUdonSharpCompilerVersionAttribute(string version)
    {
        _version = VersionRange.Parse(version);
    }

    public bool IsFulfill(string version)
    {
        return _version.IsFulfill(version);
    }
}