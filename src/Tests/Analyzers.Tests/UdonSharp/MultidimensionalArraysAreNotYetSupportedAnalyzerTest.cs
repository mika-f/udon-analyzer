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

[Describe(typeof(MultidimensionalArraysAreNotYetSupportedAnalyzer), "VSC")]
public class MultidimensionalArraysAreNotYetSupportedAnalyzerTest : UdonSharpDiagnosticVerifier<MultidimensionalArraysAreNotYetSupportedAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_MultidimensionalArrayCreationTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        var arr = [|new int[1, 2]|];
    }
}
");
    }

    [Fact]
    public async Task TestDiagnostic_MultidimensionalImplicitArrayCreationTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
	    var arr = [|new[,] { { 1 }, { 2 } }|];
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_JaggedArrayCreationTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        var arr = new int[5][];
    }
}
");
    }
}