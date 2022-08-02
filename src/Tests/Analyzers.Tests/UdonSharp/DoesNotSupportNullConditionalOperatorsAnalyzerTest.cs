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

[Describe(typeof(DoesNotSupportNullConditionalOperatorsAnalyzer), "VSC")]
public class DoesNotSupportNullConditionalOperatorsAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotSupportNullConditionalOperatorsAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_NullConditionalOperatorTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        string a = null;
        string b = [|a?.ToLowerInvariant()|];
    }
}
");
    }
}