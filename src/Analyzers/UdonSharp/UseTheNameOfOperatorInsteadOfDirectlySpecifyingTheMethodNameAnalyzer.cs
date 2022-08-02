// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

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
public class UseTheNameOfOperatorInsteadOfDirectlySpecifyingTheMethodNameAnalyzer : BaseDiagnosticAnalyzer
{
    public override DiagnosticDescriptor SupportedDiagnostic => DiagnosticDescriptors.UseTheNameOfOperatorInsteadOfDirectlySpecifyingTheMethodName;

    public override void Initialize(AnalysisContext context)
    {
        base.Initialize(context);

        context.RegisterSyntaxNodeAction(w => RunAnalyzer(w, false, AnalyzeInvocationExpression), SyntaxKind.InvocationExpression);
    }

    private void AnalyzeInvocationExpression(SyntaxNodeAnalysisContext context)
    {
        var expression = (InvocationExpressionSyntax)context.Node;

        if (expression.Expression is MemberAccessExpressionSyntax ma)
            AnalyzeNameSyntax(context, ma.Name, expression.ArgumentList.Arguments);
        else if (expression.Expression is IdentifierNameSyntax identifier)
            AnalyzeNameSyntax(context, identifier, expression.ArgumentList.Arguments);
    }

    private void AnalyzeNameSyntax(SyntaxNodeAnalysisContext context, SimpleNameSyntax name, SeparatedSyntaxList<ArgumentSyntax> arguments)
    {
        switch (name.Identifier.ValueText)
        {
            case "SendCustomEvent":
            {
                var param = arguments[0];
                if (param.Expression is LiteralExpressionSyntax literal && literal.Kind() == SyntaxKind.StringLiteralExpression)
                    DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, param);
                break;
            }

            case "SendCustomEventDelayedFrames":
            {
                var param = arguments[0];
                if (param.Expression is LiteralExpressionSyntax literal && literal.Kind() == SyntaxKind.StringLiteralExpression)
                    DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, param);
                break;
            }

            case "SendCustomEventDelayedSeconds":
            {
                var param = arguments[0];
                if (param.Expression is LiteralExpressionSyntax literal && literal.Kind() == SyntaxKind.StringLiteralExpression)
                    DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, param);
                break;
            }

            case "SendCustomNetworkEvent":
            {
                var param = arguments[1];
                if (param.Expression is LiteralExpressionSyntax literal && literal.Kind() == SyntaxKind.StringLiteralExpression)
                    DiagnosticHelper.ReportDiagnostic(context, SupportedDiagnostic, param);
                break;
            }
        }
    }
}