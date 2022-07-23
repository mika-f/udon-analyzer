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
public class DoesNotYetSupportInitializerListsAnalyzer : BaseDiagnosticAnalyzer
{
    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.DoesNotYetSupportInitializerLists;

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, true, AnalyzeObjectCreationExpressionSyntax), SyntaxKind.ObjectCreationExpression);
    }

    private void AnalyzeObjectCreationExpressionSyntax(SyntaxNodeAnalysisContext context)
    {
        var expression = (ObjectCreationExpressionSyntax)context.Node;
        if (expression.Initializer != null)
            DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, expression.Initializer);
    }
}