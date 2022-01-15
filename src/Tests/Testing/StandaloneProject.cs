// -----------------------------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the Microsoft Reference Source License. See LICENSE in the project root for license information.
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;

using NatsunekoLaboratory.UdonAnalyzer.Testing.Extensions;

using Xunit;

namespace NatsunekoLaboratory.UdonAnalyzer.Testing;

public class StandaloneProject
{
    private const string SolutionName = "NatsunekoLaboratory.UdonAnalyzer";
    private const string AssemblyName = SolutionName;

    protected Solution? Solution { get; private set; }

    protected Project? Project { get; private set; }

    protected ReadOnlyCollection<Diagnostic> Diagnostics { get; private set; } = new List<Diagnostic>().AsReadOnly();

    public static StandaloneProject CreateProject<T>(IEnumerable<string> sources, string debugName = "StandaloneProject") where T : StandaloneProject, new()
    {
        var projectId = ProjectId.CreateNewId(debugName);
        var solution = new AdhocWorkspace().CurrentSolution.AddProject(projectId, SolutionName, AssemblyName, LanguageNames.CSharp);

        var standalone = new T();

        solution = standalone.ExternalReferences().Aggregate(solution, (current, reference) => current.AddMetadataReference(projectId, MetadataReference.CreateFromFile(reference)));
        solution = solution.WithProjectParseOptions(projectId, new CSharpParseOptions(LanguageVersion.Latest));

        standalone.Solution = solution;

        foreach (var source in sources)
        {
            var filename = $"{Guid.NewGuid()}.cs";
            var documentId = DocumentId.CreateNewId(projectId, filename);

            solution = solution.AddDocument(documentId, filename, SourceText.From(source), filePath: $"/{filename}");
        }

        standalone.Solution = solution;
        standalone.Project = standalone.Solution.GetProject(projectId);

        return standalone;
    }

    protected virtual IEnumerable<string> ExternalReferences()
    {
        return new List<string>();
    }

    public async Task RunAnalyzerAsync<TAnalyzer>(DiagnosticResult[] expectedDiagnostics, ImmutableArray<string> allowedDiagnosticIds, CancellationToken cancellationToken) where TAnalyzer : DiagnosticAnalyzer, new()
    {
        Assert.NotNull(Project);

        var compilation = await Project.GetCompilationAsync(cancellationToken);
        Assert.NotNull(compilation);

        var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, true);
        var compilationWithAnalyzers = compilation.WithOptions(compilationOptions).WithAnalyzers(ImmutableArray.Create<DiagnosticAnalyzer>(new TAnalyzer()));

        var allDiagnostics = await compilationWithAnalyzers.GetAllDiagnosticsAsync(cancellationToken);
        var allowedDiagnostics = allDiagnostics.Where(w => allowedDiagnosticIds.Contains(w.Id));
        var actualDiagnostics = new List<Diagnostic>();

        foreach (var diagnostic in allDiagnostics.Except(allowedDiagnostics).Where(w => w.Location.IsInSource).ToList())
            if (diagnostic.Location == Location.None)
            {
                actualDiagnostics.Add(diagnostic);
            }
            else
            {
                var syntax = await Project.Documents
                                          .ToAsyncEnumerable()
                                          .SelectAwait(async w => await w.GetSyntaxTreeAsync(cancellationToken))
                                          .FirstOrDefaultAsync(w => w == diagnostic.Location.SourceTree, cancellationToken);
                if (syntax != null)
                    actualDiagnostics.Add(diagnostic);
            }

        Diagnostics = actualDiagnostics.AsReadOnly();

