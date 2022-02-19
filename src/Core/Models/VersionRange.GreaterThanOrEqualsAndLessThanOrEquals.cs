// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// -------------------------------------------------------------------------------------------

using NatsunekoLaboratory.UdonAnalyzer.Extensions;

namespace NatsunekoLaboratory.UdonAnalyzer.Models;

public class VersionRangeGreaterThanOrEqualsAndLessThanOrEquals : VersionRange
{
    public VersionRangeGreaterThanOrEqualsAndLessThanOrEquals(string min, string max) : base(min, max) { }

    public override bool IsFulfill(string version)
    {
        var v = GenericVersion.Parse(version);
        return MinVersion.IsGreaterThan(v) && MaxVersion.IsGreaterThan(v);
    }

    public override string ToRangeString()
    {
        return $"[{MinVersion},{MaxVersion}]";
    }
}