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

[Describe(typeof(UseTheNameOfOperatorInsteadOfDirectlySpecifyingTheMethodNameAnalyzer), "VSC")]
public class UseTheNameOfOperatorInsteadOfDirectlySpecifyingTheMethodNameAnalyzerTest : UdonSharpDiagnosticVerifier<UseTheNameOfOperatorInsteadOfDirectlySpecifyingTheMethodNameAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_SendCustomEventTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    private TestBehaviour0 _behaviour;

    public void TestMethod()
    {
        _behaviour.SendCustomEvent([|""TestMethod""|]);
        SendCustomEvent([|""TestMethod""|]);
    }
}
");
    }

    [Fact]
    public async Task TestDiagnostic_SendCustomEventDelayedFramesTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    private TestBehaviour0 _behaviour;

    public void TestMethod()
    {
        _behaviour.SendCustomEventDelayedFrames([|""TestMethod""|], 1);
    }
}
");
    }

    [Fact]
    public async Task TestDiagnostic_SendCustomEventDelayedSecondsTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    private TestBehaviour0 _behaviour;

    public void TestMethod()
    {
        _behaviour.SendCustomEventDelayedSeconds([|""TestMethod""|], 1);
    }
}
");
    }

    [Fact]
    public async Task TestDiagnostic_SendCustomNetworkEventTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.Udon.Common.Interfaces;

class TestBehaviour0 : UdonSharpBehaviour
{
    private TestBehaviour0 _behaviour;

    public void TestMethod()
    {
        _behaviour.SendCustomNetworkEvent(NetworkEventTarget.All, [|""TestMethod""|], 1);
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_UseNameOfOperatorTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.Udon.Common.Interfaces;

class TestBehaviour0 : UdonSharpBehaviour
{
    private TestBehaviour0 _behaviour;

    public void TestMethod()
    {
        _behaviour.SendCustomNetworkEvent(NetworkEventTarget.All, nameof(TestMethod), 1);
    }
}
");
    }
}