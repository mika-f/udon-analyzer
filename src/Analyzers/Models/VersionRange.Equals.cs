// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using NatsunekoLaboratory.UdonAnalyzer.Extensions;

namespace NatsunekoLaboratory.UdonAnalyzer.Models;

public class VersionRangeEquals : VersionRange
{
    public VersionRangeEquals(string equals) : base(equals, "0") { }

    public override bool IsFulfill(string version)
    {
        return MinVersion.IsSame(GenericVersion.Parse(version));
    }

    public override string ToRangeString()
    {
        return $"[{MinVersion}]";
    }
}