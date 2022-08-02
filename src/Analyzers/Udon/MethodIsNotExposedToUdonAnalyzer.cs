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

        if (expression.Expression is MemberAccessExpressionSyntax expr)
        {
            var receiver = context.SemanticModel.GetSymbolInfo(expr.Expression);
            if (receiver.Symbol?.ContainingType != null)
                if (SymbolDictionary.Instance.IsSymbolIsAllowed(si.Symbol.OriginalDefinition, receiver.Symbol.ContainingType, context))
                    return;

            if (receiver.Symbol is IPropertySymbol ps)
                if (SymbolDictionary.Instance.IsSymbolIsAllowed(si.Symbol.OriginalDefinition, ps.Type, context))
                    return;

            var ti = context.SemanticModel.GetTypeInfo(expr.Expression);
            if (ti.Type?.BaseType?.Equals(context.SemanticModel.Compilation.GetTypeByMetadataName("System.Enum"), SymbolEqualityComparer.Default) == true)
                if (SymbolDictionary.Instance.IsSymbolIsAllowed(si.Symbol.OriginalDefinition, ti.Type, context))
                    return;
        }

        if (SymbolDictionary.Instance.IsSymbolIsAllowed(si.Symbol.OriginalDefinition, null, context))
            return;

        DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, expression, si.Symbol.ToDisplayString());
    }
}