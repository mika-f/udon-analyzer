// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// -------------------------------------------------------------------------------------------

using NatsunekoLaboratory.UdonAnalyzer.Extensions;

namespace NatsunekoLaboratory.UdonAnalyzer.Internals.Models;

internal class VersionRangeGreaterThanOrEqualsAndLessThanOrEquals : VersionRange
{
    public VersionRangeGreaterThanOrEqualsAndLessThanOrEquals(string min, string max) : base(min, max) { }

    public override bool IsFulfill(string version)
    {
        var v = GenericVersion.Parse(version);
        return MinVersion.IsGreaterThanOrEquals(v) && MaxVersion.IsLessThanOrEquals(v);
    }
}