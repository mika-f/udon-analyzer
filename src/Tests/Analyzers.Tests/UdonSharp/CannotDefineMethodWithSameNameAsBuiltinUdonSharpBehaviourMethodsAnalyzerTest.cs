// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Threading.Tasks;

using NatsunekoLaboratory.UdonAnalyzer.AnalyzerSpec.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.Testing;
using NatsunekoLaboratory.UdonAnalyzer.UdonSharp;

using Xunit;

namespace Analyzers.Tests.UdonSharp;

[Describe(typeof(CannotDefineMethodWithSameNameAsBuiltinUdonSharpBehaviourMethodsAnalyzer), "VSC")]
public class CannotDefineMethodWithSameNameAsBuiltinUdonSharpBehaviourMethodsAnalyzerTest : UdonSharpDiagnosticVerifier<CannotDefineMethodWithSameNameAsBuiltinUdonSharpBehaviourMethodsAnalyzer>
{
    [Theory]
    [InlineData("SendCustomEvent")]
    [InlineData("SendCustomNetworkEvent")]
    [InlineData("SetProgramVariable")]
    [InlineData("GetProgramVariable")]
    [InlineData("VRCInstantiate")]
    [InlineData("GetUdonTypeID")]
    [InlineData("GetUdonTypeName")]
    public async Task TestDiagnostic_MethodDeclarationWithSameNameAsBuiltinUdonSharpMethodsOnUdonSharpBehaviour(string name)
    {
        await VerifyAnalyzerAsync(@$"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{{
    [|public void {name}() {{}}|@{name}]
}}
");
    }

    [Fact]
    [Example]
    public async Task TestDiagnostic_MethodDeclarationWithSameNameAsBuiltinUdonSharpMethodsOnUdonSharpBehaviourExample()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    [|public void SendCustomEvent() {}|@SendCustomEvent]
}
");
    }

    [Theory]
    [InlineData("SendCustomEvent")]
    [InlineData("SendCustomNetworkEvent")]
    [InlineData("SetProgramVariable")]
    [InlineData("GetProgramVariable")]
    [InlineData("VRCInstantiate")]
    [InlineData("GetUdonTypeID")]
    [InlineData("GetUdonTypeName")]
    public async Task TestNoDiagnostic_MethodDeclarationWithSameNameAsBuiltinUdonSharpMethodsOnMonoBehaviour(string name)
    {
        await VerifyAnalyzerAsync(@$"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{{
    public void {name}() {{}}
}}
");
    }
}