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

[Describe(typeof(DoesNotCurrentlySupportTypeCheckingAnalyzer), "VRC")]
public class DoesNotCurrentlySupportTypeCheckingAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotCurrentlySupportTypeCheckingAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_IsPatternOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    void TestMethod()
    {
        var a = """";
        var b = [|a is string c|];
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_IsPatternOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{
    void TestMethod()
    {
        var a = """";
        var b = a is string c;
    }
}
");
    }
}