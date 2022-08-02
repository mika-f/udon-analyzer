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

namespace NatsunekoLaboratory.UdonAnalyzer.UdonSharp;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[RequireUdonVersion("[3.1.0,)")]
[RequireUdonSharpCompilerVersion("[1.0.0,)")]
public class StaticFieldsAreNotYetSupportedOnUserDefinedTypesAnalyzer : BaseDiagnosticAnalyzer
{
    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.StaticFieldsAreNotYetSupportedOnUserDefinedTypes;

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, true, AnalyzeFieldDeclaration), SyntaxKind.FieldDeclaration);
        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, true, AnalyzePropertyDeclaration), SyntaxKind.PropertyDeclaration);
    }

    private void AnalyzeFieldDeclaration(SyntaxNodeAnalysisContext context)
    {
        var declaration = (FieldDeclarationSyntax)context.Node;
        if (declaration.Modifiers.Any(SyntaxKind.StaticKeyword))
            DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, declaration);
    }

    private void AnalyzePropertyDeclaration(SyntaxNodeAnalysisContext context)
    {
        var declaration = (PropertyDeclarationSyntax)context.Node;
        if (declaration.Modifiers.Any(SyntaxKind.StaticKeyword))
            DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, declaration);
    }
}