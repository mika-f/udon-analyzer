// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// -------------------------------------------------------------------------------------------

using NatsunekoLaboratory.UdonAnalyzer.Extensions;

namespace NatsunekoLaboratory.UdonAnalyzer.Models;

internal class VersionRangeLessThanOrEquals : VersionRange
{
    public VersionRangeLessThanOrEquals(string max) : base("0", max) { }

    public override bool IsFulfill(string version)
    {
        return MaxVersion.IsLessThanOrEquals(GenericVersion.Parse(version));
    }
}