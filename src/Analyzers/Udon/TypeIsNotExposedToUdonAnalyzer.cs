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
public class TypeIsNotExposedToUdonAnalyzer : BaseDiagnosticAnalyzer
{
    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.TypeIsNotExposedToUdon;

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, false, AnalyzeMethodDeclaration), SyntaxKind.MethodDeclaration);
        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, false, AnalyzeVariableDeclaration), SyntaxKind.VariableDeclaration);
        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, false, AnalyzePropertyDeclaration), SyntaxKind.PropertyDeclaration);
    }

    private void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
    {
        var declaration = (MethodDeclarationSyntax)context.Node;

        CheckReturnType(context, declaration);
        CheckParameterType(context, declaration);
    }

    private void CheckReturnType(SyntaxNodeAnalysisContext context, MethodDeclarationSyntax declaration)
    {
        var symbol = context.SemanticModel.GetTypeInfo(declaration.ReturnType);
        if (symbol.Type == null)
            return;

        if (SymbolDictionary.Instance.IsSymbolIsAllowed(symbol.Type, context))
            return;

        DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, declaration.ReturnType, symbol.Type.ToDisplayString());
    }

    private void CheckParameterType(SyntaxNodeAnalysisContext context, MethodDeclarationSyntax declaration)
    {
        foreach (var parameter in declaration.ParameterList.Parameters)
        {
            if (parameter.Type == null)
                continue;

            var symbol = context.SemanticModel.GetTypeInfo(parameter.Type);
            if (symbol.Type == null)
                continue;

            if (SymbolDictionary.Instance.IsSymbolIsAllowed(symbol.Type, context))
                continue;

            DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, parameter.Type, symbol.Type.ToDisplayString());
        }
    }

    private void AnalyzeVariableDeclaration(SyntaxNodeAnalysisContext context)
    {
        var declaration = (VariableDeclarationSyntax)context.Node;
        var symbol = context.SemanticModel.GetTypeInfo(declaration.Type);
        if (symbol.Type == null)
            return;

        if (SymbolDictionary.Instance.IsSymbolIsAllowed(symbol.Type, context))
            return;

        DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, declaration.Type, symbol.Type.ToDisplayString());
    }

    private void AnalyzePropertyDeclaration(SyntaxNodeAnalysisContext context)
    {
        var declaration = (PropertyDeclarationSyntax)context.Node;
        var symbol = context.SemanticModel.GetTypeInfo(declaration.Type);
        if (symbol.Type == null)
            return;

        if (SymbolDictionary.Instance.IsSymbolIsAllowed(symbol.Type, context))
            return;

        DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, declaration.Type, symbol.Type.ToDisplayString());
    }
}