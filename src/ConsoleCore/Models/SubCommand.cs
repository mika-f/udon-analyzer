// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;

using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Helpers;

namespace NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Models;

public static class SubCommand
{
    public static SubCommand<Anonymous> Create()
    {
        return Create(() => ExitCodes.Failure);
    }

    public static SubCommand<Anonymous> Create(Func<int> invoke)
    {
        return Create(() => Task.FromResult(invoke.Invoke()));
    }

    public static SubCommand<Anonymous> Create(Func<Task<int>> invoke)
    {
        Task<int> Invoker(Anonymous _)
        {
            return invoke.Invoke();
        }

        return new SubCommand<Anonymous>(Invoker);
    }

    public static SubCommand<T> Create<T>(Func<T, int> invoke) where T : class, new()
    {
        return Create<T>(w => Task.FromResult(invoke.Invoke(w)));
    }

    public static SubCommand<T> Create<T>(Func<T, Task<int>> invoke) where T : class, new()
    {
        return new SubCommand<T>(invoke);
    }
}