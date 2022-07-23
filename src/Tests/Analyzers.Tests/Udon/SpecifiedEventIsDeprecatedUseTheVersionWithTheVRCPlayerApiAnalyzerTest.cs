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

[Describe(typeof(SpecifiedEventIsDeprecatedUseTheVersionWithTheVRCPlayerApiAnalyzer), "VRC")]
// ReSharper disable once InconsistentNaming
public class SpecifiedEventIsDeprecatedUseTheVersionWithTheVRCPlayerApiAnalyzerTest : UdonSharpDiagnosticVerifier<SpecifiedEventIsDeprecatedUseTheVersionWithTheVRCPlayerApiAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_DeprecatedVRChatEventTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    [|public override void OnStationEntered() {}|@OnStationEntered]

    [|public override void OnStationExited() {}|@OnStationExited]

    [|public override void OnOwnershipTransferred() {}|@OnOwnershipTransferred]
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_NewVersionOfVRChatEventTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public override void OnStationEntered(VRCPlayerApi player) {}

    public override void OnStationExited(VRCPlayerApi player) {}

    public override void OnOwnershipTransferred(VRCPlayerApi api) {}
}
");
    }
}