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

[Describe(typeof(TheMethodSpecifiedForSendCustomEventMustBePublicAnalyzer), "VRC")]
public class TheMethodSpecifiedForSendCustomEventMustBePublicAnalyzerTest : UdonSharpDiagnosticVerifier<TheMethodSpecifiedForSendCustomEventMustBePublicAnalyzer>
{
    [Fact]
    public async Task TestDiagnostic_SendCustomEventDelayedFramesOnAnotherReceiverAndNotPublicMethod()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    private TestBehaviour1 _behaviour;

    public override void Interact()
    {
        [|_behaviour.SendCustomEventDelayedFrames(""TestMethod"", 1)|];
    }
}

class TestBehaviour1 : UdonSharpBehaviour
{
    private void TestMethod() {}
}
");
    }

    [Fact]
    public async Task TestDiagnostic_SendCustomEventDelayedFramesOnThisReceiverAndNotPublicMethod()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public override void Interact()
    {
        [|SendCustomEventDelayedFrames(""TestMethod"", 1)|];
    }

    private void TestMethod() { }
}
");
    }

    [Fact]
    public async Task TestDiagnostic_SendCustomEventDelayedSecondsOnAnotherReceiverAndNotPublicMethod()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    private TestBehaviour1 _behaviour;

    public override void Interact()
    {
        [|_behaviour.SendCustomEventDelayedSeconds(""TestMethod"", 1)|];
    }
}

class TestBehaviour1 : UdonSharpBehaviour
{
    private void TestMethod() {}
}
");
    }

    [Fact]
    public async Task TestDiagnostic_SendCustomEventDelayedSecondsOnThisReceiverAndNotPublicMethod()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public override void Interact()
    {
        [|SendCustomEventDelayedSeconds(""TestMethod"", 1)|];
    }

    private void TestMethod() { }
}
");
    }

    [Fact]
    [Example]
    public async Task TestDiagnostic_SendCustomEventOnAnotherReceiverAndNotPublicMethod()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    private TestBehaviour1 _behaviour;

    public override void Interact()
    {
        [|_behaviour.SendCustomEvent(""TestMethod"")|];
    }
}

class TestBehaviour1 : UdonSharpBehaviour
{
    private void TestMethod() {}
}

");
    }

    [Fact]
    public async Task TestDiagnostic_SendCustomEventOnAnotherReceiverAndPublicMethod()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    private TestBehaviour1 _behaviour;

    public override void Interact()
    {
        _behaviour.SendCustomEvent(""TestMethod"");
    }
}

class TestBehaviour1 : UdonSharpBehaviour
{
    public void TestMethod() {}
}

");
    }

    [Fact]
    [Example]
    public async Task TestDiagnostic_SendCustomEventOnThisReceiverAndNotPublicMethod()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public override void Interact()
    {
        [|SendCustomEvent(""TestMethod"")|];
    }

    private void TestMethod() { }
}
");
    }

    [Fact]
    public async Task TestDiagnostic_SendCustomEventOnThisReceiverAndPublicMethod()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public override void Interact()
    {
        SendCustomEvent(""TestMethod"");
    }

    public void TestMethod() { }
}
");
    }

    [Fact]
    public async Task TestDiagnostic_SendCustomNetworkEventOnThisReceiverAndNotPublicMethod()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.Udon.Common.Interfaces;

class TestBehaviour0 : UdonSharpBehaviour
{
    public override void Interact()
    {
        [|SendCustomNetworkEvent(NetworkEventTarget.All, ""TestMethod"")|];
    }

    private void TestMethod() { }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_GetComponentUdonBehaviourIsNotSendCustomNetworkEventTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.Udon;

class TestBehaviour0 : UdonSharpBehaviour
{
    private void TestMethod()
    {
        var _ = GetComponent<UdonBehaviour>();
    }
}

");
    }
}