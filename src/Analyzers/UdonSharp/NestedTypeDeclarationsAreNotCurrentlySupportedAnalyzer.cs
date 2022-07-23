// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

using NatsunekoLaboratory.UdonAnalyzer.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.Internal;

namespace NatsunekoLaboratory.UdonAnalyzer.UdonSharp;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[RequireUdonVersion("[3.1.0,)")]
[RequireUdonSharpCompilerVersion("[1.0.0,)")]
public class NestedTypeDeclarationsAreNotCurrentlySupportedAnalyzer : BaseDiagnosticAnalyzer
{
    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.NestedTypeDeclarationsAreNotCurrentlySupported;

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, true, AnalyzeTypeDeclarationSyntax), SyntaxKind.ClassDeclaration, SyntaxKind.StructDeclaration, SyntaxKind.RecordDeclaration, SyntaxKind.InterfaceDeclaration);
    }

    private void AnalyzeTypeDeclarationSyntax(SyntaxNodeAnalysisContext context)
    {
        var s = context.SemanticModel.GetDeclaredSymbol(context.Node);
        if (s is not INamedTypeSymbol nts)
            return;

        if (nts.ContainingType != null)
            DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, context.Node);
    }
}