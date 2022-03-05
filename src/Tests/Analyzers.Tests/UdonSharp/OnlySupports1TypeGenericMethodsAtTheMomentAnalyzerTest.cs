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

[Describe(typeof(OnlySupports1TypeGenericMethodsAtTheMomentAnalyzer), "VSC")]
public class OnlySupports1TypeGenericMethodsAtTheMomentAnalyzerTest : UdonSharpDiagnosticVerifier<OnlySupports1TypeGenericMethodsAtTheMomentAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_MethodInvocationWithMultipleGenericsOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    // below method definition might not work on UdonSharp, but this test disable all other analyzers
    void Test<T1, T2>() {}

    void TestMethod()
    {
        [|Test<string, int>()|];
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_MethodInvocationWithMultipleGenericOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{
    void Test<T1, T2>() {}

    void TestMethod()
    {
        Test<string, int>();
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_MethodInvocationWithSingleGenericsOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    void TestMethod()
    {
        GetComponents<Rigidbody>();
    }
}
");
    }
}