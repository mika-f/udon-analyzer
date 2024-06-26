﻿// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.CSharp.Workspace;

public record AnalyzerMetadata(string Id, string Title, string Description, string Category, DiagnosticSeverity Severity, string? RuntimeVersion, string? CompilerVersion, string? CodeWithDiagnostic, string? CodeWithFix, string? ClassName);