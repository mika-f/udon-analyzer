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

[Describe(typeof(SyncingOfArrayTypesIsOnlySupportedInManualSyncModeAnalyzer), "VRC")]
public class SyncingOfArrayTypesIsOnlySupportedInManualSyncModeAnalyzerTest : UdonSharpDiagnosticVerifier<SyncingOfArrayTypesIsOnlySupportedInManualSyncModeAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_ArraySyncingInContinuousMode()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
class TestBehaviour0 : UdonSharpBehaviour
{
    [|[UdonSynced]
    private string[] _urls;|@string[\]]
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_ArraySyncingInManualMode()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
class TestBehaviour0 : UdonSharpBehaviour
{
    [UdonSynced]
    private string[] _urls;
}
");
    }
}