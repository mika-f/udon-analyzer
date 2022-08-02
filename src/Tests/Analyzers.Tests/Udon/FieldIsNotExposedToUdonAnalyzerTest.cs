// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

using NatsunekoLaboratory.UdonAnalyzer.AnalyzerSpec.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.Testing;
using NatsunekoLaboratory.UdonAnalyzer.Udon;

using Xunit;

namespace Analyzers.Tests.Udon;

[Describe(typeof(FieldIsNotExposedToUdonAnalyzer), "VRC")]
public class FieldIsNotExposedToUdonAnalyzerTest : UdonSharpDiagnosticVerifier<FieldIsNotExposedToUdonAnalyzer>
{
    [Theory]
    [InlineData("VideoError.Unknown", "Type_VRCSDK3ComponentsVideoVideoError")]
    [InlineData("NetworkEventTarget.All", "Type_VRCUdonCommonInterfacesNetworkEventTarget")]
    [InlineData("_tm.text", "TMProTextMeshProUGUI.__get_text__SystemString")]
    [InlineData("_behaviour1.gameObject", "/* WHITELISTED */")]
    [InlineData("_behaviour2.gameObject", "/* WHITELISTED */")]
    [InlineData("_constraint.gameObject", "UnityEngineAnimationsScaleConstraint.__get_gameObject__UnityEngineGameObject")]
    [InlineData("_arr.Length", "SystemInt32Array.__get_Length__SystemInt32")]
    [InlineData("_arr2.Length", "SystemInt32Array.__get_Length__SystemInt32")]
    [InlineData("Vector3.one", "UnityEngineVector3.__get_one__UnityEngineVector3")]
    [InlineData("Vector3.one == Vector3.one", "UnityEngineVector3.__get_one__UnityEngineVector3")]
    [InlineData("_pickup.InteractionText", "VRCSDK3ComponentsVRCPickup.__get_InteractionText__SystemString")]
    public async Task TestNoDiagnostic_AllowedGetterContextFieldOnUdonSharpBehaviour(string access, string declaration)
    {
        var additionals = new List<(string Filename, string Content)>
        {
            ("PublicAPI.Shipped.test.txt", declaration)
        };


        await VerifyAnalyzerAsync(@$"
using System;

using TMPro;

using UdonSharp;

using UnityEngine;
using UnityEngine.Animations;

using VRC.Udon.Common.Interfaces;
using VRC.SDKBase;
using VRC.SDK3.Components.Video;
using VRC.Udon;

class TestBehaviour : UdonSharpBehaviour
{{
    private TextMeshProUGUI _tm;
    private UdonSharpBehaviour _behaviour1;
    private UdonBehaviour _behaviour2;
    private ScaleConstraint _constraint;
    private int[] _arr;
    private int[][] _arr2;
    private VRC_Pickup _pickup;
    
    public void TestMethod()
    {{
        var _ = {access};
    }}
}}
", additionals);
    }

    [Fact]
    [Example]
    public async Task TestDiagnostic_DisallowedFieldSymbol()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using UnityEngine;

class TestBehaviour0 : UdonSharpBehaviour
{
    [SerializedField]
    private ParticleSystemForceField _field;

    private void TestMethod()
    {
        var @delegate = [|_field.gameObject|@UnityEngine.Component.gameObject];
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_UserDefinedVariableOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    private string _str;

    public void TestMethod()
    {
        var _ = this._str;
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_UserDefinedMethodInOtherBehaviourOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    private SomeBehaviour _behaviour;

    public void TestMethod()
    {
        var _ = _behaviour.SomeProperty;
    }
}

class SomeBehaviour : UdonSharpBehaviour
{
    public string SomeProperty { get; set; }
}
");
    }
}