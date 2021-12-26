// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// -------------------------------------------------------------------------------------------

namespace NatsunekoLaboratory.UdonAnalyzer;

public readonly struct AnalyzerOptionDescriptor<T>
{
    public AnalyzerOptionDescriptor(string key, T? defaultValue = default)
    {
        Key = key;
        DefaultValue = defaultValue;
    }

    public string Key { get; }

    public T? DefaultValue { get; }
}