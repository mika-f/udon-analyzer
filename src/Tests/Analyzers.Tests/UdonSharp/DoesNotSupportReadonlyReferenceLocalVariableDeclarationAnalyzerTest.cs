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

[Describe(typeof(DoesNotSupportReadonlyReferenceLocalVariableDeclarationAnalyzer), "VSC")]
public class DoesNotSupportReadonlyReferenceLocalVariableDeclarationAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotSupportReadonlyReferenceLocalVariableDeclarationAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_ReadonlyReferenceLocalVariableDeclarationOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod()
    {
        var a = 0;
        [|ref readonly var|] b = ref a;
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_ReferenceLocalVariableDeclarationOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod()
    {
        var a = 0;
        ref var b = ref a;
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_ReadonlyReferenceLocalVariableDeclarationOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{
    public void TestMethod()
    {
        var a = 0;
        ref readonly var b = ref a;
    }
}
");
    }
}