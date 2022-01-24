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

    public static bool HasProperty(object instance, string key)
    {
        var obj = instance.GetType();
        return obj.GetProperty(key, BindingFlags.Public) != null;
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

    public static void AssignObject(object instance, string key, string value)
    {
        static T Guard<T>(string val, Func<string, T> converter)
        {
            try
            {
                return converter.Invoke(val);
            }
            catch
            {
                throw new ArgumentException($"failed to cast to {typeof(T).FullName} from string");
            }
        }

        var t = instance.GetType().GetProperty(key, BindingFlags.Public)?.PropertyType;
        if (t == null)
            throw new InvalidOperationException($"The property {key} does not have public setter for assigning the parsed value");

        switch (t)
        {
            case { } when t == typeof(string):
                AssignObject(instance, key, (object)value, t);
                break;

            case { } when t == typeof(byte):
                AssignObject(instance, key, Guard(value, byte.Parse), t);
                break;

            case { } when t == typeof(short):
                AssignObject(instance, key, Guard(value, short.Parse), t);
                break;

            case { } when t == typeof(int):
                AssignObject(instance, key, Guard(value, int.Parse), t);
                break;

            case { } when t == typeof(long):
                AssignObject(instance, key, Guard(value, long.Parse), t);
                break;

            case { } when t == typeof(float):
                AssignObject(instance, key, Guard(value, float.Parse), t);
                break;

            case { } when t == typeof(double):
                AssignObject(instance, key, Guard(value, double.Parse), t);
                break;

            case { } when t == typeof(bool):
                AssignObject(instance, key, Guard(value, bool.Parse), t);
                break;
        }
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