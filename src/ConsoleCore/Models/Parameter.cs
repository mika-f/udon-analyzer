﻿// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

namespace NatsunekoLaboratory.UdonAnalyzer.ConsoleCore.Models;

internal record Parameter(int Order, string? LongName, string? ShortName, string Value) { }