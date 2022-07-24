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
public class MethodIsNotExposedToUdonAnalyzer : BaseDiagnosticAnalyzer
{
    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.MethodIsNotExposedToUdon;

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, false, AnalyzeInvocationExpression), SyntaxKind.InvocationExpression);
    }

    private void AnalyzeInvocationExpression(SyntaxNodeAnalysisContext context)
    {
        var expression = (InvocationExpressionSyntax)context.Node;
        var si = context.SemanticModel.GetSymbolInfo(expression);
        if (si.Symbol == null)
            return;

        if (SymbolDictionary.Instance.IsSymbolIsAllowed(si.Symbol.OriginalDefinition, context))
            return;

        DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, expression, si.Symbol.ToDisplayString());
    }
}