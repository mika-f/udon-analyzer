// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace NatsunekoLaboratory.UdonAnalyzer.Internal;

internal static class DiagnosticDescriptors
{
    #region VSC - UdonSharp Compiler

    public static readonly DiagnosticDescriptor DoesNotSupportInheritingFromInterfaces = DiagnosticDescriptorFactory.Create(
        "VSC0001",
        "UdonSharp does not yet support inheriting from interfaces",
        "UdonSharp does not yet support inheriting from interfaces",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor BehavioursMustInheritFromSpecifiedClassInsteadOfMonoBehaviour = DiagnosticDescriptorFactory.Create(
        "VSC0002",
        "UdonSharp behaviours must inherit from 'UdonSharpBehaviour' instead of 'MonoBehaviour'",
        "UdonSharp behaviours must inherit from 'UdonSharpBehaviour' instead of 'MonoBehaviour'",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor DoesNotYetSupportInheritingFromClassesOtherThanSpecifiedClass = DiagnosticDescriptorFactory.Create(
        "VSC0003",
        "UdonSharp does not yet support inheriting from classes other than 'UdonSharpBehaviour'",
        "UdonSharp does not yet support inheriting from classes other than '{0}'",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor DoesNotCurrentlySupportConstructorsOnBehaviours = DiagnosticDescriptorFactory.Create(
        "VSC0004",
        "UdonSharp does not currently support constructors on UdonSharpBehaviours, use the Start() event to initialize instead",
        "UdonSharp does not currently support constructors on UdonSharpBehaviours, use the Start() event to initialize instead",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor OnlySupportsClassesThatInheritFromUdonSharpBehaviour = DiagnosticDescriptorFactory.Create(
        "VSC0005",
        "UdonSharp only supports classes that inherit from 'UdonSharpBehaviour' at the moment",
        "UdonSharp only supports classes that inherit from 'UdonSharpBehaviour' at the moment",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor DoesNotCurrentlySupportStaticUserDefinedProperties = DiagnosticDescriptorFactory.Create(
        "VSC0006",
        "UdonSharp does not currently support static user-defined property declarations",
        "UdonSharp does not currently support static user-defined property declarations",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor DoesNotCurrentlySupportInitializersOnProperties = DiagnosticDescriptorFactory.Create(
        "VSC0007",
        "UdonSharp does not currently support initializers on properties",
        "UdonSharp does not currently support initializers on properties",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor BaseTypeCallingIsNotYetSupported = DiagnosticDescriptorFactory.Create(
        "VSC0008",
        "Base type calling is not yet supported by UdonSharp",
        "Base type calling is not yet supported by UdonSharp",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor DefaultExpressionsAreNotYetSupported = DiagnosticDescriptorFactory.Create(
        "VSC0009",
        "Default expressions are not yet supported by UdonSharp",
        "Default expressions are not yet supported by UdonSharp",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    // INSERT_VSC_DESCRIPTOR_HERE

    #endregion

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

    public static readonly DiagnosticDescriptor DoesNotSupportTheAwakeEvent = DiagnosticDescriptorFactory.Create(
        "VRC0003",
        "Udon does not support the 'Awake' event, use 'Start' instead",
        "Udon does not support the 'Awake' event, use 'Start' instead",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    // INSERT_VRC_DESCRIPTOR_HERE

    #endregion
}