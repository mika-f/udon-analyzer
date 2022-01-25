// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;

using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Helpers;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Models;

namespace NatsunekoLaboratory.UdonAnalyzer.ConsoleCore;

public sealed class ConsoleHost<T> : ConsoleHost where T : class, new()
{
    private readonly Func<T, Task<int>> _default;

    internal ConsoleHost(string[] args, Func<T, Task<int>> @default) : base(args, () => Task.FromResult(ExitCodes.Failure))
    {
        _default = @default;
    }

    public override async Task<int> RunAsync()
    {
        if (TryParseCommandLineArgs<T>(out var args, out var errors))
            return await _default.Invoke(args).ConfigureAwait(false);

        foreach (var error in errors)
            await Console.Error.WriteLineAsync(error.ToMessageString()).ConfigureAwait(false);

        return ExitCodes.Failure;
    }
}