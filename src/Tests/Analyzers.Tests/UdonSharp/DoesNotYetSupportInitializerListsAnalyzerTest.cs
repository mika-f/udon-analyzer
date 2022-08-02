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

[Describe(typeof(DoesNotYetSupportInitializerListsAnalyzer), "VSC")]
public class DoesNotYetSupportInitializerListsAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotYetSupportInitializerListsAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_InitializersOnObjectTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        var go = new GameObject [|{ name = ""Hello, World"" }|];
    }
}
");
    }
}