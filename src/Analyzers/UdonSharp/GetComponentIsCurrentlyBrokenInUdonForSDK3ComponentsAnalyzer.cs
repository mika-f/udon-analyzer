// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

using NatsunekoLaboratory.UdonAnalyzer.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.Extensions;
using NatsunekoLaboratory.UdonAnalyzer.Internal;

namespace NatsunekoLaboratory.UdonAnalyzer.UdonSharp;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[RequireUdonVersion("[2021.11.24.16.19,)")]
[RequireUdonSharpCompilerVersion("[0.20.3,)")]
// ReSharper disable once InconsistentNaming
public class GetComponentIsCurrentlyBrokenInUdonForSDK3ComponentsAnalyzer : BaseDiagnosticAnalyzer
{
    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.GetComponentIsCurrentlyBrokenInUdonForSDK3Components;

    private static readonly HashSet<string> BrokenGetComponentTypes = new()
    {
        "VRC.SDKBase.VRC_AvatarPedestal",
        "VRC.SDK3.Components.VRCAvatarPedestal",
        "VRC.SDKBase.VRC_Pickup",
        "VRC.SDK3.Components.VRCPickup",
        "VRC.SDKBase.VRC_PortalMarker",
        "VRC.SDK3.Components.VRCPortalMarker",
        "VRC.SDKBase.VRCStation",
        "VRC.SDK3.Components.VRCStation",
        "VRC.SDK3.Video.Components.VRCUnityVideoPlayer",
        "VRC.SDK3.Video.Components.AVPro.VRCAVProVideoPlayer",
        "VRC.SDK3.Video.Components.Base.BaseVRCVideoPlayer",
        "VRC.SDK3.Components.VRCObjectPool",
        "VRC.SDK3.Components.VRCObjectSync"
    };

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, true, AnalyzeInvocationExpression), SyntaxKind.InvocationExpression);
    }

    private void AnalyzeInvocationExpression(SyntaxNodeAnalysisContext context)
    {
        var invocation = (InvocationExpressionSyntax)context.Node;
        if (invocation.Expression is not GenericNameSyntax generics)
            return;

        if (!generics.Identifier.ValueText.StartsWith("GetComponent"))
            return;

        var typeArgument = generics.TypeArgumentList.Arguments.First();
        if (BrokenGetComponentTypes.Any(w => typeArgument.IsClassOf(w, context.SemanticModel)))
            DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, invocation);
    }
}