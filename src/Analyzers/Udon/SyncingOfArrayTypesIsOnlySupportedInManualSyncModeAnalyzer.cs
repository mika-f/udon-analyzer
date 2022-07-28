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

namespace NatsunekoLaboratory.UdonAnalyzer.Udon;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[RequireUdonVersion("[3.1.0,)")]
[RequireUdonSharpCompilerVersion("[1.0.0,)")]
public class SyncingOfArrayTypesIsOnlySupportedInManualSyncModeAnalyzer : BaseDiagnosticAnalyzer
{
    private const string UdonBehaviourSyncModeAttributeFullyQualifiedName = "UdonSharp.UdonBehaviourSyncModeAttribute";
    private const string UdonSyncedAttributeFullyQualifiedName = "UdonSharp.UdonSyncedAttribute";

    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.SyncingOfArrayTypesIsOnlySupportedInManualSyncMode;

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, false, AnalyzeFieldDeclaration), SyntaxKind.FieldDeclaration);
    }

    private void AnalyzeFieldDeclaration(SyntaxNodeAnalysisContext context)
    {
        var declaration = (FieldDeclarationSyntax)context.Node;
        var t = context.SemanticModel.GetTypeInfo(declaration.Declaration.Type);

        if (t.Type is not IArrayTypeSymbol ats)
            return;

        if (declaration.HasAttribute(UdonSyncedAttributeFullyQualifiedName, context.SemanticModel))
        {
            var cls = declaration.AncestorsAndSelf().OfType<ClassDeclarationSyntax>().First();
            if (!cls.HasAttribute(UdonBehaviourSyncModeAttributeFullyQualifiedName, context.SemanticModel))
                return;

            var attr = cls.GetAttribute(UdonBehaviourSyncModeAttributeFullyQualifiedName, context.SemanticModel);
            if (attr?.ArgumentList == null || attr.ArgumentList.Arguments.Count < 1)
                return;

            var val = context.SemanticModel.GetConstantValue(attr.ArgumentList.Arguments[0].Expression);
            if (!val.HasValue || val.Value is 4 /* Manual */)
                return;

            DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, declaration, ats.ToDisplayString());
        }
    }
}