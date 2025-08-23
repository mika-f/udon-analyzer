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

[Describe(typeof(TheAsKeywordIsNotYetSupportedAnalyzer), "VRC")]
public class TheAsKeywordIsNotYetSupportedAnalyzerTest : UdonSharpDiagnosticVerifier<TheAsKeywordIsNotYetSupportedAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_AsKeywordExpression()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        var a = """";
        var b = [|a as object|];
    }
}
");
    }
}