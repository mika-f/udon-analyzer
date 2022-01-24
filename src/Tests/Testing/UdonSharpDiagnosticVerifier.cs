// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using Microsoft.CodeAnalysis.Diagnostics;

namespace NatsunekoLaboratory.UdonAnalyzer.Testing;

public class UdonSharpDiagnosticVerifier<TAnalyzer> : DiagnosticVerifier<TAnalyzer, UdonSharpStandaloneProject> where TAnalyzer : DiagnosticAnalyzer, new() { }