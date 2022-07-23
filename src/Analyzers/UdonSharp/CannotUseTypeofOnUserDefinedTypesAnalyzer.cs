// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Linq;

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
public class CannotUseTypeofOnUserDefinedTypesAnalyzer : BaseDiagnosticAnalyzer
{
    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.CannotUseTypeofOnUserDefinedTypes;

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, true, AnalyzeTypeOfExpressionSyntax), SyntaxKind.TypeOfExpression);
    }

    private void AnalyzeTypeOfExpressionSyntax(SyntaxNodeAnalysisContext context)
    {
        var expression = (TypeOfExpressionSyntax)context.Node;
        var si = context.SemanticModel.GetSymbolInfo(expression.Type);
        if (si.Symbol is not INamedTypeSymbol nts)
            return;

        // all of UdonSharp scripts are provided as source files
        if (nts.Locations.All(w => w.IsInSource))
            DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, expression);
    }
}