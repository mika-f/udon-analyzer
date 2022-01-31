// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;

using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Helpers;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Interfaces;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Models;

namespace NatsunekoLaboratory.UdonAnalyzer.ConsoleCore;

public sealed class ConsoleHost<T> : ExecutorBase<T> where T : class, new()
{
    internal ConsoleHost(Func<T, Task<int>> executor) : base(executor) { }

    protected override async Task<int> RunAsync()
    {
        if (TryGetSubCommand(out var command) && HasSubCommand(command))
        {
            var cmd = SubCommands.First(w => w.Key == command).Value;
            return await cmd.RunAsync(GetRemainingArgs()).ConfigureAwait(false);
        }

        if (TryParseCommandLineArgs<T>(out var args, out var errors))
            return await Executor.Invoke(args).ConfigureAwait(false);

        foreach (var error in errors)
            await Console.Error.WriteLineAsync(error.ToMessageString()).ConfigureAwait(false);

        return ExitCodes.Failure;
    }


    public new ConsoleHost<T> AddCommand(string key, ISubCommand command)
    {
        base.AddCommand(key, command);

        return this;
    }

    public new ConsoleHost<T> AddCommand<TArgs>(string key, ISubCommand<TArgs> command) where TArgs : class, new()
    {
        base.AddCommand(key, command);

        return this;
    }

    public new async Task<int> RunAsync(string[] args)
    {
        return await base.RunAsync(args);
    }
}