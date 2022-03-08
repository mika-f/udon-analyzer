// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Threading.Tasks;

using NatsunekoLaboratory.UdonAnalyzer.AnalyzerSpec.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.Testing;
using NatsunekoLaboratory.UdonAnalyzer.Udon;

using Xunit;

namespace Analyzers.Tests.Udon;

[Describe(typeof(DoesNotCurrentlySupportTypeCastingAnalyzer), "VRC")]
public class DoesNotCurrentlySupportTypeCastingAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotCurrentlySupportTypeCastingAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_AsExpressionOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    void TestMethod()
    {
        var a = """";
        var b = [|a as string|];
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_AsExpressionOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{
    void TestMethod()
    {
        var a = """";
        var b = a as string;
    }
}
");
    }
}