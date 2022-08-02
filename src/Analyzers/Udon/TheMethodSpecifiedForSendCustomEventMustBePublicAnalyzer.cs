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
using NatsunekoLaboratory.UdonAnalyzer.Internal;

namespace NatsunekoLaboratory.UdonAnalyzer.Udon;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
[RequireUdonVersion("[3.1.0,)")]
[RequireUdonSharpCompilerVersion("[1.0.0,)")]
public class TheMethodSpecifiedForSendCustomEventMustBePublicAnalyzer : BaseDiagnosticAnalyzer
{
    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.TheMethodSpecifiedForSendCustomEventMustBePublic;

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, false, AnalyzeInvocationExpression), SyntaxKind.InvocationExpression);
    }

    private void AnalyzeInvocationExpression(SyntaxNodeAnalysisContext context)
    {
        var invocation = (InvocationExpressionSyntax)context.Node;
        if (invocation.Expression is IdentifierNameSyntax identifier)
        {
            // directly access
            var target = AnalyzeSendCustomEvent(context, identifier, invocation.ArgumentList.Arguments);
            if (target == null)
                return;

            var receiver = context.SemanticModel.GetDeclaredSymbol(invocation.AncestorsAndSelf().OfType<ClassDeclarationSyntax>().First());
            if (AnalyzeReceiverMembers(receiver, target))
                return;
        }
        else if (invocation.Expression is MemberAccessExpressionSyntax ma)
        {
            // receiver access
            var target = AnalyzeSendCustomEvent(context, ma.Name, invocation.ArgumentList.Arguments);
            if (target == null)
                return;

            var receiver = context.SemanticModel.GetTypeInfo(ma.Expression).Type as INamedTypeSymbol;
            if (AnalyzeReceiverMembers(receiver, target))
                return;
        }

        DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, invocation);
    }

    private static string? AnalyzeSendCustomEvent(SyntaxNodeAnalysisContext context, SimpleNameSyntax name, SeparatedSyntaxList<ArgumentSyntax> arguments)
    {
        var param = name.Identifier.ValueText switch
        {
            "SendCustomEvent" => arguments[0],
            "SendCustomEventDelayedFrames" => arguments[0],
            "SendCustomEventDelayedSeconds" => arguments[0],
            "SendCustomNetworkEvent" => arguments[1],
            _ => null
        };

        if (param == null)
            return null;

        var value = context.SemanticModel.GetConstantValue(param.Expression);
        return value.HasValue ? value.Value as string : null;
    }

    private static bool AnalyzeReceiverMembers(INamedTypeSymbol? symbol, string target)
    {
        if (symbol == null)
            return true;

        var candidates = symbol.GetMembers(target).OfType<IMethodSymbol>().ToList();
        if (candidates.Count != 1)
            return true;

        return candidates[0].DeclaredAccessibility == Accessibility.Public;
    }
}