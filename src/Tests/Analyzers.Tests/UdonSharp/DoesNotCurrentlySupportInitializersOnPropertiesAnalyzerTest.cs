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

[Describe(typeof(DoesNotCurrentlySupportInitializersOnPropertiesAnalyzer), "VSC")]
public class DoesNotCurrentlySupportInitializersOnPropertiesAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotCurrentlySupportInitializersOnPropertiesAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_UserDefinedPropertyWithInitializerOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public int SomeProperty { get; set; } [|= 1|];
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_UserDefinedPropertyWithoutInitializerOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public int SomeProperty { get; set; }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_UserDefinedPropertyWithInitializerOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{
    public int SomeProperty { get; set; } = 1;
}
");
    }
}