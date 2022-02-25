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

[Describe(typeof(DoesNotSupportMultidimensionalArraysAnalyzer), "VSC")]
public class DoesNotSupportMultidimensionalArraysAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotSupportMultidimensionalArraysAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_MultidimensionalArrayDeclarationWithRankSpecifiersOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod()
    {
        var a = [|new int[1,1]|];
    }
}
");
    }

    [Fact]
    public async Task TestDiagnostic_MultidimensionalArrayDeclarationWithoutRankSpecifiersOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod()
    {
        var a = [|new int[,] { { 1, 2 } }|];
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_MultidimensionalArrayDeclarationOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{
    public void TestMethod()
    {
        var a = new int[1,1];
    }
}
");
    }
}