// -----------------------------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the Microsoft Reference Source License. See LICENSE in the project root for license information.
// -----------------------------------------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace NatsunekoLaboratory.UdonAnalyzer.Testing;

public abstract class CodeFixVerifier<TAnalyzer, TCodeFix, TProject> : DiagnosticVerifier<TAnalyzer, TProject> where TAnalyzer : DiagnosticAnalyzer, new() where TCodeFix : CodeFixProvider, new() where TProject : StandaloneProject, new()
{
    protected async Task VerifyCodeFixAsync(string oldSourceWithAnnotation, string newSource)
    {
        var (oldSource, diagnostics) = ParseAnnotatedSource(oldSourceWithAnnotation);
        await VerifyCodeFixAsync(oldSource, newSource, diagnostics);
    }

    protected async Task VerifyCodeFixAsync(string oldSource, string newSource, params DiagnosticResult[] expected)
    {
        var project = StandaloneProject.CreateProject<TProject>(new[] { oldSource });

        await project.RunAnalyzerAsync<TAnalyzer>(expected, FilteredDiagnosticIds, CancellationToken.None);
        await project.RunCodeFixAsync<TCodeFix>(newSource, CancellationToken.None);
    }
}