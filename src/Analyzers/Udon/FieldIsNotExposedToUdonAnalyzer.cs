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
using NatsunekoLaboratory.UdonAnalyzer.Models;

namespace NatsunekoLaboratory.UdonAnalyzer.Udon;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[RequireUdonVersion("[3.1.0,)")]
[RequireUdonSharpCompilerVersion("[1.0.0,)")]
public class FieldIsNotExposedToUdonAnalyzer : BaseDiagnosticAnalyzer
{
    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.FieldIsNotExposedToUdon;

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, false, AnalyzeMemberAccessExpression), SyntaxKind.SimpleMemberAccessExpression);
    }

    private void AnalyzeMemberAccessExpression(SyntaxNodeAnalysisContext context)
    {
        var expression = (MemberAccessExpressionSyntax)context.Node;
        var isAssignment = expression.Parent is AssignmentExpressionSyntax assignment && assignment.Right != expression;
        var si = context.SemanticModel.GetSymbolInfo(expression);
        if (si.Symbol == null)
            return;

        var t = context.SemanticModel.GetTypeInfo(expression.Expression);
        if (SymbolDictionary.Instance.IsSymbolIsAllowed(si.Symbol, t.Type, !isAssignment, context))
            return;

        DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, expression, si.Symbol.ToDisplayString());
    }
}