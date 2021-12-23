// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;

using NatsunekoLaboratory.UdonAnalyzer.Internals.Models;

namespace NatsunekoLaboratory.UdonAnalyzer.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class RequireUdonVersionAttribute : Attribute
{
    private readonly VersionRange _version;

    public RequireUdonVersionAttribute(string version)
    {
        _version = VersionRange.Parse(version);
    }

    public bool IsFulfill(string version)
    {
        return _version.IsFulfill(version);
    }
}