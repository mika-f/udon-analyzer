// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace NatsunekoLaboratory.UdonAnalyzer.Models;

public class SafeStack<T> : Stack<T>
{
    public bool CanPop()
    {
        return Count > 0;
    }

    public T? SafePeek()
    {
        return Count > 0 ? Peek() : default;
    }
}