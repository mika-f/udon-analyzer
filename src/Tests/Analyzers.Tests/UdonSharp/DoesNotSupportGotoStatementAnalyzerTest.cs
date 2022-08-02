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

[Describe(typeof(DoesNotSupportGotoStatementAnalyzer), "VSC")]
public class DoesNotSupportGotoStatementAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotSupportGotoStatementAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_GotoStatement()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        [|goto label1;|]

label1:
        return;
    }
}
");
    }
}