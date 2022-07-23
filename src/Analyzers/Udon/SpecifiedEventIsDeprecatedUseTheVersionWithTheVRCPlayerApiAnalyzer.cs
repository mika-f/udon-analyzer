// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

using NatsunekoLaboratory.UdonAnalyzer.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.Internal;

namespace NatsunekoLaboratory.UdonAnalyzer.Udon;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[RequireUdonVersion("[3.1.0,)")]
[RequireUdonSharpCompilerVersion("[1.0.0,)")]
// ReSharper disable once InconsistentNaming
public class SpecifiedEventIsDeprecatedUseTheVersionWithTheVRCPlayerApiAnalyzer : BaseDiagnosticAnalyzer
{
    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.SpecifiedEventIsDeprecatedUseTheVersionWithTheVRCPlayerApi;

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, true, AnalyzeMethodDeclaration), SyntaxKind.MethodDeclaration);
    }

    private void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
    {
        var declaration = (MethodDeclarationSyntax)context.Node;
        var symbol = context.SemanticModel.GetDeclaredSymbol(declaration);
        if (symbol?.Name is "OnStationEntered" or "OnStationExited" or "OnOwnershipTransferred" && symbol.Parameters.Length == 0)
            DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, declaration, symbol.Name);
    }
}