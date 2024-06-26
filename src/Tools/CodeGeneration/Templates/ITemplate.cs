﻿// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Immutable;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Templates;

public interface ITemplate
{
    string Key { get; }

    string Content { get; }

    string VariableFormat { get; }

    ImmutableArray<string> Variables { get; }

    string OutPath { get; }
}