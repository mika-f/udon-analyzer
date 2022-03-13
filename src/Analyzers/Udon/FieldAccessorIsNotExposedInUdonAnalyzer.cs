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
[RequireUdonVersion("[2021.11.24.16.19,)")]
[RequireUdonSharpCompilerVersion("[0.20.3,)")]
public class FieldAccessorIsNotExposedInUdonAnalyzer : BaseDiagnosticAnalyzer
{
    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.FieldAccessorIsNotExposedInUdon;

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, true, AnalyzeMemberAccessExpression), SyntaxKind.SimpleMemberAccessExpression);
        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, true, AnalyzeIdentifierName), SyntaxKind.IdentifierName);
    }

    private void AnalyzeMemberAccessExpression(SyntaxNodeAnalysisContext context)
    {
        var expression = (MemberAccessExpressionSyntax)context.Node;
        var info = context.SemanticModel.GetSymbolInfo(expression);
        if (info.Symbol == null)
            return;

        if (!SymbolDictionary.Instance.IsSymbolIsAllowed(info.Symbol.OriginalDefinition, context))
            DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, expression);
    }

    private void AnalyzeIdentifierName(SyntaxNodeAnalysisContext context)
    {
        var identifier = (IdentifierNameSyntax)context.Node;
        var info = context.SemanticModel.GetSymbolInfo(identifier);
        if (info.Symbol is not IFieldSymbol or IPropertySymbol)
            return;

        if (!SymbolDictionary.Instance.IsSymbolIsAllowed(info.Symbol.OriginalDefinition, context))
            DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, identifier);
    }
}