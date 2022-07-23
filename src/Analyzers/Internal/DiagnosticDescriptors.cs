// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace NatsunekoLaboratory.UdonAnalyzer.Internal;

internal static class DiagnosticDescriptors
{
    #region VRC - VRChat SDK

    public static readonly DiagnosticDescriptor TryCatchFinallyIsNotSupported = DiagnosticDescriptorFactory.Create(
        "VRC0001",
        "Try-Catch-Finally is not supported",
        "Try-Catch-Finally is not supported",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor DoesNotSupportThrowingExceptions = DiagnosticDescriptorFactory.Create(
        "VRC0002",
        "Does not support throwing exceptions",
        "Does not support throwing exceptions",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    // INSERT_VRC_DESCRIPTOR_HERE

    #endregion

    #region VSC - UdonSharp Compiler

    public static readonly DiagnosticDescriptor UdonSharpBehaviourClassesMustBeSameNameAsCsharpFile = DiagnosticDescriptorFactory.Create(
        "VSC0001",
        "UdonSharpBehaviour classes must be same name as C# files",
        "UdonSharpBehaviour classes must be same name as C# files",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor LocalMethodDeclarationsAreNotCurrentlySupported = DiagnosticDescriptorFactory.Create(
        "VSC0002",
        "Local method declarations are not currently supported",
        "Local method declarations are not currently supported",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor NestedTypeDeclarationsAreNotCurrentlySupported = DiagnosticDescriptorFactory.Create(
        "VSC0003",
        "Nested type declarations are not currently supported",
        "Nested type declarations are not currently supported",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor CannotUseTypeofOnUserDefinedTypes = DiagnosticDescriptorFactory.Create(
        "VSC0004",
        "Cannot use typeof on user-defined types",
        "Cannot use typeof on user-defined types",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor DoesNotYetSupportInitializerLists = DiagnosticDescriptorFactory.Create(
        "VSC0005",
        "Does not yet support initializer lists",
        "Does not yet support initializer lists",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor MultidimensionalArraysAreNotYetSupported = DiagnosticDescriptorFactory.Create("VSC0006", "Multidimensional arrays are not yet supported", "Multidimensional arrays are not yet supported", DiagnosticCategories.Compiler, DiagnosticSeverity.Error);

    // INSERT_VSC_DESCRIPTOR_HERE

    #endregion
}