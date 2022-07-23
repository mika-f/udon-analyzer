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
public class DoesNotSupportInstantiatingNonGameObjectTypesAnalyzer : BaseDiagnosticAnalyzer
{
    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.DoesNotSupportInstantiatingNonGameObjectTypes;

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, true, AnalyzeInvocationExpressionSyntax), SyntaxKind.InvocationExpression);
    }

    private void AnalyzeInvocationExpressionSyntax(SyntaxNodeAnalysisContext context)
    {
        var expression = (InvocationExpressionSyntax)context.Node;
        var si = context.SemanticModel.GetSymbolInfo(expression);
        if (si.Symbol is not IMethodSymbol ms)
            return;

        if (ms.Name == "Instantiate" && ms.ContainingType.ToDisplayString() == "UnityEngine.Object")
        {
            if (ms.TypeArguments.Length != 1)
            {
                DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, expression);
                return;
            }

            if (ms.TypeArguments[0].ToDisplayString() != "UnityEngine.GameObject")
                DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, expression);
        }
    }
}