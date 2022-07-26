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

[Describe(typeof(MethodIsNotExposedToUdonAnalyzer), "VRC")]
public class MethodIsNotExposedToUdonAnalyzerTest : UdonSharpDiagnosticVerifier<MethodIsNotExposedToUdonAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_DisallowedSymbolMethod()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using UnityEngine;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        [|GetComponent<Rigidbody>()|@UnityEngine.Component.GetComponent<UnityEngine.Rigidbody>()];
    }
}
");
    }

    [Theory]
    [InlineData("RequestSerialization", "VRCUdonCommonInterfacesIUdonEventReceiver.__RequestSerialization__SystemVoid")]
    [InlineData("GetProgramVariable(\"SomeMethod\")", "VRCUdonCommonInterfacesIUdonEventReceiver.__GetProgramVariable__SystemString__SystemObject")]
    [InlineData("name.ToString()", "SystemString.__ToString__SystemString")]
    [InlineData("_ps.Play()", "UnityEngineParticleSystem.__Play__SystemVoid")]
    [InlineData("new StopWatch()", "SystemDiagnosticsStopwatch.__ctor____SystemDiagnosticsStopwatch")]
    [InlineData("VideoError.Unknown.ToString()", "VRCSDK3ComponentsVideoVideoError.__ToString__SystemString")]
    [InlineData("_err.ToString()", "VRCSDK3ComponentsVideoVideoError.__ToString__SystemString")]
    [InlineData("transform.GetComponentsInChildren<Transform>()", "UnityEngineTransform.__GetComponentsInChildren__TArray")]
    [InlineData("_pickup.Drop()", "VRCSDK3ComponentsVRCPickup.__Drop__SystemVoid")]
    public async Task TestNoDiagnostic_AllowedMethodOnUdonSharpBehaviour(string invocation, string declaration)
    {
        var additionals = new List<(string Filename, string Content)>
        {
            ("PublicAPI.Shipped.test.txt", declaration)
        };

        await VerifyAnalyzerAsync(@$"
using System;

using UdonSharp;

using UnityEngine;

using VRC.SDKBase;
using VRC.SDK3.Components.Video;

class TestBehaviour : UdonSharpBehaviour
{{
    private ParticleSystem _ps;
    private VideoError _err;
    private VRC_Pickup _pickup;

    public void TestMethod()
    {{
        {invocation};
    }}
}}
", additionals);
    }

    [Fact]
    public async Task TestNoDiagnostic_UserDefinedMethodOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod()
    {
        SomeMethod();
    }

    public void SomeMethod() {}
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
        _behaviour.TestMethod();
    }
}

class SomeBehaviour : UdonSharpBehaviour
{
    public void TestMethod() {}
}
");
    }
}