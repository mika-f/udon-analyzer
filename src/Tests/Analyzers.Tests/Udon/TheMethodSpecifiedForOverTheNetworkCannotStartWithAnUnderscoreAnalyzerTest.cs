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

[Describe(typeof(TheMethodSpecifiedForOverTheNetworkCannotStartWithAnUnderscoreAnalyzer), "VRC")]
public class TheMethodSpecifiedForOverTheNetworkCannotStartWithAnUnderscoreAnalyzerTest : UdonSharpDiagnosticVerifier<TheMethodSpecifiedForOverTheNetworkCannotStartWithAnUnderscoreAnalyzer>
{
    [Fact]
    public async Task TestDiagnostic_TestMethodStartsWithUnderscoreOnAnotherReceiverSpecifiedOnSendCustomNetworkEventTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.Udon.Common.Interfaces;

class TestBehaviour0 : UdonSharpBehaviour
{
    private TestBehaviour1 _behaviour;

    private void TestMethod()
    {
        [|_behaviour.SendCustomNetworkEvent(NetworkEventTarget.All, ""_TestMethod"")|];
    }
}

class TestBehaviour1 : UdonSharpBehaviour
{
    private void _TestMethod() {}
}
");
    }

    [Fact]
    [Example]
    public async Task TestDiagnostic_TheMethodStartsWithUnderscoreOnThisReceiverSpecifiedOnSendCustomNetworkEventTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.Udon.Common.Interfaces;

class TestBehaviour0 : UdonSharpBehaviour
{
    private void TestMethod()
    {
        [|SendCustomNetworkEvent(NetworkEventTarget.All, ""_TestMethod"")|];
    }

    private void _TestMethod() {}
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_TestMethodNotStartsWithUnderscoreOnAnotherReceiverSpecifiedOnSendCustomNetworkEventTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.Udon.Common.Interfaces;

class TestBehaviour0 : UdonSharpBehaviour
{
    private TestBehaviour1 _behaviour;

    private void TestMethod()
    {
        _behaviour.SendCustomNetworkEvent(NetworkEventTarget.All, ""TestMethod"");
    }
}

class TestBehaviour1 : UdonSharpBehaviour
{
    private void TestMethod() {}
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_TheMethodNotStartsWithUnderscoreOnThisReceiverSpecifiedOnSendCustomNetworkEventTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.Udon.Common.Interfaces;

class TestBehaviour0 : UdonSharpBehaviour
{
    private void TestMethod()
    {
        SendCustomNetworkEvent(NetworkEventTarget.All, ""_TestMethod"");
    }

    private void TestMethod() {}
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