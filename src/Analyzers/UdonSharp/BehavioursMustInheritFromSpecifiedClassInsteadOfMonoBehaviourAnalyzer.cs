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
public class BehavioursMustInheritFromSpecifiedClassInsteadOfMonoBehaviourAnalyzer : BaseDiagnosticAnalyzer
{
    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.BehavioursMustInheritFromSpecifiedClassInsteadOfMonoBehaviour;

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, AnalyzeBaseList), SyntaxKind.BaseList);
    }

    private static void AnalyzeBaseList(SyntaxNodeAnalysisContext context)
    {
        var bases = (BaseListSyntax)context.Node;
        var inherit = context.GetEditorConfigValue(AnalyzerOptionDescriptors.BehaviourInheritFrom)!;
        var symbol = context.SemanticModel.Compilation.GetTypeByMetadataName(inherit);
        if (symbol == null)
            return;

        var inheritance = bases.Types.Where(w => !w.IsClassOf(symbol, context.SemanticModel)).ToList();
        if (inheritance.Count > 0)
            DiagnosticHelper.ReportDiagnostic(context, DiagnosticDescriptors.BehavioursMustInheritFromSpecifiedClassInsteadOfMonoBehaviour, inheritance[0]);
    }
}