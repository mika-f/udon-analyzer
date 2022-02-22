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
[RequireUdonVersion("[2021.11.24.16.19,)")]
[RequireUdonSharpCompilerVersion("[0.20.3,)")]
public class DoesNotSupportThrowingExceptionsAnalyzer : BaseDiagnosticAnalyzer
{
    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.DoesNotSupportThrowingExceptions;

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, AnalyzeThrowExpression), SyntaxKind.ThrowExpression);
        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, AnalyzeThrowStatement), SyntaxKind.ThrowStatement);
    }

    private void AnalyzeThrowExpression(SyntaxNodeAnalysisContext context)
    {
        var throwExpression = (ThrowExpressionSyntax)context.Node;

        DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, throwExpression);
    }

    private void AnalyzeThrowStatement(SyntaxNodeAnalysisContext context)
    {
        var throwStatement = (ThrowStatementSyntax)context.Node;

        DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, throwStatement);
    }
}