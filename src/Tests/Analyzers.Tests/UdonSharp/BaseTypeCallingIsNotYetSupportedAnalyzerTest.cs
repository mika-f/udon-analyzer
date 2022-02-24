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

[Describe(typeof(BaseTypeCallingIsNotYetSupportedAnalyzer), "VSC")]
public class BaseTypeCallingIsNotYetSupportedAnalyzerTest : UdonSharpDiagnosticVerifier<BaseTypeCallingIsNotYetSupportedAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_BaseTypeCallingOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod()
    {
        [|base|].OnDrop();
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_BaseTypeCallingOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{
    public void TestMethod()
    {
        base.IsInvoking();
    }
}
");
    }
}