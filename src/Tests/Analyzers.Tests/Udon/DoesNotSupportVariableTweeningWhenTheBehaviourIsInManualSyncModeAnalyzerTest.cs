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

[Describe(typeof(DoesNotSupportVariableTweeningWhenTheBehaviourIsInManualSyncModeAnalyzer), "VRC")]
public class DoesNotSupportVariableTweeningWhenTheBehaviourIsInManualSyncModeAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotSupportVariableTweeningWhenTheBehaviourIsInManualSyncModeAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_NonVariableTweeningWhenBehaviourIsManualSyncModeAndSyncedVariableIsTweening()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
class TestBehaviour0 : UdonSharpBehaviour
{
    [|[UdonSynced(UdonSyncMode.Linear)]
    private int _linear;|]

    [|[UdonSynced(UdonSyncMode.Smooth)]
    private int _smooth;|]
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_NonVariableTweeningWhenBehaviourIsManualSyncModeAndSyncedVariableIsNotTweening()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
class TestBehaviour0 : UdonSharpBehaviour
{
    [UdonSynced(UdonSyncMode.None)]
    private int _none;

    [UdonSynced]
    private int _default;
}
");
    }
}