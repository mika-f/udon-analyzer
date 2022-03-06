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

[Describe(typeof(DoesNotYetSupportStaticUsingDirectivesAnalyzer), "VSC")]
public class DoesNotYetSupportStaticUsingDirectivesAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotYetSupportStaticUsingDirectivesAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_StaticUsingDirectivesOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

[|using static UnityEngine.Mathf;|]

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod()
    {
        var a = Abs(-1f);
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_StaticUsingDirectivesOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

using static UnityEngine.Mathf;

class TestBehaviour : MonoBehaviour
{
    public void TestMethod()
    {
        var a = Abs(-1f);
    }
}
");
    }
}