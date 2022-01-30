// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// -------------------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace NatsunekoLaboratory.UdonAnalyzer.Internal;

internal static class DiagnosticDescriptors
{
    public static readonly DiagnosticDescriptor TryCatchFinallyIsNotSupported = DiagnosticDescriptorFactory.Create(
        "VRC0001",
        "Try/Catch/Finally is not supported by UdonSharp since Udon does not have a way to handle exceptions",
        "Try/Catch/Finally is not supported by UdonSharp since Udon does not have a way to handle exceptions",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

}