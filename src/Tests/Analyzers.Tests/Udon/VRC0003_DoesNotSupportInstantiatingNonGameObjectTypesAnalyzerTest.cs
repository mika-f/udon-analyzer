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

[Describe(typeof(DoesNotSupportInstantiatingNonGameObjectTypesAnalyzer), "VRC")]
public class DoesNotSupportInstantiatingNonGameObjectTypesAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotSupportInstantiatingNonGameObjectTypesAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_InstantiatingNonGameObjectTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using UnityEngine;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        [|Instantiate(new TrailRenderer())|];
    }
}
");
    }

    [Fact]
    public async Task TestDiagnostic_InstantiatingNullObjectTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using UnityEngine;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        [|Instantiate(null)|];
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_InstantiatingGameObjectTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using UnityEngine;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        Instantiate(new GameObject());
    }
}
");
    }
}