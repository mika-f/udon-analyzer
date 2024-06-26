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
[RequireUdonVersion("[3.1.0,)")]
[RequireUdonSharpCompilerVersion("[1.0.0,)")]
public class DuplicateFieldChangeCallbackTargetAnalyzer : BaseDiagnosticAnalyzer
{
    private const string FieldChangeCallbackAttributeFullyQualifiedName = "UdonSharp.FieldChangeCallbackAttribute";

    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.DuplicateFieldChangeCallbackTarget;

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, false, AnalyzeFieldDeclaration), SyntaxKind.FieldDeclaration);
    }

    private void AnalyzeFieldDeclaration(SyntaxNodeAnalysisContext context)
    {
        var declaration = (FieldDeclarationSyntax)context.Node;
        if (!declaration.HasAttribute(FieldChangeCallbackAttributeFullyQualifiedName, context.SemanticModel))
            return;

        if (declaration.Ancestors().FirstOrDefault(w => w is ClassDeclarationSyntax) is not ClassDeclarationSyntax cls)
            return;

        var targetProperty = GetTargetPropertyNameFromSyntax(context, declaration);
        if (string.IsNullOrWhiteSpace(targetProperty))
            return;

        var attr = declaration.GetAttributes(FieldChangeCallbackAttributeFullyQualifiedName, context.SemanticModel).First();
        if (declaration.Declaration.Variables.Count > 1)
        {
            DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, attr);
            return;
        }

        var fields = cls.Members.OfType<FieldDeclarationSyntax>();
        foreach (var field in fields.Where(w => w.HasAttribute(FieldChangeCallbackAttributeFullyQualifiedName, context.SemanticModel)))
        {
            var target = GetTargetPropertyNameFromSyntax(context, field);
            if (target == targetProperty && !declaration.IsEquivalentTo(field))
            {
                DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, attr);
                break;
            }
        }
    }

    private static string? GetTargetPropertyNameFromSyntax(SyntaxNodeAnalysisContext context, FieldDeclarationSyntax field)
    {
        var attr = field.GetAttributes(FieldChangeCallbackAttributeFullyQualifiedName, context.SemanticModel).FirstOrDefault();
        if (attr is not { ArgumentList.Arguments.Count: > 0 })
            return null;

        var value = context.SemanticModel.GetConstantValue(attr.ArgumentList.Arguments.First().Expression);
        return value.HasValue ? value.Value as string : null;
    }
}