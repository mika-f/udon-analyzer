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

[Describe(typeof(TheParameterTypeOfMethodDoesNotMatchTheNthArgumentOfSendCustomNetworkEventAnalyzer), "VRC")]
public class TheParameterTypeOfMethodDoesNotMatchTheNthArgumentOfSendCustomNetworkEventAnalyzerTest : UdonSharpDiagnosticVerifier<TheParameterTypeOfMethodDoesNotMatchTheNthArgumentOfSendCustomNetworkEventAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_TheSpecifiedParameterDoesNotMatchToArgumentSignature_InContext()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.SDK3.UdonNetworkCalling;
using VRC.Udon.Common.Interfaces;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod()
    {
        SendCustomNetworkEvent(NetworkEventTarget.All, ""SomeMethod"", [|(long)1|@int,TestBehaviour.SomeMethod(int),3,long]);
    }

    public void SomeMethod(int value) { }
}
");
    }

    [Fact]
    public async Task TestDiagnostic_TheSpecifiedParameterDoesNotMatchToArgumentSignature_InOtherContext()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.SDK3.UdonNetworkCalling;
using VRC.Udon.Common.Interfaces;

class TestBehaviour : UdonSharpBehaviour
{
    private TestBehaviour1 _other;
    
    public void TestMethod()
    {
        _other.SendCustomNetworkEvent(NetworkEventTarget.All, ""SomeMethod"", [|(long)1|@int,TestBehaviour1.SomeMethod(int),3,long]);
    }
}

class TestBehaviour1 : UdonSharpBehaviour
{
    public void SomeMethod(int value) { }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_TheSpecifiedParameterIsNone()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.SDK3.UdonNetworkCalling;
using VRC.Udon.Common.Interfaces;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod()
    {
        SendCustomNetworkEvent(NetworkEventTarget.All, ""SomeMethod"");
    }

    public void SomeMethod() { }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_TheSpecifiedParameterMatchedToArgument()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.SDK3.UdonNetworkCalling;
using VRC.Udon.Common.Interfaces;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod()
    {
        SendCustomNetworkEvent(NetworkEventTarget.All, ""SomeMethod"", 1);
    }

    public void SomeMethod(int value) { }
}
");
    }
}