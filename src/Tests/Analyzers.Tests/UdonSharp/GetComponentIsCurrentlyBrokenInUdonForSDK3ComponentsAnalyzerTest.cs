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

[Describe(typeof(GetComponentIsCurrentlyBrokenInUdonForSDK3ComponentsAnalyzer), "VSC")]
// ReSharper disable once InconsistentNaming
public class GetComponentIsCurrentlyBrokenInUdonForSDK3ComponentsAnalyzerTest : UdonSharpDiagnosticVerifier<GetComponentIsCurrentlyBrokenInUdonForSDK3ComponentsAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_GetComponentsForBrokenComponentOnUdonSharpBehaviourExample()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.SDK3.Video.Components;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod()
    {
        var component = [|GetComponent<VRCUnityVideoPlayer>()|];
    }
}
");
    }

    [Theory]
    [InlineData("VRCAvatarPedestal")]
    [InlineData("VRCPickup")]
    [InlineData("VRCPortalMarker")]
    [InlineData("VRCStation")]
    [InlineData("VRCObjectPool")]
    [InlineData("VRCObjectSync")]
    public async Task TestDiagnostic_GetComponentsForBrokenComponentOnUdonSharpBehaviour(string t)
    {
        await VerifyAnalyzerAsync(@$"
using UdonSharp;

using VRC.SDK3.Components;

class TestBehaviour : UdonSharpBehaviour
{{
    public void TestMethod()
    {{
        var component = [|GetComponent<{t}>()|];
    }}
}}
");
    }

    [Theory]
    [InlineData("VRCAvatarPedestal")]
    [InlineData("VRCPickup")]
    [InlineData("VRCPortalMarker")]
    [InlineData("VRCStation")]
    [InlineData("VRCObjectPool")]
    [InlineData("VRCObjectSync")]
    public async Task TestNoDiagnostic_GetComponentsForBrokenComponentOnMonoBehaviour(string t)
    {
        await VerifyAnalyzerAsync(@$"
using UnityEngine;

using VRC.SDK3.Components;

class TestBehaviour : MonoBehaviour
{{
    public void TestMethod()
    {{
        var component = GetComponent<{t}>();
    }}
}}
");
    }

    [Theory]
    [InlineData("Rigidbody")]
    [InlineData("Transform")]
    public async Task TestNoDiagnostic_GetComponentsForNotBrokenComponentOnUdonSharpBehaviour(string t)
    {
        await VerifyAnalyzerAsync(@$"
using UdonSharp;

using UnityEngine;

class TestBehaviour : UdonSharpBehaviour
{{
    public void TestMethod()
    {{
        var component = GetComponent<{t}>();
    }}
}}
");
    }
}