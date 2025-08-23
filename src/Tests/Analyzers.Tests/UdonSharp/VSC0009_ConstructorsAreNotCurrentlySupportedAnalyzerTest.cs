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

[Describe(typeof(ConstructorsAreNotCurrentlySupportedAnalyzer), "VSC")]
public class ConstructorsAreNotCurrentlySupportedAnalyzerTest : UdonSharpDiagnosticVerifier<ConstructorsAreNotCurrentlySupportedAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_ConstructoresOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    [|public TestBehaviour0() { }|]

    [|public TestBehaviour0(string arg) { }|]
}
");
    }
}