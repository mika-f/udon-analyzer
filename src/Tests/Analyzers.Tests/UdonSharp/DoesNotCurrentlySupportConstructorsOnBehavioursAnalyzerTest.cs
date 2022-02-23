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

[Describe(typeof(DoesNotCurrentlySupportConstructorsOnBehavioursAnalyzer), "VSC")]
public class DoesNotCurrentlySupportConstructorsOnBehavioursAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotCurrentlySupportConstructorsOnBehavioursAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_ConstructorOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    [|public TestBehaviour() {}|]
}
");
    }

    [Fact]
    public async Task TestDiagnostic_ConstructorWithArgumentsOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    [|public TestBehaviour(string arg) {}|]
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_ConstructorNotOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : MonoBehaviour
{
    public TestBehaviour() {}
}
");
    }
}