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

[Describe(typeof(NetworkCallableAttributeMustBeRequiredForCallingMethodViaSendCustomNetworkEventWithParametersAnalyzer), "VRC")]
public class NetworkCallableAttributeMustBeRequiredForCallingMethodViaSendCustomNetworkEventWithParametersAnalyzerTest : UdonSharpDiagnosticVerifier<NetworkCallableAttributeMustBeRequiredForCallingMethodViaSendCustomNetworkEventWithParametersAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_TheSpecifiedMethodDoesNotHaveNetworkCallableAttribute()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.SDK3.UdonNetworkCalling;
using VRC.Udon.Common.Interfaces;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod()
    {
        [|SendCustomNetworkEvent(NetworkEventTarget.All, ""SomeMethod"", 1)|];
    }

    public void SomeMethod(int value) { }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_TheSpecifiedMethoHaveNetworkCallableAttribute()
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

    [NetworkCallable]
    public void SomeMethod(int value) { }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_TheSpecifiedMethoHaveNonParameters()
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
}