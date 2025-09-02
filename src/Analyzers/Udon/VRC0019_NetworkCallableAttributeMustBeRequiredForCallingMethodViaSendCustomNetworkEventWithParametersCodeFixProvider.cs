// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using NatsunekoLaboratory.UdonAnalyzer.Internal;

using SyntaxFactory = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace NatsunekoLaboratory.UdonAnalyzer.Udon;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NetworkCallableAttributeMustBeRequiredForCallingMethodViaSendCustomNetworkEventWithParametersCodeFixProvider))]
[Shared]
public class NetworkCallableAttributeMustBeRequiredForCallingMethodViaSendCustomNetworkEventWithParametersCodeFixProvider : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DiagnosticDescriptors.NetworkCallableAttributeMustBeRequiredForCallingMethodViaSendCustomNetworkEventWithParameters.Id);

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
        if (root == null)
            return;

        foreach (var diagnostic in context.Diagnostics)
        {
            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var invocation = root.FindToken(diagnosticSpan.Start).Parent?.AncestorsAndSelf().OfType<InvocationExpressionSyntax>().First();
            if (invocation != null)
            {
                var ms = await GetTargetNetworkCallingMethod(context.Document, invocation, context.CancellationToken);
                if (ms == null)
                    continue;

                context.RegisterCodeFix(CodeAction.Create("Add [NetworkCallable] to the target method", ct => RunCodeFix(context.Document, ms, ct)), diagnostic);
            }
        }
    }

    private async Task<IMethodSymbol?> GetTargetNetworkCallingMethod(Document document, InvocationExpressionSyntax invocation, CancellationToken ct)
    {
        var sm = await document.GetSemanticModelAsync(ct);
        if (invocation.Expression is IdentifierNameSyntax identifier)
        {
            if (sm?.GetDeclaredSymbol(invocation.AncestorsAndSelf().OfType<ClassDeclarationSyntax>().First()) is not INamedTypeSymbol receiver)
                return null;

            return await AnalyzeSendCustomEvent(document, identifier, invocation.ArgumentList.Arguments, receiver, ct);
        }

        if (invocation.Expression is MemberAccessExpressionSyntax ma)
        {
            if (sm?.GetTypeInfo(ma.Expression).Type is not INamedTypeSymbol receiver)
                return null;

            return await AnalyzeSendCustomEvent(document, ma.Name, invocation.ArgumentList.Arguments, receiver, ct);
        }

        return null;
    }

    private static async Task<IMethodSymbol?> AnalyzeSendCustomEvent(Document document, SimpleNameSyntax name, SeparatedSyntaxList<ArgumentSyntax> arguments, INamedTypeSymbol receiver, CancellationToken ct)
    {
        if (arguments.Count < 2)
            return null;

        var param = name.Identifier.ValueText switch
        {
            "SendCustomNetworkEvent" => arguments[1],
            _ => null
        };

        if (param == null)
            return null;

        var sm = await document.GetSemanticModelAsync(ct);
        if (sm == null)
            return null;

        var targetName = sm.GetConstantValue(param.Expression);
        if (!targetName.HasValue)
            return null;

        var candidates = receiver.GetMembers(targetName.Value as string ?? throw new InvalidOperationException()).OfType<IMethodSymbol>().ToList();
        var handler = candidates.FirstOrDefault(w => w.Parameters.Length == arguments.Count - 2);
        return handler ?? null;
    }

    private async Task<Solution> RunCodeFix(Document document, IMethodSymbol declaration, CancellationToken ct)
    {
        var solution = document.Project.Solution;
        var syntax = declaration.DeclaringSyntaxReferences.First();
        var tree = syntax.SyntaxTree;
        var workspace = solution.Workspace;
        var targetDocument = workspace.CurrentSolution.GetDocument(tree);
        if (targetDocument == null)
            return solution;

        var oldRoot = await targetDocument.GetSyntaxRootAsync(ct);
        var oldNode = (MethodDeclarationSyntax)await syntax.GetSyntaxAsync(ct);
        var newNode = oldNode!.AddAttributeLists(SyntaxFactory.AttributeList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("global::VRC.SDK3.UdonNetworkCalling.NetworkCallable")) })));
        var newRoot = oldRoot?.ReplaceNode(oldNode, newNode);
        if (newRoot == null)
            return solution;

        return solution.WithDocumentSyntaxRoot(targetDocument.Id, newRoot);
    }
}