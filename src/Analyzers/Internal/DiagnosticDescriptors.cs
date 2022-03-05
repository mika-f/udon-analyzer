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
        "UdonSharp does not yet support inheriting from classes other than 'UdonSharpBehaviour'",
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

    public static readonly DiagnosticDescriptor DefaultLiteralExpressionsAreNotYetSupported = DiagnosticDescriptorFactory.Create(
        "VSC0010",
        "Default literal expressions are not yet supported by UdonSharp",
        "Default literal expressions are not yet supported by UdonSharp",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor DoesNotSupportMultidimensionalArrays = DiagnosticDescriptorFactory.Create(
        "VSC0011",
        "UdonSharp does not support multidimensional arrays at the moment, use jagged arrays instead for now",
        "UdonSharp does not support multidimensional arrays at the moment, use jagged arrays instead for now",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor DoesNotSupportMultidimensionalArrayAccesses = DiagnosticDescriptorFactory.Create(
        "VSC0012",
        "UdonSharp does not support multidimensional array accesses yet",
        "UdonSharp does not support multidimensional array accesses yet",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor DoesNotCurrentlySupportNullConditionalOperators = DiagnosticDescriptorFactory.Create(
        "VSC0013",
        "UdonSharp does not currently support null conditional operators",
        "UdonSharp does not currently support null conditional operators",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor DoesNotCurrentlySupportStaticMethodDeclarations = DiagnosticDescriptorFactory.Create(
        "VSC0014",
        "UdonSharp does not currently support static method declarations",
        "UdonSharp does not currently support static method declarations",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor DoesNotYetSupportOutParametersOnUserDefinedMethods = DiagnosticDescriptorFactory.Create(
        "VSC0015",
        "UdonSharp does not yet support 'out' parameters on user-defined methods",
        "UdonSharp does not yet support 'out' parameters on user-defined methods",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor DoesNotYetSupportInParametersOnUserDefinedMethods = DiagnosticDescriptorFactory.Create(
        "VSC0016",
        "UdonSharp does not yet support 'in' parameters on user-defined methods",
        "UdonSharp does not yet support 'in' parameters on user-defined methods",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor DoesNotYetSupportRefParametersOnUserDefinedMethods = DiagnosticDescriptorFactory.Create(
        "VSC0017",
        "UdonSharp does not yet support 'ref' parameters on user-defined methods",
        "UdonSharp does not yet support 'ref' parameters on user-defined methods",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor DoesNotSupportReadonlyReferenceLocalVariableDeclaration = DiagnosticDescriptorFactory.Create(
        "VSC0018",
        "UdonSharp does not support 'readonly references' local variable declarations",
        "UdonSharp does not support 'readonly references' local variable declarations",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor DoesNotSupportReturnsReadonlyReferenceOnUserDefinedMethodDeclaration = DiagnosticDescriptorFactory.Create(
        "VSC0019",
        "UdonSharp does not support returns 'readonly references' on user defined method declarations",
        "UdonSharp does not support 'readonly references' on user defined method declarations",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor DoesNotYetSupportGoto = DiagnosticDescriptorFactory.Create(
        "VSC0020",
        "UdonSharp does not yet support goto",
        "UdonSharp does not yet support goto",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor DoesNotYetSupportLabeledStatements = DiagnosticDescriptorFactory.Create(
        "VSC0021",
        "UdonSharp does not yet support labeled statements",
        "UdonSharp does not yet support labeled statements",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor DoesNotYetSupportObjectInitializers = DiagnosticDescriptorFactory.Create(
        "VSC0022",
        "Object initializers are not yet supported by UdonSharp",
        "Object initializers are not yet supported by UdonSharp",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor DoesNotYetSupportGotoCase = DiagnosticDescriptorFactory.Create(
        "VSC0023",
        "UdonSharp does not yet support goto case statement",
        "UdonSharp does not yet support goto case statement",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor DoesNotYetSupportGotoDefault = DiagnosticDescriptorFactory.Create(
        "VSC0024",
        "UdonSharp does not yet support goto default statement",
        "UdonSharp does not yet support goto default statement",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor OnlyOneClassDeclarationPerFileIsCurrentlySupported = DiagnosticDescriptorFactory.Create(
        "VSC0025",
        "Only one class declaration per file is currently supported by UdonSharp",
        "Only one class declaration per file is currently supported by UdonSharp",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    // ReSharper disable once InconsistentNaming
    public static readonly DiagnosticDescriptor GetComponentIsCurrentlyBrokenInUdonForSDK3Components = DiagnosticDescriptorFactory.Create(
        "VSC0026",
        "GetComponent<T> is currently broken in Udon for SDK3 components",
        "GetComponent<T> is currently broken in Udon for SDK3 components",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor DoesNotSupportDefaultArgumentsOrParamsArguments = DiagnosticDescriptorFactory.Create(
        "VSC0027",
        "UdonSharp custom methods currently do not support default arguments or params arguments",
        "UdonSharp custom methods currently do not support default arguments or params arguments",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor DoesNotYetSupportedStaticFields = DiagnosticDescriptorFactory.Create(
        "VSC0028",
        "Static fields are not yet supported by UdonSharp",
        "Static fields are not yet supported by UdonSharp",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor OnlyOneFieldMayTargetPropertyOnThisProperty = DiagnosticDescriptorFactory.Create(
        "VSC0029",
        "Only one field may target property",
        "Only one field may target property '{0}'",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error,
        true,
        "Only one field may target property."
    );

    public static readonly DiagnosticDescriptor InvalidTargetProperty = DiagnosticDescriptorFactory.Create(
        "VSC0030",
        "Invalid target property",
        "Invalid target property for '{0}'",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error,
        true,
        "Invalid target property."
    );

    public static readonly DiagnosticDescriptor TypesMustMatchBetweenPropertyAndVariableChangeField = DiagnosticDescriptorFactory.Create(
        "VSC0031",
        "Types must match between property and variable change field",
        "Types must match between property and variable change field",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    public static readonly DiagnosticDescriptor CannotDefineMethodWithSameNameAsBuiltinUdonSharpBehaviourMethods = DiagnosticDescriptorFactory.Create(
        "VSC0032",
        "Cannot define method with the same name as built-in UdonSharpBehaviour methods",
        "Cannot define method '{0}' with the same name as built-in UdonSharpBehaviour methods",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error,
        true,
        "Cannot define method with the same name as built-in UdonSharpBehaviour methods."
    );

    public static readonly DiagnosticDescriptor OnlySupports1TypeGenericMethodsAtTheMoment = DiagnosticDescriptorFactory.Create(
        "VSC0033",
        "UdonSharp only supports 1 type generic methods at the moment",
        "UdonSharp only supports 1 type generic methods at the moment",
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