// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.IO;

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
public class UdonSharpBehaviourClassesMustBeSameNameAsCsharpFileAnalyzer : BaseDiagnosticAnalyzer
{
    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.UdonSharpBehaviourClassesMustBeSameNameAsCsharpFile;

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, true, AnalyzeClassDeclarationSyntax), SyntaxKind.ClassDeclaration);
    }

    public void AnalyzeClassDeclarationSyntax(SyntaxNodeAnalysisContext context)
    {
        var classDecl = (ClassDeclarationSyntax)context.Node;
        if (classDecl.Modifiers.Any(SyntaxKind.AbstractKeyword))
            return;

        var tree = context.SemanticModel.SyntaxTree.FilePath;
        if (string.IsNullOrWhiteSpace(tree))
            return;

        if (classDecl.Identifier.Text != Path.GetFileNameWithoutExtension(tree))
            DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, classDecl);
    }
}