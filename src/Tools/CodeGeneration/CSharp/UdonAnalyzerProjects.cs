// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.CSharp;

internal static class UdonAnalyzerProjects
{
    // PLEASE ADD PROJECT UNIQUE ID TO THE FOLLOWING SECTION:

    public static UdonAnalyzerProject AnalyzersTests => new("Tests/Analyzers", "Analyzers.Tests.csproj", "Tests");
    public static UdonAnalyzerProject CodeFixesTests => new("Tests/CodeFixes", "CodeFixes.Tests.csproj", "Tests");
    public static UdonAnalyzerProject CodeGeneration => new("Tools/CodeGeneration", "CodeGeneration.csproj", "Tools");
    public static UdonAnalyzerProject ConsoleCore => new("Tools/ConsoleCore", "ConsoleCore.csproj", "Tools");
    public static UdonAnalyzerProject Analyzers => new("Analyzers", "Analyzers.csproj");
    public static UdonAnalyzerProject CodeFixes => new("CodeFixes", "CodeFixes.csproj");
    public static UdonAnalyzerProject Core => new("Core", "Core.csproj");
}