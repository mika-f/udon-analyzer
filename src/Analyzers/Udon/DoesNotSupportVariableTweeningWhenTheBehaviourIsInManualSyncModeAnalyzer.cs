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
public class DoesNotSupportVariableTweeningWhenTheBehaviourIsInManualSyncModeAnalyzer : BaseDiagnosticAnalyzer
{
    private const string UdonBehaviourSyncModeAttributeFullyQualifiedName = "UdonSharp.UdonBehaviourSyncModeAttribute";
    private const string UdonSyncedAttributeFullyQualifiedName = "UdonSharp.UdonSyncedAttribute";

    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.DoesNotSupportVariableTweeningWhenTheBehaviourIsInManualSyncMode;

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, false, AnalyzeFieldDeclaration), SyntaxKind.FieldDeclaration);
    }

    private void AnalyzeFieldDeclaration(SyntaxNodeAnalysisContext context)
    {
        var declaration = (FieldDeclarationSyntax)context.Node;
        if (declaration.HasAttribute(UdonSyncedAttributeFullyQualifiedName, context.SemanticModel))
        {
            var attr = declaration.GetAttribute(UdonSyncedAttributeFullyQualifiedName, context.SemanticModel);
            if (attr == null || attr.ArgumentList?.Arguments.Count < 1)
                return;

            // UdonSynced(default) == UdonSyncMode.None
            if (attr.ArgumentList == null)
                return;

            var val = context.SemanticModel.GetConstantValue(attr.ArgumentList.Arguments[0].Expression);
            if (!val.HasValue || val.Value is not 2 /* Linear */ and not 3 /* Smooth */)
                return;

            var cls = declaration.AncestorsAndSelf().OfType<ClassDeclarationSyntax>().First();
            if (!cls.HasAttribute(UdonBehaviourSyncModeAttributeFullyQualifiedName, context.SemanticModel))
                return;

            var attr2 = cls.GetAttribute(UdonBehaviourSyncModeAttributeFullyQualifiedName, context.SemanticModel);
            if (attr2 == null || attr2.ArgumentList?.Arguments.Count < 1)
                return;

            var mode = context.SemanticModel.GetConstantValue(attr2.ArgumentList!.Arguments[0].Expression);
            if (!mode.HasValue || mode.Value is not 4 /* Manual */)
                return;

            DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, declaration);
        }
    }
}