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

[Describe(typeof(DoesNotSupportTheAwakeEventAnalyzer), "VRC")]
public class DoesNotSupportTheAwakeEventAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotSupportTheAwakeEventAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_AwakeEventOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    [|public void Awake() {}|]
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_AwakeMethodOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public void Awake(object obj) {}
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_AwakeEventOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{
    public void Awake() {}
}
");
    }
}