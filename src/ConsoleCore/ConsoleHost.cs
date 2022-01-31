// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;

using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Helpers;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Models;

namespace NatsunekoLaboratory.UdonAnalyzer.ConsoleCore;

public static class ConsoleHost
{
    public static ConsoleHost<Anonymous> Create()
    {
        return Create(() => ExitCodes.Failure);
    }

    public static ConsoleHost<Anonymous> Create(Func<int> @default)
    {
        return Create(() => Task.FromResult(@default.Invoke()));
    }

    public static ConsoleHost<Anonymous> Create(Func<Task<int>> @default)
    {
        Task<int> Executor(Anonymous _)
        {
            return @default.Invoke();
        }

        return new ConsoleHost<Anonymous>(Executor);
    }

    public static ConsoleHost<T> Create<T>(Func<T, int> @default) where T : class, new()
    {
        return Create<T>(w => Task.FromResult(@default.Invoke(w)));
    }

    public static ConsoleHost<T> Create<T>(Func<T, Task<int>> @default) where T : class, new()
    {
        return new ConsoleHost<T>(@default);
    }
}