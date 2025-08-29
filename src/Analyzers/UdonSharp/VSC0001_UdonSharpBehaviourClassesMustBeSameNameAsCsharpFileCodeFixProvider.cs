// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;

using NatsunekoLaboratory.UdonAnalyzer.Internal;

namespace NatsunekoLaboratory.UdonAnalyzer.UdonSharp;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(UdonSharpBehaviourClassesMustBeSameNameAsCsharpFileCodeFixProvider))]
[Shared]
public class UdonSharpBehaviourClassesMustBeSameNameAsCsharpFileCodeFixProvider : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DiagnosticDescriptors.UdonSharpBehaviourClassesMustBeSameNameAsCsharpFile.Id);

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
        if (root == null)
            return;

        foreach (var diagnostic in context.Diagnostics)
        {
            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var declaration = root.FindToken(diagnosticSpan.Start).Parent?.AncestorsAndSelf().OfType<ClassDeclarationSyntax>().First();
            if (declaration != null)
            {
                var oldName = declaration.Identifier.ToFullString().Trim();
                var newName = context.Document.Name.Replace(".cs", "");
                context.RegisterCodeFix(CodeAction.Create($"Rename '{oldName}' to '{newName}'", w => RunCodeFix(context.Document, root, declaration, w)), diagnostic);
            }
        }
    }

    public override FixAllProvider? GetFixAllProvider()
    {
        return WellKnownFixAllProviders.BatchFixer;
    }

    private static async Task<Solution> RunCodeFix(Document document, SyntaxNode root, ClassDeclarationSyntax @class, CancellationToken ct)
    {
        var solution = document.Project.Solution;
        var sm = await document.GetSemanticModelAsync(ct);
        var symbol = sm.GetDeclaredSymbol(@class);
        if (symbol == null)
            return solution;

        var filename = document.Name.Replace(".cs", "");
        return await Renamer.RenameSymbolAsync(solution, symbol, new SymbolRenameOptions(), filename, ct);
    }
}