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

[Describe(typeof(CannotSyncVariableBecauseBehaviourIsSetToNoVariableSyncAnalyzer), "VRC")]
public class CannotSyncVariableBecauseBehaviourIsSetToNoVariableSyncAnalyzerTest : UdonSharpDiagnosticVerifier<CannotSyncVariableBecauseBehaviourIsSetToNoVariableSyncAnalyzer>
{
    [Fact]
    public async Task TestDiagnostic_AnyTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.Any)]
class TestBehaviour0 : UdonSharpBehaviour
{
    [UdonSynced(UdonSyncMode.Smooth)]
    private int _var;
}
");
    }

    [Fact]
    public async Task TestDiagnostic_ContinuousTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
class TestBehaviour0 : UdonSharpBehaviour
{
    [UdonSynced(UdonSyncMode.Smooth)]
    private int _var;
}
");
    }

    [Fact]
    public async Task TestDiagnostic_ManualTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
class TestBehaviour0 : UdonSharpBehaviour
{
    [UdonSynced(UdonSyncMode.Smooth)]
    private int _var;
}
");
    }

    [Fact]
    public async Task TestDiagnostic_NoneTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
class TestBehaviour0 : UdonSharpBehaviour
{
    [UdonSynced(UdonSyncMode.Smooth)]
    private int _var;
}
");
    }

    [Fact]
    public async Task TestDiagnostic_NoVariableSyncAndNoUdonSyncedTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
class TestBehaviour0 : UdonSharpBehaviour
{
    private int _var;
}
");
    }

    [Fact]
    [Example]
    public async Task TestDiagnostic_NoVariableSyncTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.Udon.Common.Interfaces;

[[|UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)|]]
class TestBehaviour0 : UdonSharpBehaviour
{
    [UdonSynced(UdonSyncMode.Smooth)]
    private int _var;
}
");
    }
}