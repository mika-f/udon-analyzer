// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// -------------------------------------------------------------------------------------------

using NatsunekoLaboratory.UdonAnalyzer.Extensions;

namespace NatsunekoLaboratory.UdonAnalyzer.Models;

internal class VersionRangeGreaterThanOrEquals : VersionRange
{
    public VersionRangeGreaterThanOrEquals(string min) : base(min, "0") { }

    public override bool IsFulfill(string version)
    {
        return MinVersion.IsLessThanOrEquals(GenericVersion.Parse(version));
    }
}