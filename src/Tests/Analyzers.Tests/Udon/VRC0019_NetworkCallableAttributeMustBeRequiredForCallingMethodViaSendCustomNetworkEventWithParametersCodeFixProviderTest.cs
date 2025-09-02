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

[Describe(typeof(NetworkCallableAttributeMustBeRequiredForCallingMethodViaSendCustomNetworkEventWithParametersCodeFixProvider), "VRC")]
public class NetworkCallableAttributeMustBeRequiredForCallingMethodViaSendCustomNetworkEventWithParametersCodeFixProviderTest : UdonSharpCodeFixVerifier<
    NetworkCallableAttributeMustBeRequiredForCallingMethodViaSendCustomNetworkEventWithParametersAnalyzer,
    NetworkCallableAttributeMustBeRequiredForCallingMethodViaSendCustomNetworkEventWithParametersCodeFixProvider
>
{
    [Fact]
    [Example]
    public async Task TestCodeFix_MethodCalledViaSendCustomNetworkEventWithParametersIsNotMarkedWithNetworkCallableAttribute()
    {
        await VerifyCodeFixAsync("""
                                 using UdonSharp;

                                 using VRC.SDK3.UdonNetworkCalling;
                                 using VRC.Udon.Common.Interfaces;

                                 class TestBehaviour : UdonSharpBehaviour
                                 {
                                     public void TestMethod()
                                     {
                                         [|SendCustomNetworkEvent(NetworkEventTarget.All, "SomeMethod", 1)|];
                                     }

                                     public void SomeMethod(int value) { }

                                 """,
                                 """
                                 using UdonSharp;

                                 using VRC.SDK3.UdonNetworkCalling;
                                 using VRC.Udon.Common.Interfaces;

                                 class TestBehaviour : UdonSharpBehaviour
                                 {
                                     public void TestMethod()
                                     {
                                         SendCustomNetworkEvent(NetworkEventTarget.All, "SomeMethod", 1);
                                     }

                                     [global::VRC.SDK3.UdonNetworkCalling.NetworkCallable]
                                     public void SomeMethod(int value) { }
                                 }
                                 """
        );
    }
}