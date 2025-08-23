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
public class MultidimensionalArraysAreNotYetSupportedAnalyzer : BaseDiagnosticAnalyzer
{
    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.MultidimensionalArraysAreNotYetSupported;

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, true, AnalyzeArrayCreationExpressionSyntax), SyntaxKind.ArrayCreationExpression);
        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, true, AnalyzeImplicitArrayCreationExpressionSyntax), SyntaxKind.ImplicitArrayCreationExpression);
    }

    private void AnalyzeArrayCreationExpressionSyntax(SyntaxNodeAnalysisContext context)
    {
        var expression = (ArrayCreationExpressionSyntax)context.Node;
        if (expression.Type.RankSpecifiers[0].Sizes.Count != 1)
            DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, expression);
    }

    private void AnalyzeImplicitArrayCreationExpressionSyntax(SyntaxNodeAnalysisContext context)
    {
        var expression = (ImplicitArrayCreationExpressionSyntax)context.Node;
        if (expression.Commas.Count != 0)
            DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, expression);
    }
}