// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// -------------------------------------------------------------------------------------------

using System;

using Microsoft.CodeAnalysis.Diagnostics;

namespace NatsunekoLaboratory.UdonAnalyzer.Extensions;

public static class SyntaxNodeAnalysisContextExtensions
{
    public static T? GetEditorConfigValue<T>(this SyntaxNodeAnalysisContext context, AnalyzerOptionDescriptor<T> descriptor)
    {
        return GetEditorConfigValue(context, descriptor.Key, descriptor.DefaultValue);
    }

    public static T GetEditorConfigValue<T>(this SyntaxNodeAnalysisContext context, string key, T defaultValue)
    {
        var provider = context.Options.AnalyzerConfigOptionsProvider;
        var options = provider.GetOptions(context.Node.SyntaxTree);

        if (options.TryGetValue(key, out var value))
            return typeof(T) switch
            {
                { } when typeof(T) == typeof(string) => (T)(object)value,
                { } when typeof(T) == typeof(bool) => bool.TryParse(value, out var b) ? (T)(object)b : defaultValue,
                { } when typeof(T).IsEnum => Enum.IsDefined(typeof(T), value) ? (T)Enum.Parse(typeof(T), value) : defaultValue,
                { } when typeof(T) == typeof(int) => int.TryParse(value, out var i) ? (T)(object)i : defaultValue,
                _ => throw new ArgumentOutOfRangeException($"not supported type: {typeof(T)}")
            };

        return defaultValue;
    }
}