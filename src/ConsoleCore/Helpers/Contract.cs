// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;

namespace NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Helpers;

public static class Contract
{
    [Conditional("DEBUG")]
    public static void Assert(bool assertion, string error)
    {
        if (assertion)
            return;

        throw new ArgumentException($"AssertionFailed: {error}");
    }

    [Conditional("DEBUG")]
    public static void Require(bool assertion, string error)
    {
        if (assertion)
            return;

        throw new ArgumentException($"RequirementFailed: {error}");
    }

    [Conditional("DEBUG")]
    public static void Ensuring(bool assertion, string error)
    {
        if (assertion)
            return;

        throw new ArgumentException($"EnsuringFailed: {error}");
    }
}