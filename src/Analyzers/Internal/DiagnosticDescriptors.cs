// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace NatsunekoLaboratory.UdonAnalyzer.Internal;

internal static class DiagnosticDescriptors
{
    #region VSC - UdonSharp Compiler

    public static readonly DiagnosticDescriptor UdonSharpBehaviourClassesMustBeSameNameAsCsharpFile = DiagnosticDescriptorFactory.Create(
        "VSC0001",
        "UdonSharpBehaviour classes must be same name as C# files",
        "UdonSharpBehaviour classes must be same name as C# files",
        DiagnosticCategories.Compiler,
        DiagnosticSeverity.Error
    );

    // INSERT_VSC_DESCRIPTOR_HERE

    #endregion

    #region VRC - VRChat SDK

    // INSERT_VRC_DESCRIPTOR_HERE

    #endregion
}