﻿// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Threading.Tasks;

namespace NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Interfaces;

public interface ISubCommand
{
    Task<int> RunAsync(string[] args);
}