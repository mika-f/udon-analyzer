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

[Describe(typeof(DoesNotSupportMultidimensionalArrayAccessesAnalyzer), "VSC")]
public class DoesNotSupportMultidimensionalArrayAccessesAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotSupportMultidimensionalArrayAccessesAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_MultidimensionalArrayAccessOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod()
    {
        var a = new [1,1];
        [|a[0,0]|] = 1;
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_MultidimensionalArrayAccessOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{
    public void TestMethod()
    {
        var a = new [1,1];
        a[0,0]= 1;
    }
}
");
    }
}