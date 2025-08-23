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
public class CannotSyncVariableBecauseBehaviourIsSetToNoVariableSyncAnalyzer : BaseDiagnosticAnalyzer
{
    private const string UdonBehaviourSyncModeAttributeFullyQualifiedName = "UdonSharp.UdonBehaviourSyncModeAttribute";
    private const string UdonSyncedAttributeFullyQualifiedName = "UdonSharp.UdonSyncedAttribute";

    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.CannotSyncVariableBecauseBehaviourIsSetToNoVariableSync;

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, false, AnalyzeClassDeclaration), SyntaxKind.ClassDeclaration);
    }

    private void AnalyzeClassDeclaration(SyntaxNodeAnalysisContext context)
    {
        var declaration = (ClassDeclarationSyntax)context.Node;
        if (declaration.HasAttribute(UdonBehaviourSyncModeAttributeFullyQualifiedName, context.SemanticModel))
        {
            var attr = declaration.GetAttribute(UdonBehaviourSyncModeAttributeFullyQualifiedName, context.SemanticModel);
            if (attr == null || attr.ArgumentList?.Arguments.Count < 1)
                return;

            var val = context.SemanticModel.GetConstantValue(attr.ArgumentList!.Arguments[0].Expression);
            if (!val.HasValue || val.Value is not 2 /* NoVariableSync */)
                return;

            var members = declaration.DescendantNodes().OfType<FieldDeclarationSyntax>();
            if (members.Any(w => w.HasAttribute(UdonSyncedAttributeFullyQualifiedName, context.SemanticModel)))
                DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, attr);
        }
    }
}