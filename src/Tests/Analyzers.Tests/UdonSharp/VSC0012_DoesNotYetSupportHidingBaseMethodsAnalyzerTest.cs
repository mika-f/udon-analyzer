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

[Describe(typeof(DoesNotYetSupportHidingBaseMethodsAnalyzer), "VSC")]
public class DoesNotYetSupportHidingBaseMethodsAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotYetSupportHidingBaseMethodsAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_HidingMethod()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod(string args) {}
}

class TestBehaviour1 : TestBehaviour0
{
    [|public void TestMethod(string args) {}|@TestMethod]
}
", EnableWorkspaceAnalyzing());
    }

    [Fact]
    public async Task TestNoDiagnostic_NotHidingMethodWithDifferenceArgumentCount()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod() {}
}

class TestBehaviour1 : TestBehaviour0
{
    public void TestMethod(string arg) {}
}
", EnableWorkspaceAnalyzing());
    }

    [Fact]
    public async Task TestNoDiagnostic_NotHidingMethodWithDifferenceArgumentType()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod(int args) {}
}

class TestBehaviour1 : TestBehaviour0
{
    public void TestMethod(string arg) {}
}
", EnableWorkspaceAnalyzing());
    }

    [Fact]
    public async Task TestNoDiagnostic_NotHidingMethodWithOverrideModifiersTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public override void OnPreSerialization()
    {
        base.OnPreSerialization();
    }
}
");
    }
}