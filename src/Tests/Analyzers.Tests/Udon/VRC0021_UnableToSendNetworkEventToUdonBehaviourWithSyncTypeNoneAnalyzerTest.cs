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

[Describe(typeof(UnableToSendNetworkEventToUdonBehaviourWithSyncTypeNoneAnalyzer), "VRC")]
public class UnableToSendNetworkEventToUdonBehaviourWithSyncTypeNoneAnalyzerTest : UdonSharpDiagnosticVerifier<UnableToSendNetworkEventToUdonBehaviourWithSyncTypeNoneAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_SendCustomNetworkEventToUdonBehaviourThatSpecifiedSyncModeNone()
    {
        await VerifyAnalyzerAsync("""

                                  using UdonSharp;

                                  using VRC.Udon.Common.Interfaces;

                                  class TestBehaviour1 : UdonSharpBehaviour
                                  {
                                      private TestBehaviour2 _other;

                                      public void SomeMethod()
                                      {
                                          [|_other.SendCustomNetworkEvent(NetworkEventTarget.All, "SomeMethod")|@TestBehaviour2];
                                      }
                                  }

                                  [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
                                  class TestBehaviour2 : UdonSharpBehaviour
                                  {
                                      public void SomeMethod() {}
                                  }

                                  """);
    }

    [Fact]
    public async Task TestNoDiagnostic_SendCustomNetworkEventToUdonBehaviourThatSpecifiedSyncModeNotSpecified()
    {
        await VerifyAnalyzerAsync("""

                                  using UdonSharp;

                                  using VRC.Udon.Common.Interfaces;

                                  class TestBehaviour1 : UdonSharpBehaviour
                                  {
                                      private TestBehaviour2 _other;

                                      public void SomeMethod()
                                      {
                                          _other.SendCustomNetworkEvent(NetworkEventTarget.All, "SomeMethod");
                                      }
                                  }

                                  class TestBehaviour2 : UdonSharpBehaviour
                                  {
                                      public void SomeMethod() {}
                                  }

                                  """);
    }


    [Fact]
    public async Task TestNoDiagnostic_SendCustomNetworkEventToUdonBehaviourThatSpecifiedSyncModeAny()
    {
        await VerifyAnalyzerAsync("""

                                  using UdonSharp;

                                  using VRC.Udon.Common.Interfaces;

                                  class TestBehaviour1 : UdonSharpBehaviour
                                  {
                                      private TestBehaviour2 _other;

                                      public void SomeMethod()
                                      {
                                          _other.SendCustomNetworkEvent(NetworkEventTarget.All, "SomeMethod");
                                      }
                                  }

                                  [UdonBehaviourSyncMode(BehaviourSyncMode.Any)]
                                  class TestBehaviour2 : UdonSharpBehaviour
                                  {
                                      public void SomeMethod() {}
                                  }

                                  """);
    }


    [Fact]
    public async Task TestNoDiagnostic_SendCustomNetworkEventToUdonBehaviourThatSpecifiedSyncModeNoVariableSync()
    {
        await VerifyAnalyzerAsync("""

                                  using UdonSharp;

                                  using VRC.Udon.Common.Interfaces;

                                  class TestBehaviour1 : UdonSharpBehaviour
                                  {
                                      private TestBehaviour2 _other;

                                      public void SomeMethod()
                                      {
                                          _other.SendCustomNetworkEvent(NetworkEventTarget.All, "SomeMethod");
                                      }
                                  }

                                  [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
                                  class TestBehaviour2 : UdonSharpBehaviour
                                  {
                                      public void SomeMethod() {}
                                  }

                                  """);
    }


    [Fact]
    public async Task TestNoDiagnostic_SendCustomNetworkEventToUdonBehaviourThatSpecifiedSyncModeContinuous()
    {
        await VerifyAnalyzerAsync("""

                                  using UdonSharp;

                                  using VRC.Udon.Common.Interfaces;

                                  class TestBehaviour1 : UdonSharpBehaviour
                                  {
                                      private TestBehaviour2 _other;

                                      public void SomeMethod()
                                      {
                                          _other.SendCustomNetworkEvent(NetworkEventTarget.All, "SomeMethod");
                                      }
                                  }

                                  [UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
                                  class TestBehaviour2 : UdonSharpBehaviour
                                  {
                                      public void SomeMethod() {}
                                  }

                                  """);
    }


    [Fact]
    public async Task TestNoDiagnostic_SendCustomNetworkEventToUdonBehaviourThatSpecifiedSyncModeManual()
    {
        await VerifyAnalyzerAsync("""

                                  using UdonSharp;

                                  using VRC.Udon.Common.Interfaces;

                                  class TestBehaviour1 : UdonSharpBehaviour
                                  {
                                      private TestBehaviour2 _other;

                                      public void SomeMethod()
                                      {
                                          _other.SendCustomNetworkEvent(NetworkEventTarget.All, "SomeMethod");
                                      }
                                  }

                                  [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
                                  class TestBehaviour2 : UdonSharpBehaviour
                                  {
                                      public void SomeMethod() {}
                                  }

                                  """);
    }
}