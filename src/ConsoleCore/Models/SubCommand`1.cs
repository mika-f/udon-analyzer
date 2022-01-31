// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;

using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Helpers;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Interfaces;

namespace NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Models;

public class SubCommand<T> : ExecutorBase<T>, ISubCommand<T> where T : class, new()
{
    private readonly Func<T, Task<int>> _invoke;

    internal SubCommand(Func<T, Task<int>> invoke) : base(invoke)
    {
        _invoke = invoke;
    }

    public new SubCommand<T> AddCommand(string key, ISubCommand command)
    {
        base.AddCommand(key, command);

        return this;
    }

    public new SubCommand<T> AddCommand<TArgs>(string key, ISubCommand<TArgs> command) where TArgs : class, new()
    {
        base.AddCommand(key, command);

        return this;
    }

    protected override async Task<int> RunAsync()
    {
        if (TryGetSubCommand(out var command) && HasSubCommand(command))
        {
            var cmd = SubCommands.First(w => w.Key == command).Value;
            return await cmd.RunAsync(GetRemainingArgs());
        }

        if (TryParseCommandLineArgs<T>(out var args, out var errors))
            return await _invoke.Invoke(args).ConfigureAwait(false);

        foreach (var error in errors)
            await Console.Error.WriteLineAsync(error.ToMessageString()).ConfigureAwait(false);

        return ExitCodes.Failure;
    }

    public new Task<int> RunAsync(string[] args)
    {
        return base.RunAsync(args);
    }
}