        VerifyDiagnostics(actualDiagnostics.ToArray(), expectedDiagnostics);
    }

    public async Task RunCodeFixAsync<TCodeFix>(string fixedSource, CancellationToken cancellationToken) where TCodeFix : CodeFixProvider, new()
    {
        Assert.NotNull(Project);

        var document = Project.Documents.FirstOrDefault();
        Assert.NotNull(document);

        var trees = await document.GetSyntaxTreeAsync(cancellationToken);
        var nodes = await Diagnostics.ToAsyncEnumerable()
                                     .SelectAwait(async w => await document.FindNodeAsync(w.Location.SourceSpan, cancellationToken))
                                     .ToListAsync(cancellationToken);

        foreach (var (actualDiagnostic, i) in Diagnostics.Select((w, i) => (w, i)))
        {
            var node = await document.FindEquivalentNodeAsync(nodes[i], cancellationToken);
            Assert.NotNull(node);

            var diagnostic = Diagnostic.Create(actualDiagnostic.Descriptor, Location.Create(trees, node.Span), actualDiagnostic.Severity, actualDiagnostic.AdditionalLocations, actualDiagnostic.Properties);
            var actions = new List<CodeAction>();
            var context = new CodeFixContext(document, diagnostic, (a, _) => actions.Add(a), cancellationToken);

            await new TCodeFix().RegisterCodeFixesAsync(context);

            Assert.True(actions.Count > 0);
            document = await ApplyCodeFixAsync(document.Id, actions.First(), cancellationToken);
        }

        var text = await document.GetTextAsync(cancellationToken);
        Assert.Equal(fixedSource, text.ToString());
    }

    private static void VerifyDiagnostics(Diagnostic[] actualDiagnostics, IReadOnlyList<DiagnosticResult> expectedDiagnostics)
    {
        if (expectedDiagnostics.Count != actualDiagnostics.Length)
            AssertFailure("Mismatch between number of diagnostic returned", actualDiagnostics.Length, expectedDiagnostics.Count, actualDiagnostics);

        for (var i = 0; i < expectedDiagnostics.Count; i++)
        {
            var actual = actualDiagnostics[i];
            var expect = expectedDiagnostics[i];

            if (!expect.HasLocation)
            {
                if (actual.Location != Location.None)
                    AssertFailure("Mismatch between location attribute of diagnostic returned", "Location: Source", "Location: Project", actual);
            }
            else
            {
                VerifyDiagnosticLocations(actual.Location, expect.Spans);
            }

            if (actual.Id != expect.Id)
                AssertFailure("Mismatch between ID of diagnostic returned", actual.Id, expect.Id, actual);

            if (actual.Severity != expect.Severity)
                AssertFailure("Mismatch between severity of diagnostic returned", actual.Severity, expect.Severity, actual);

            if (actual.GetMessage() != expect.Message)
                AssertFailure("Mismatch between message of diagnostic returned", actual.GetMessage(), expect.Message ?? "(null)", actual);
        }
    }

    private static void VerifyDiagnosticLocations(Location actualLocation, IEnumerable<DiagnosticLocation> expectedLocations)
    {
        var actualSpan = actualLocation.GetLineSpan();
        var expectedSpan = expectedLocations.First();

        var actualStartLinePosition = actualSpan.StartLinePosition;
        var expectedStartLinePosition = expectedSpan.Span.StartLinePosition;

        if (actualStartLinePosition.Line > 0 && actualStartLinePosition.Line != expectedStartLinePosition.Line)
            AssertFailure("Mismatch between start line position of diagnostic returned", actualStartLinePosition.Line + 1, expectedStartLinePosition.Line + 1);
        if (actualStartLinePosition.Character > 0 && actualStartLinePosition.Character != expectedSpan.Span.StartLinePosition.Character)
            AssertFailure("Mismatch between start character position of diagnostic returned", actualStartLinePosition.Character, expectedStartLinePosition.Character);

        var actualEndLinePosition = actualSpan.EndLinePosition;
        var expectedEndLinePosition = expectedSpan.Span.EndLinePosition;

        if (actualEndLinePosition.Line > 0 && actualEndLinePosition.Line != expectedEndLinePosition.Line)
            AssertFailure("Mismatch between end line position of diagnostic returned", actualEndLinePosition.Line + 1, expectedEndLinePosition.Line + 1);
        if (actualEndLinePosition.Character > 0 && actualEndLinePosition.Character != expectedEndLinePosition.Character)
            AssertFailure("Mismatch between end character position of diagnostic returned", actualEndLinePosition.Character, expectedEndLinePosition.Character);
    }

    private static async Task<Document> ApplyCodeFixAsync(DocumentId documentId, CodeAction action, CancellationToken cancellationToken = default)
    {
        var operation = await action.GetOperationsAsync(cancellationToken);
        return operation.OfType<ApplyChangesOperation>().First().ChangedSolution.GetDocument(documentId);
    }

    private static void AssertFailure(string message, object actual, object expected, params Diagnostic[] actualDiagnostics)
    {
        var messages = new[]
        {
            $"Message       : {message}",
            $"Expected value: {expected}",
            $"Actual value  : {actual}",
            $"Diagnostics   : {(actualDiagnostics.Length > 0 ? FormatDiagnostics(actualDiagnostics) : "NONE or NOT PROVIDED")}"
        };

        Assert.True(false, string.Join(Environment.NewLine, messages));
    }

    private static string FormatDiagnostics(IEnumerable<Diagnostic> diagnostics)
    {
        var sb = new StringBuilder();

        foreach (var diagnostic in diagnostics)
        {
            var location = diagnostic.Location;
            if (location.IsInMetadata)
            {
                sb.Append(diagnostic.Id)
                  .Append(" at InMetadata with ")
                  .Append(diagnostic.GetMessage());
            }
            else
            {
                var position = location.GetLineSpan().StartLinePosition;
                sb.Append(diagnostic.Id)
                  .Append(" at ")
                  .Append(position.Line + 1)
                  .Append(':')
                  .Append(position.Character + 1)
                  .Append(" with ")
                  .Append(diagnostic.GetMessage());
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}