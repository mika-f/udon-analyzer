// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Helpers;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Interfaces;
using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Models;

namespace NatsunekoLaboratory.UdonAnalyzer.ConsoleCore;

internal class CommandLineParser
{
    private static readonly Type[] SupportedTypes =
    {
        typeof(bool),
        typeof(byte),
        typeof(sbyte),
        typeof(char),
        typeof(decimal),
        typeof(double),
        typeof(float),
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong),
        typeof(short),
        typeof(ushort),
        typeof(string)
    };

    private static readonly Type[] SupportedInheritTypes =
    {
        typeof(List<>),
        typeof(Enum)
    };

    private readonly string[] _args;
    private readonly bool _hasSubCommand;

    public CommandLineParser(string[] args, bool hasSubCommand)
    {
        _args = args;
        _hasSubCommand = hasSubCommand;
    }

    public bool TryGetSubCommand([NotNullWhen(true)] out string? command)
    {
        command = _hasSubCommand && _args.Length > 0 ? _args[0] : null;
        return !string.IsNullOrWhiteSpace(command);
    }

    public bool TryParse<T>([NotNullWhen(true)] out T? entity, out IReadOnlyCollection<IErrorMessage> errors) where T : class, new()
    {
        var r = TryParse(typeof(T), out var rawEntity, out errors);
        entity = rawEntity as T;

        return r;
    }

    public bool TryParse(Type t, [NotNullWhen(true)] out object? entity, out IReadOnlyCollection<IErrorMessage> errors)
    {
        static bool CreateError(string message, out object? e, out IReadOnlyCollection<IErrorMessage> errs)
        {
            e = null;
            errs = new List<IErrorMessage> { new ErrorMessage(message) }.AsReadOnly();

            return false;
        }

        var instance = EntityBuilder.CreateInstance(t);
        if (instance == null)
            return CreateError($"failed to create the instance of Entity<{t.FullName}>: it does not have the default constructor", out entity, out errors);

        entity = Parse(instance, out var internalErrors);

        if (internalErrors.Count > 0)
        {
            errors = internalErrors;
            return false;
        }

        return EntityBuilder.ValidateObject(entity, out errors);
    }

    private object Parse(object instance, out IReadOnlyCollection<IErrorMessage> errors)
    {
        var parameters = ParseArguments();
        AssignParametersToObject(instance, parameters, out errors);

        return instance;
    }

    private List<Parameter> ParseArguments()
    {
        var isParsingNamedParams = false;
        var category = -1;
        var name = "";
        var parameters = new List<Parameter>();

        foreach (var (arg, i) in GetParsingArgs().Select((w, i) => (w, i)))
            switch (true)
            {
                case { } when arg.StartsWith("--") && !isParsingNamedParams:
                    isParsingNamedParams = true;
                    category = 1;
                    name = arg["--".Length..];
                    break;

                case { } when arg.StartsWith("--") && isParsingNamedParams:
                    parameters.Add(new Parameter(i, category == 1 ? name : null, category == 2 ? name : null, "true"));
                    category = 1;
                    name = arg["--".Length..];
                    break;

                case { } when arg.StartsWith("-") && !isParsingNamedParams:
                    category = 2;
                    name = arg["-".Length..];
                    if (name.Length != 1)
                        parameters.Add(new Parameter(i, null, name[0].ToString(), name[1..]));
                    else
                        isParsingNamedParams = true;

                    break;

                case { } when isParsingNamedParams:
                    parameters.Add(new Parameter(i - 1, category == 1 ? name : null, category == 2 ? name : null, arg));
                    category = -1;
                    isParsingNamedParams = false;
                    break;

                default:
                    parameters.Add(new Parameter(i, null, null, arg));
                    break;
            }

        return parameters;
    }

    private static void AssignParametersToObject(object instance, List<Parameter> parameters, out IReadOnlyCollection<IErrorMessage> errors)
    {
        var internalErrors = new List<IErrorMessage>();

        if (CheckNamedParametersDuplication(parameters))
        {
            internalErrors.Add(new ErrorMessage("duplicated parameters"));
            errors = internalErrors.AsReadOnly();
            return;
        }

        foreach (var property in GetTargetProperties(instance))
        {
            var attr = (OptionAttribute)Attribute.GetCustomAttribute(property, typeof(OptionAttribute))!;
            if (attr.AutomaticSetLongName)
                attr.LongName = property.Name;

            if (IsAssignableToObject(attr, parameters, out var parameter))
                try
                {
                    var value = CastToTFromString(property.PropertyType, parameter.Value, attr.SeparatorChar);
                    property.SetMethod!.Invoke(instance, new[] { value });
                }
                catch (Exception e)
                {
                    internalErrors.Add(new ErrorMessage(e.Message));
                }
            else if (attr.IsRequired)
                internalErrors.Add(new ErrorMessage($"Property '{property.Name}' is not provided value"));
        }

        errors = internalErrors.AsReadOnly();
    }

    private static bool CheckNamedParametersDuplication(List<Parameter> parameters)
    {
        var namedParameters = parameters.Where(w => w.LongName != null || w.ShortName != null).ToList();
        return namedParameters.DistinctBy(w => w.LongName).Count() != namedParameters.Count || namedParameters.DistinctBy(w => w.ShortName).Count() != namedParameters.Count;
    }

    private static bool IsAssignableToObject(OptionAttribute attr, List<Parameter> parameters, [NotNullWhen(true)] out Parameter? parameter)
    {
        if (parameters.Any(w => attr.ShortName != "" && w.ShortName == attr.ShortName))
        {
            parameter = parameters.First(w => attr.ShortName != "" && w.ShortName == attr.ShortName);
            return true;
        }

        if (parameters.Any(w => attr.LongName != "" && w.LongName == attr.LongName))
        {
            parameter = parameters.First(w => attr.LongName != "" && w.LongName == attr.LongName);
            return true;
        }

        if (parameters.Any(w => w.Order == attr.Order))
        {
            parameter = parameters.First(w => w.Order == attr.Order);
            return true;
        }

        parameter = null;
        return false;
    }

    private static object CastToTFromString(Type t, string str, char separator)
    {
        switch (t)
        {
            case { } when t == typeof(string):
                return str;

            case { } when t == typeof(bool):
                return bool.Parse(str);

            case { } when t == typeof(byte):
                return byte.Parse(str);

            case { } when t == typeof(sbyte):
                return sbyte.Parse(str);

            case { } when t == typeof(char):
                return char.Parse(str);

            case { } when t == typeof(decimal):
                return decimal.Parse(str);

            case { } when t == typeof(double):
                return double.Parse(str);

            case { } when t == typeof(float):
                return float.Parse(str);

            case { } when t == typeof(int):
                return int.Parse(str);

            case { } when t == typeof(uint):
                return uint.Parse(str);

            case { } when t == typeof(long):
                return long.Parse(str);

            case { } when t == typeof(ulong):
                return ulong.Parse(str);

            case { } when t == typeof(short):
                return short.Parse(str);

            case { } when t == typeof(ushort):
                return ushort.Parse(str);

            case { IsEnum: true }:
                return Enum.Parse(t, str);

            case { } when t == typeof(List<>):
                return str.Split(separator).Select(w => CastToTFromString(t.GetElementType()!, w, separator)).ToList();

            case { IsArray: true }:
                return str.Split(separator).Select(w => CastToTFromString(t.GetElementType()!, w, separator)).ToArray();
        }

        throw new ArgumentOutOfRangeException();
    }

    private static List<PropertyInfo> GetTargetProperties(object instance)
    {
        bool IsSupportedArray(Type t)
        {
            return t.IsArray && SupportedTypes.Contains(t.GetElementType()!);
        }

        bool IsSupportedInheritance(Type t)
        {
            return SupportedInheritTypes.Contains(t.BaseType);
        }

        return instance.GetType()
                       .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                       .Where(w => w.SetMethod != null && Attribute.GetCustomAttribute(w, typeof(OptionAttribute)) != null)
                       .Where(w => SupportedTypes.Contains(w.PropertyType) || IsSupportedArray(w.PropertyType) || IsSupportedInheritance(w.PropertyType))
                       .ToList();
    }

    private IEnumerable<string> GetParsingArgs()
    {
        return _hasSubCommand && _args.Length > 0 ? _args[1..] : _args;
    }
}