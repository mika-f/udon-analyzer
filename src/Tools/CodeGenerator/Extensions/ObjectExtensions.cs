// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Reflection;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGenerator.Extensions;

public static class ObjectExtensions
{
    public static void CopyTo<T>(this T obj, T another) where T : class
    {
        var srcProperties = obj.GetType()
                               .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                               .Where(w => w.CanRead)
                               .ToList();

        var destProperties = another.GetType()
                                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                    .Where(w => w.CanWrite)
                                    .ToList();

        foreach (var srcProperty in srcProperties)
        {
            var target = destProperties.FirstOrDefault(w => w == srcProperty);
            if (target == null)
                continue;

            target.SetValue(another, srcProperty.GetValue(obj));
        }
    }
}