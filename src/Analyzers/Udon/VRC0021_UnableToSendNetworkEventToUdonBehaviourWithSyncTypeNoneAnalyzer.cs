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
public class UnableToSendNetworkEventToUdonBehaviourWithSyncTypeNoneAnalyzer : BaseDiagnosticAnalyzer
{
    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.UnableToSendNetworkEventToUdonBehaviourWithSyncTypeNone;

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
            AnalyzeReceiverMembers(context, receiver, target, invocation.ArgumentList.Arguments);
        }
        else if (invocation.Expression is MemberAccessExpressionSyntax ma)
        {
            // receiver access
            var target = AnalyzeSendCustomEvent(context, ma.Name, invocation.ArgumentList.Arguments);
            if (target == null)
                return;
            var receiver = context.SemanticModel.GetTypeInfo(ma.Expression).Type as INamedTypeSymbol;
            AnalyzeReceiverMembers(context, receiver, target, invocation.ArgumentList.Arguments);
        }
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

    private void AnalyzeReceiverMembers(SyntaxNodeAnalysisContext context, INamedTypeSymbol? receiver, string target, SeparatedSyntaxList<ArgumentSyntax> argumentSyntaxList)
    {
        if (receiver == null)
            return;

        var candidates = receiver.GetMembers(target).OfType<IMethodSymbol>().ToList();
        var handler = candidates.FirstOrDefault(w => w.Parameters.Length == argumentSyntaxList.Count - 2);
        if (handler == null)
            return;

        var @class = handler.ContainingType;
        var attr = @class.GetAttributes().FirstOrDefault(w => w.AttributeClass?.ToDisplayString() == "UdonSharp.UdonBehaviourSyncModeAttribute");
        if (attr == null)
            return;

        var param = attr.ConstructorArguments.FirstOrDefault();
        if ((int)(param.Value ?? 0) == 1) 
            DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, context.Node, receiver.ToDisplayString());
    }
}