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

[Describe(typeof(DoesNotCurrentlySupportStaticUserDefinedPropertiesAnalyzer), "VSC")]
public class DoesNotCurrentlySupportStaticUserDefinedPropertiesAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotCurrentlySupportStaticUserDefinedPropertiesAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_StaticUserDefinedPropertyOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    [|public static int SomeProperty { get; set; }|]
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_InstanceUserDefinedPropertyOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public int SomeProperty { get; set; }
}
");
    }
}