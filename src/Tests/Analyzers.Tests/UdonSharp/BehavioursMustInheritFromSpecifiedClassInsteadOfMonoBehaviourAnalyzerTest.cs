// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

using NatsunekoLaboratory.UdonAnalyzer.AnalyzerSpec.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.Testing;
using NatsunekoLaboratory.UdonAnalyzer.UdonSharp;

using Xunit;

namespace Analyzers.Tests.UdonSharp;

[Describe(typeof(BehavioursMustInheritFromSpecifiedClassInsteadOfMonoBehaviourAnalyzer), "VSC")]
public class BehavioursMustInheritFromSpecifiedClassInsteadOfMonoBehaviourAnalyzerTest : UdonSharpDiagnosticVerifier<BehavioursMustInheritFromSpecifiedClassInsteadOfMonoBehaviourAnalyzer>
{
    protected override ImmutableArray<string> FilteredDiagnosticIds => ImmutableArray.Create("CS0433");

    [Fact]
    public async Task TestNoDiagnostic_InheritFromMonoBehaviourWhenNotEnableWorkspaceAnalyzing()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{
}
");
    }


    [Fact]
    [Example]
    public async Task TestDiagnostic_InheritFromMonoBehaviourWhenEnableWorkspaceAnalyzing()
    {
        var editorconfig = new Dictionary<string, string>
        {
            { "udon_analyzer.enable_workspace_analyzing", "true" }
        };

        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : [|MonoBehaviour|]
{
}
", editorconfig);
    }
}