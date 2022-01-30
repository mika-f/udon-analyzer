// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// -------------------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace NatsunekoLaboratory.UdonAnalyzer.Internal;

internal static class DiagnosticDescriptors
{
    #region VRC - VRChat SDK

    public static readonly DiagnosticDescriptor TryCatchFinallyIsNotSupported = DiagnosticDescriptorFactory.Create(
        "VRC0001",
        "Try/Catch/Finally is not supported by UdonSharp since Udon does not have a way to handle exceptions",
        "Try/Catch/Finally is not supported by UdonSharp since Udon does not have a way to handle exceptions",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor DoesNotSupportThrowingExceptions = DiagnosticDescriptorFactory.Create(
        "VRC0002",
        "UdonSharp does not support throwing exceptions since Udon does not have support for exception throwing at the moment",
        "UdonSharp does not support throwing exceptions since Udon does not have support for exception throwing at the moment",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    #endregion
}