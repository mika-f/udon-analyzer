// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

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
public class DoesNotYetSupportHidingBaseMethodsAnalyzer : BaseDiagnosticAnalyzer
{
    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.DoesNotYetSupportHidingBaseMethods;

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, false, AnalyzeMethodDeclaration), SyntaxKind.MethodDeclaration);
    }

    private void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
    {
        var declaration = (MethodDeclarationSyntax)context.Node;
        var symbol = context.SemanticModel.GetDeclaredSymbol(declaration);
        if (symbol == null)
            return;

        var cls = context.SemanticModel.GetDeclaredSymbol(declaration.AncestorsAndSelf().OfType<ClassDeclarationSyntax>().First());
        if (cls == null)
            return;

        var members = GetAllInheritTypes(cls).SelectMany(w => w.GetMembers()).ToList();
        if (members.OfType<IMethodSymbol>().Count(w => w.Name == symbol.Name && EqualityOverload(w.Parameters, symbol.Parameters)) >= 2)
            DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, declaration, symbol.Name);
    }

    private static IEnumerable<INamedTypeSymbol> GetAllInheritTypes(INamedTypeSymbol s)
    {
        var current = s;

        while (current != null)
        {
            yield return current;
            current = current.BaseType;
        }
    }

    private static bool EqualityOverload(ImmutableArray<IParameterSymbol> parametersA, ImmutableArray<IParameterSymbol> parametersB)
    {
        if (parametersA.Length != parametersB.Length)
            return false;

        for (var i = 0; i < parametersA.Length; i++)
        {
            var argA = parametersA[i];
            var argB = parametersB[i];

            if (argA.Type.Equals(argB.Type, SymbolEqualityComparer.Default))
                continue;

            return false;
        }

        return true;
    }
}