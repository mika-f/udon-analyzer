// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

using NatsunekoLaboratory.UdonAnalyzer.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.Internal;

namespace NatsunekoLaboratory.UdonAnalyzer.UdonSharp;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[RequireUdonVersion("[2021.11.24.16.19,)")]
[RequireUdonSharpCompilerVersion("[0.20.3,)")]
public class CannotDefineMethodWithSameNameAsBuiltinUdonSharpBehaviourMethodsAnalyzer : BaseDiagnosticAnalyzer
{
    private static readonly string[] BuiltinUdonSharpMethods =
    {
        "SendCustomEvent",
        "SendCustomNetworkEvent",
        "SetProgramVariable",
        "GetProgramVariable",
        "VRCInstantiate",
        "GetUdonTypeID",
        "GetUdonTypeName"
    };

    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.CannotDefineMethodWithSameNameAsBuiltinUdonSharpBehaviourMethods;

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, true, AnalyzeMethodDeclaration), SyntaxKind.MethodDeclaration);
    }

    private void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
    {
        var declaration = (MethodDeclarationSyntax)context.Node;
        if (BuiltinUdonSharpMethods.Contains(declaration.Identifier.ValueText))
            DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, declaration, declaration.Identifier.ValueText);
    }
}