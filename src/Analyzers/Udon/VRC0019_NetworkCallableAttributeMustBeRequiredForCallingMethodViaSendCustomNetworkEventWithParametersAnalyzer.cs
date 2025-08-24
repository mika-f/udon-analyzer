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
[RequireUdonVersion("[3.8.1,)")]
[RequireUdonSharpCompilerVersion("[1.0.0,)")]
public class NetworkCallableAttributeMustBeRequiredForCallingMethodViaSendCustomNetworkEventWithParametersAnalyzer : BaseDiagnosticAnalyzer
{
    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.NetworkCallableAttributeMustBeRequiredForCallingMethodViaSendCustomNetworkEventWithParameters;

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
            if (AnalyzeReceiverMembers(context, receiver, target, invocation.ArgumentList.Arguments.Count)) 
                return;
        }
        else if (invocation.Expression is MemberAccessExpressionSyntax ma)
        {
            // receiver access
            var target = AnalyzeSendCustomEvent(context, ma.Name, invocation.ArgumentList.Arguments);
            if (target == null)
                return;

            var receiver = context.SemanticModel.GetTypeInfo(ma.Expression).Type as INamedTypeSymbol;
            if (AnalyzeReceiverMembers(context, receiver, target, invocation.ArgumentList.Arguments.Count))
                return;
        }

        DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, invocation);
    }

    private string? AnalyzeSendCustomEvent(SyntaxNodeAnalysisContext context, SimpleNameSyntax name, SeparatedSyntaxList<ArgumentSyntax> arguments)
    {
        var param = name.Identifier.ValueText switch
        {
            "SendCustomNetworkEvent" => arguments[1],
            _ => null
        };

        if (param == null)
            return null;

        var value = context.SemanticModel.GetConstantValue(param.Expression);
        return value.HasValue ? value.Value as string : null;
    }

    private bool AnalyzeReceiverMembers(SyntaxNodeAnalysisContext context, INamedTypeSymbol? receiver, string target, int len)
    {
        if (receiver == null)
            return true;

        var candidates = receiver.GetMembers(target).OfType<IMethodSymbol>().ToList();
        var handler = candidates.FirstOrDefault(w => w.Parameters.Length == len - 2);
        if (handler == null)
            return true;
        
        if (len - 2 == 0)
            return true; // No parameters, no need to check for NetworkCallable attribute

        var attrs = handler.GetAttributes().Where(w => w.AttributeClass?.ToDisplayString() == "VRC.SDK3.UdonNetworkCalling.NetworkCallableAttribute").ToList();
        if (attrs.Count == 0)
            return false;

        return true;
    }
}