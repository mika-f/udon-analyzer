// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;

namespace NatsunekoLaboratory.UdonAnalyzer.Extensions;

public static class Int32Extensions
{
    public static void Times(this int obj, Action action)
    {
        for (var i = 0; i < obj; i++)
            action.Invoke();
    }
}