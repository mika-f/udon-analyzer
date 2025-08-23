// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Threading.Tasks;

using NatsunekoLaboratory.UdonAnalyzer.AnalyzerSpec.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.Testing;
using NatsunekoLaboratory.UdonAnalyzer.UdonSharp;

using Xunit;

namespace Analyzers.Tests.UdonSharp;

[Describe(typeof(PartialMethodDeclarationsAreNotYetSupportedAnalyzer), "VSC")]
public class PartialMethodDeclarationsAreNotYetSupportedAnalyzerTest : UdonSharpDiagnosticVerifier<PartialMethodDeclarationsAreNotYetSupportedAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_PartialMethodDeclarationTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

partial class TestBehaviour0 : UdonSharpBehaviour
{
    [|partial void TestMethod();|]
}

partial class TestBehaviour0
{
    [|partial void TestMethod()
    {
    }|]
}

");
    }

    [Fact]
    public async Task TestNoDiagnostic_NonPartialMethodDeclarationTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

partial class TestBehaviour0 : UdonSharpBehaviour
{
    void TestMethod1() {}
}

partial class TestBehaviour0
{
    void TestMethod2() {}
}
");
    }
}