// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

using NatsunekoLaboratory.UdonAnalyzer.Extensions;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.CSharp.Workspace;

internal sealed class UdonAnalyzerSolution
{
    private readonly Solution _solution;

    static UdonAnalyzerSolution()
    {
        MSBuildLocator.RegisterDefaults();
    }

    private UdonAnalyzerSolution(Solution solution)
    {
        _solution = solution;
    }

    public Project? GetProject(UdonAnalyzerProject project)
    {
        var b = TryGetProject(project, out var r);
        return b ? r : null;
    }

    public bool TryGetProject(UdonAnalyzerProject project, [NotNullWhen(true)] out Project? result)
    {
        var r = _solution.Projects.Where(w => w.FilePath?.EndsWith(project.CsProjName) == true).ToList();
        result = r.FirstOrDefault();

        return result != null;
    }

    public static async Task<UdonAnalyzerSolution> CreateFromPathAsync(string path)
    {
        var workspace = MSBuildWorkspace.Create();
        workspace.WorkspaceFailed += (_, args) =>
        {
            switch (args.Diagnostic.Kind)
            {
                case WorkspaceDiagnosticKind.Failure:
                    Console.Error.WriteLine(args.Diagnostic.Message);
                    break;

                case WorkspaceDiagnosticKind.Warning:
                    Console.WriteLine(args.Diagnostic.Message);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        };

        var solution = await workspace.OpenSolutionAsync(path).Stay();
        return new UdonAnalyzerSolution(solution);
    }
}