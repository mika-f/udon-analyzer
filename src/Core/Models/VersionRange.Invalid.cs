// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// -------------------------------------------------------------------------------------------

namespace NatsunekoLaboratory.UdonAnalyzer.Models;

internal class VersionRangeInvalid : VersionRange
{
    public VersionRangeInvalid() : base("0", "0") { }

    public override bool IsFulfill(string version)
    {
        return false;
    }

    public override string ToRangeString()
    {
        return "INVALID";
    }
}