// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

using NatsunekoLaboratory.UdonAnalyzer.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.Extensions;
using NatsunekoLaboratory.UdonAnalyzer.Internal;
using NatsunekoLaboratory.UdonAnalyzer.Models;

namespace NatsunekoLaboratory.UdonAnalyzer.Udon;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[RequireUdonVersion("[3.1.0,)")]
[RequireUdonSharpCompilerVersion("[1.0.0,)")]
public class DoesNotCurrentlySupportSyncingOfTheTypeAnalyzer : BaseDiagnosticAnalyzer
{
    private const string UdonSyncedAttributeFullyQualifiedName = "UdonSharp.UdonSyncedAttribute";
    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.DoesNotCurrentlySupportSyncingOfTheType;

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
            var symbol = context.SemanticModel.GetTypeInfo(declaration.Declaration.Type);
            if (symbol.Type == null)
                return;

            if (SymbolDictionary.Instance.IsSymbolCanSync(symbol.Type))
                return;

            DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, declaration);
        }
    }
}