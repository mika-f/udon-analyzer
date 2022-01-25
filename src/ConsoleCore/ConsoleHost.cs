// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Helpers;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Interfaces;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Models;

namespace NatsunekoLaboratory.UdonAnalyzer.ConsoleCore;

public class ConsoleHost
{
    private readonly Func<Task<int>> _default;
    private CommandLineParser? _parser;

    protected Dictionary<string, SubCommand> SubCommands { get; }

    protected string[] Args { get; }

    protected ConsoleHost(string[] args, Func<Task<int>> @default)
    {
        Args = args;
        SubCommands = new Dictionary<string, SubCommand>();
        _default = @default;
    }

    public static ConsoleHost Create(string[] args)
    {
        return Create(args, () => ExitCodes.Failure);
    }

    public static ConsoleHost Create(string[] args, Func<int> @default)
    {
        return Create(args, () => Task.FromResult(@default.Invoke()));
    }

    public static ConsoleHost Create(string[] args, Func<Task<int>> @default)
    {
        return new ConsoleHost(args, @default);
    }

    public static ConsoleHost<T> Create<T>(string[] args, Func<T, int> @default) where T : class, new()
    {
        return Create<T>(args, w => Task.FromResult(@default.Invoke(w)));
    }

    public static ConsoleHost<T> Create<T>(string[] args, Func<T, Task<int>> @default) where T : class, new()
    {
        return new ConsoleHost<T>(args, @default);
    }

    public ConsoleHost AddCommand(string key, SubCommand command)
    {
        if (HasSubCommand(key))
            throw new ArgumentException($"The sub-command {key} is already exists");

        SubCommands.Add(key, command);
        return this;
    }

    public ConsoleHost AddCommand<T>(string key, SubCommand<T> command) where T : class, new()
    {
        if (HasSubCommand(key))
            throw new ArgumentException($"The sub-command {key} is already exists");

        SubCommands.Add(key, command);
        return this;
    }

    protected bool HasSubCommand(string command)
    {
        return SubCommands.ContainsKey(command);
    }

    protected bool TryGetSubCommand([NotNullWhen(true)] out string? command)
    {
        _parser ??= new CommandLineParser(Args, SubCommands.Count > 0);

        return _parser.TryGetSubCommand(out command);
    }

    protected bool TryParseCommandLineArgs<T>([NotNullWhen(true)] out T? entity, out IReadOnlyCollection<IErrorMessage> errors) where T : class, new()
    {
        _parser ??= new CommandLineParser(Args, SubCommands.Count > 0);

        return _parser.TryParse(out entity, out errors);
    }

    protected bool TryParseCommandLineArgs(Type t, [NotNullWhen(true)] out object? entity, out IReadOnlyCollection<IErrorMessage> errors)
    {
        _parser ??= new CommandLineParser(Args, SubCommands.Count > 0);

        return _parser.TryParse(t, out entity, out errors);
    }

    public virtual async Task<int> RunAsync()
    {
        return 0;
    }
}