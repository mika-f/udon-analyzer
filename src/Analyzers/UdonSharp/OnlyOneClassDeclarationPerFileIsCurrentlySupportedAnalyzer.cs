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
using NatsunekoLaboratory.UdonAnalyzer.Extensions;
using NatsunekoLaboratory.UdonAnalyzer.Internal;

namespace NatsunekoLaboratory.UdonAnalyzer.UdonSharp;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[RequireUdonVersion("[2021.11.24.16.19,)")]
[RequireUdonSharpCompilerVersion("[0.20.3,)")]
public class OnlyOneClassDeclarationPerFileIsCurrentlySupportedAnalyzer : BaseDiagnosticAnalyzer
{
    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.OnlyOneClassDeclarationPerFileIsCurrentlySupported;

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, false, AnalyzeClassDeclaration), SyntaxKind.ClassDeclaration);
    }

    private void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
    {
        var declaration = (ClassDeclarationSyntax)context.Node;
        var declarations = SyntaxNodeHelper.EnumerateClassDeclarations(context.SemanticModel.SyntaxTree.GetRoot());
        if (declarations.Count <= 1 || declarations.None(w => w.IsInheritOf("UdonSharp.UdonSharpBehaviour", context.SemanticModel)))
            return;

        var node = declarations.Select((w, i) => (Syntax: w, Index: i)).FirstOrDefault(w => w.Syntax.IsEquivalentTo(declaration, true));
        if (node == default)
            return;

        if (node.Index > 0)
            DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, declaration);
    }
}