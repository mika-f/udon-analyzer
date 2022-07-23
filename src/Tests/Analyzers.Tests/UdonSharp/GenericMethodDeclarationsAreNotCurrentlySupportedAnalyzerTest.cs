// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

using NatsunekoLaboratory.UdonAnalyzer.AnalyzerSpec.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.Testing;
using NatsunekoLaboratory.UdonAnalyzer.UdonSharp;

using Xunit;

namespace Analyzers.Tests.UdonSharp;

[Describe(typeof(GenericMethodDeclarationsAreNotCurrentlySupportedAnalyzer), "VSC")]
public class GenericMethodDeclarationsAreNotCurrentlySupportedAnalyzerTest : UdonSharpDiagnosticVerifier<GenericMethodDeclarationsAreNotCurrentlySupportedAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_GenericMethodDeclarationOnUdonSharpBehaviourTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    [|public void TestMethod<T>() {}|]
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_GenericMethodDeclarationOnNonUdonSharpBehaviourOnGlobalWorkspaceAnalyzingTest()
    {
        var editorconfig = new Dictionary<string, string>
        {
            { "udon_analyzer.enable_workspace_analyzing", "true" }
        };

        await VerifyAnalyzerAsync(@"
class TestBehaviour0
{
    public void TestMethod<T>() {}
}
", editorconfig);
    }
}