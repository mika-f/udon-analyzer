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

[Describe(typeof(DoesNotYetSupportObjectInitializersAnalyzer), "VSC")]
public class DoesNotYetSupportObjectInitializersAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotYetSupportObjectInitializersAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_ObjectCreationWithInitializerOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestObject
{
    public int SomeProperty { get; set; }
}

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod()
    {
        var some = new TestObject [|{ SomeProperty = 1 }|];
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_ObjectCreationWithoutInitializerOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestObject
{
    public int SomeProperty { get; set; }
}

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod()
    {
        var some = new TestObject();
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_ObjectCreationWithInitializerOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestObject
{
    public int SomeProperty { get; set; }
}

class TestBehaviour : MonoBehaviour
{
    public void TestMethod()
    {
        var some = new TestObject { SomeProperty = 1 };
    }
}
");
    }
}