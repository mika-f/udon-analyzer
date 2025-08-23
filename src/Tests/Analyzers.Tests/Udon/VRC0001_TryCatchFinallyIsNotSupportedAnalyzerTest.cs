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

[Describe(typeof(TryCatchFinallyIsNotSupportedAnalyzer), "VRC")]
public class TryCatchFinallyIsNotSupportedAnalyzerTest : UdonSharpDiagnosticVerifier<TryCatchFinallyIsNotSupportedAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_TryCatchStatementTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        [|try {} catch { /* ignored */ }|]
    }
}
");
    }

    [Fact]
    public async Task TestDiagnostic_TryFinallyStatementTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        [|try {} catch { /* ignored */ } finally { }|]
    }
}
");
    }
}