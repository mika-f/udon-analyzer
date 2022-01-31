// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Interfaces;

namespace NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Models;

public abstract class ExecutorBase<TArg> where TArg : class, new()
{
    private CommandLineParser? _parser;

    protected Func<TArg, Task<int>> Executor { get; }

    protected string[]? Args { get; private set; }

    protected Dictionary<string, ISubCommand> SubCommands { get; }

    protected internal ExecutorBase(Func<TArg, Task<int>> executor)
    {
        Executor = executor;
        SubCommands = new Dictionary<string, ISubCommand>();
    }

    protected void AddCommand(string key, ISubCommand command)
    {
        if (HasSubCommand(key))
            throw new ArgumentException($"The sub-command {key} is already exists");

        SubCommands.Add(key, command);
    }

    protected void AddCommand<T>(string key, ISubCommand<T> command) where T : class, new()
    {
        if (HasSubCommand(key))
            throw new ArgumentException($"The sub-command {key} is already exists");

        SubCommands.Add(key, command);
    }

    protected bool HasSubCommand(string command)
    {
        return SubCommands.ContainsKey(command);
    }

    protected bool TryGetSubCommand([NotNullWhen(true)] out string? command)
    {
        _parser ??= new CommandLineParser(Args!, SubCommands.Count > 0);

        return _parser.TryGetSubCommand(out command);
    }

    protected bool TryParseCommandLineArgs<T>([NotNullWhen(true)] out T? entity, out IReadOnlyCollection<IErrorMessage> errors) where T : class, new()
    {
        _parser ??= new CommandLineParser(Args!, SubCommands.Count > 0);

        return _parser.TryParse(out entity, out errors);
    }

    protected string[] GetRemainingArgs()
    {
        _parser ??= new CommandLineParser(Args!, SubCommands.Count > 0);

        return _parser.GetRemainingParameters();
    }

    [MemberNotNull(nameof(Args))]
    protected async Task<int> RunAsync(string[] args)
    {
        Args = args;

        return await RunAsync().ConfigureAwait(false);
    }

    protected abstract Task<int> RunAsync();
}