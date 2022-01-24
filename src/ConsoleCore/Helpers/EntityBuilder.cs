// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Interfaces;

namespace NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Helpers;

internal static class EntityBuilder
{
    public static object? CreateInstance(Type t)
    {
        return Activator.CreateInstance(t);
    }

    public static void AssignObject(object instance, string key, object value, Type? t = null)
    {
        var obj = instance.GetType();
        var setter = obj.GetProperty(key, BindingFlags.Public)?.SetMethod;
        if (setter == null)
            throw new InvalidOperationException($"The property {key} does not have public setter for assigning the parsed value");

        t ??= setter.GetParameters().First().ParameterType;

        if (setter.GetParameters().First().ParameterType != t)
            throw new InvalidOperationException($"The property {key} does not have the type {t.FullName} for assigning the parsed value");

        setter.Invoke(instance, new[] { value });
    }

    public static bool ValidateObject(object instance, out IReadOnlyCollection<IErrorMessage> errors)
    {
        if (instance is IValidatableEntity validatable)
        {
            var r = validatable.Validate(out var internalErrors);
            errors = internalErrors.AsReadOnly();

            return r;
        }

        errors = new List<IErrorMessage>().AsReadOnly();
        return true;
    }
}