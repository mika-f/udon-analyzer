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

[Describe(typeof(DoesNotSupportDefaultArgumentsOrParamsArgumentsAnalyzer), "VSC")]
public class DoesNotSupportDefaultArgumentsOrParamsArgumentsAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotSupportDefaultArgumentsOrParamsArgumentsAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_ParamsArgumentOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod([|params string[] args|]) {}
}
");
    }

    [Fact]
    public async Task TestDiagnostic_DefaultArgumentOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod(int a, [|string b = """"|], [|int c = 0|]) {}
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_ParamsArgumentOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{
    public void TestMethod(params string[] args) {}
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_DefaultArgumentOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{
    public void TestMethod(int a, string b = """", int c = 0) {}
}
");
    }
}