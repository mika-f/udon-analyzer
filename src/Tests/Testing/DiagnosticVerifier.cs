// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace NatsunekoLaboratory.UdonAnalyzer.Testing;

public abstract class DiagnosticVerifier<TAnalyzer, TProject> where TAnalyzer : DiagnosticAnalyzer, new() where TProject : StandaloneProject, new()
{
    protected virtual ImmutableArray<string> FilteredDiagnosticIds { get; } = ImmutableArray<string>.Empty;

    protected virtual DiagnosticResult ExpectDiagnostic()
    {
        return new DiagnosticResult(new TAnalyzer().SupportedDiagnostics[0]);
    }

    protected async Task VerifyAnalyzerAsync(string annotatedSource)
    {
        await VerifyAnalyzerAsync(annotatedSource, new Dictionary<string, string>());
    }

    protected async Task VerifyAnalyzerAsync(string annotatedSource, Dictionary<string, string> editorconfig)
    {
        await VerifyAnalyzerAsync(annotatedSource, Enumerable.Empty<(string Filename, string Content)>(), editorconfig);
    }

    protected async Task VerifyAnalyzerAsync(string annotatedSource, IEnumerable<(string Filename, string Content)> additionalFiles)
    {
        await VerifyAnalyzerAsync(annotatedSource, additionalFiles, new Dictionary<string, string>());
    }

    protected async Task VerifyAnalyzerAsync(string annotatedSource, IEnumerable<(string Filename, string Content)> additionalFiles, Dictionary<string, string> editorconfig)
    {
        var (source, diagnostics) = ParseAnnotatedSource(annotatedSource);
        await VerifyAnalyzerAsync(source, additionalFiles, editorconfig, diagnostics);
    }

    protected async Task VerifyAnalyzerAsync(string source, IEnumerable<(string Filename, string Content)> additionalFiles, Dictionary<string, string> editorconfig, params DiagnosticResult[] expected)
    {
        var project = StandaloneProject.CreateProject<TProject>(new[] { source });

        foreach (var file in additionalFiles)
            project.AddAdditionalFile(file.Filename, file.Content);

        var sb = new StringBuilder();
        sb.AppendLine("root = true");
        sb.AppendLine();
        sb.AppendLine("[*.*]");
        foreach (var config in editorconfig)
            sb.Append(config.Key).Append(" = ").AppendLine(config.Value);

        project.AddAnalyzerConfigFile(".editorconfig", sb.ToString());

        await project.RunAnalyzerAsync<TAnalyzer>(expected, FilteredDiagnosticIds, CancellationToken.None);
    }

    protected (string Source, DiagnosticResult[] Diagnostics) ParseAnnotatedSource(string source)
    {
        var sb = new StringBuilder();
        var diagnostics = new List<DiagnosticResult>();

        var line = 1;
        var column = 1;
        var expectedLine = 0;
        var expectedColumn = 0;
        var isReadingAnnotation = false;

        using var sr = new StringReader(source);
        while (sr.Peek() > -1)
        {
            var c = (char)sr.Read();
            switch (c)
            {
                case '\n':
                    sb.Append(c);
                    line++;
                    column = 1;
                    break;

                case '[' when sr.Peek() == '|':
                    sr.Read();

                    expectedLine = line;
                    expectedColumn = column;
                    isReadingAnnotation = true;
                    break;

                case '|' when isReadingAnnotation && sr.Peek() == '@':
                    sr.Read();

                    var message = new StringBuilder();
                    while (sr.Peek() != ']')
                        message.Append((char)sr.Read());
                    sr.Read();

                    // ReSharper disable once CoVariantArrayConversion
                    diagnostics.Add(ExpectDiagnostic().WithSpan(expectedLine, expectedColumn, line, column).WithArguments(message.ToString().Split(",")));
                    isReadingAnnotation = false;
                    break;

                case '|' when isReadingAnnotation && sr.Peek() == ']':
                    sr.Read();

                    diagnostics.Add(ExpectDiagnostic().WithSpan(expectedLine, expectedColumn, line, column));
                    isReadingAnnotation = false;
                    break;

                default:
                    sb.Append(c);
                    column++;
                    break;
            }
        }

        return (Source: sb.ToString(), Diagnostics: diagnostics.ToArray());
    }
}