//  ------------------------------------------------------------------------------------------
//   Copyright (c) Natsuneko. All rights reserved.
//   Licensed under the MIT License. See LICENSE in the project root for license information.
//  ------------------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

using NatsunekoLaboratory.UdonAnalyzer.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.Extensions;
using NatsunekoLaboratory.UdonAnalyzer.Internal;

namespace NatsunekoLaboratory.UdonAnalyzer.UdonSharp
{
    [DiagnosticAnalyzer(LanguageNames.CSharp), RequireUdonVersion("[2021.11.24.16.19,)"), RequireUdonSharpCompilerVersion("[0.20.3,)")]
    public class DoesNotCurrentlySupportStaticUserDefinedPropertiesAnalyzer : BaseDiagnosticAnalyzer
    {
        public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.DoesNotCurrentlySupportStaticUserDefinedProperties;
        public override void Initialize(AnalysisContext context)
        {
            base.Initialize(context);

            context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, true, AnalyzePropertyDeclaration), SyntaxKind.PropertyDeclaration);
        }

        private void AnalyzePropertyDeclaration(SyntaxNodeAnalysisContext context)
        {
            var declaration = (PropertyDeclarationSyntax)context.Node;
            if (declaration.HasModifiers(SyntaxKind.StaticKeyword))
                DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, declaration);
        }
    }
}