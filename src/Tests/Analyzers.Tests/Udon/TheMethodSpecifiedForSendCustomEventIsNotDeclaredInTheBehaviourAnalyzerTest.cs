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

[Describe(typeof(TheMethodSpecifiedForSendCustomEventIsNotDeclaredInTheBehaviourAnalyzer), "VRC")]
public class TheMethodSpecifiedForSendCustomEventIsNotDeclaredInTheBehaviourAnalyzerTest : UdonSharpDiagnosticVerifier<TheMethodSpecifiedForSendCustomEventIsNotDeclaredInTheBehaviourAnalyzer>
{
    [Fact]
    public async Task TestDiagnostic_TheMethodSpecifiedForSendCustomEventDelayedFramesIsNotDeclaredInAnotherReceiver()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    private TestBehaviour1 _behaviour;

    public void TestMethod()
    {
        [|_behaviour.SendCustomEventDelayedFrames(""SomeMethod"", 1)|];
    }
}

class TestBehaviour1 : UdonSharpBehaviour { }
");
    }

    [Fact]
    public async Task TestDiagnostic_TheMethodSpecifiedForSendCustomEventDelayedFramesIsNotDeclaredInThisReceiver()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        [|SendCustomEventDelayedFrames(""SomeMethod"", 1)|];
    }
}
");
    }

    [Fact]
    public async Task TestDiagnostic_TheMethodSpecifiedForSendCustomEventDelayedSecondsIsNotDeclaredInAnotherReceiver()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    private TestBehaviour1 _behaviour;

    public void TestMethod()
    {
        [|_behaviour.SendCustomEventDelayedSeconds(""SomeMethod"", 1)|];
    }
}

class TestBehaviour1 : UdonSharpBehaviour {}
");
    }

    [Fact]
    public async Task TestDiagnostic_TheMethodSpecifiedForSendCustomEventDelayedSecondsIsNotDeclaredInThisReceiver()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        [|SendCustomEventDelayedSeconds(""SomeMethod"", 1)|];
    }
}
");
    }


    [Fact]
    [Example]
    public async Task TestDiagnostic_TheMethodSpecifiedForSendCustomEventIsNotDeclaredInAnotherReceiver()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    private TestBehaviour1 _behaviour;

    public void TestMethod()
    {
        [|_behaviour.SendCustomEvent(""SomeMethod"")|];
    }
}

class TestBehaviour1 : UdonSharpBehaviour {}
");
    }


    [Fact]
    [Example]
    public async Task TestDiagnostic_TheMethodSpecifiedForSendCustomEventIsNotDeclaredInThisReceiver()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        [|SendCustomEvent(""SomeMethod"")|];
    }
}
");
    }

    [Fact]
    public async Task TestDiagnostic_TheMethodSpecifiedForSendCustomNetworkEventIsNotDeclaredInAnotherReceiver()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.Udon.Common.Interfaces;

class TestBehaviour0 : UdonSharpBehaviour
{
    private TestBehaviour1 _behaviour;

    public void TestMethod()
    {
        [|_behaviour.SendCustomNetworkEvent(NetworkEventTarget.Al, ""SomeMethod"")|];
    }
}

class TestBehaviour1 : UdonSharpBehaviour {}
");
    }

    [Fact]
    public async Task TestDiagnostic_TheMethodSpecifiedForSendCustomNetworkEventIsNotDeclaredInThisReceiver()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.Udon.Common.Interfaces;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        [|SendCustomNetworkEvent(NetworkEventTarget.Al, ""SomeMethod"")|];
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_TheMethodSpecifiedForSendCustomEventDelayedFramesIsDeclaredInAnotherReceiver()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    private TestBehaviour1 _behaviour;

    public void TestMethod()
    {
        _behaviour.SendCustomEventDelayedFrames(""SomeMethod"", 1);
    }
}

class TestBehaviour1 : UdonSharpBehaviour
{
    public void SomeMethod() {}
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_TheMethodSpecifiedForSendCustomEventDelayedFramesIsDeclaredInThisReceiver()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        SendCustomEventDelayedFrames(""SomeMethod"", 1);
    }

    public void SomeMethod() {}
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_TheMethodSpecifiedForSendCustomEventDelayedSecondsIsDeclaredInAnotherReceiver()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    private TestBehaviour1 _behaviour;

    public void TestMethod()
    {
        _behaviour.SendCustomEventDelayedSeconds(""SomeMethod"", 1);
    }
}

class TestBehaviour1 : UdonSharpBehaviour
{
    public void SomeMethod() {}
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_TheMethodSpecifiedForSendCustomEventDelayedSecondsIsDeclaredInThisReceiver()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        SendCustomEventDelayedSeconds(""SomeMethod"", 1);
    }

    public void SomeMethod() {}
}
");
    }


    [Fact]
    public async Task TestNoDiagnostic_TheMethodSpecifiedForSendCustomEventIsDeclaredInAnotherReceiver()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    private TestBehaviour1 _behaviour;

    public void TestMethod()
    {
        _behaviour.SendCustomEvent(""SomeMethod"");
    }
}

class TestBehaviour1 : UdonSharpBehaviour
{
    public void SomeMethod() {}
}
");
    }


    [Fact]
    public async Task TestNoDiagnostic_TheMethodSpecifiedForSendCustomEventIsDeclaredInThisReceiver()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        SendCustomEvent(""SomeMethod"");
    }

    public void SomeMethod() {}
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_TheMethodSpecifiedForSendCustomNetworkEventIsDeclaredInAnotherReceiver()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.Udon.Common.Interfaces;

class TestBehaviour0 : UdonSharpBehaviour
{
    private TestBehaviour1 _behaviour;

    public void TestMethod()
    {
        _behaviour.SendCustomNetworkEvent(NetworkEventTarget.Al, ""SomeMethod"");
    }
}

class TestBehaviour1 : UdonSharpBehaviour
{
    public void SomeMethod() {}
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_TheMethodSpecifiedForSendCustomNetworkEventIsDeclaredInThisReceiver()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.Udon.Common.Interfaces;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        SendCustomNetworkEvent(NetworkEventTarget.Al, ""SomeMethod"");
    }

    public void SomeMethod() {}
}
");
    }
}