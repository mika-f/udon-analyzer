// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Interfaces;

namespace NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Helpers;

internal static class EntityBuilder
{
    public static object? CreateInstance(Type t)
    {
        return Activator.CreateInstance(t);
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