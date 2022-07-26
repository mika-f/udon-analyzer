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

[Describe(typeof(TypeIsNotExposedToUdonAnalyzer), "VRC")]
public class TypeIsNotExposedToUdonAnalyzerTest : UdonSharpDiagnosticVerifier<TypeIsNotExposedToUdonAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_DisallowedTypeTest()
    {
        await VerifyAnalyzerAsync(@"
using System;

using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    private [|IntPtr|@System.IntPtr] _ptr;

    public [|IntPtr|@System.IntPtr] Ptr { get; set; }

    public [|IntPtr|@System.IntPtr] SomeMethod([|IntPtr|@System.IntPtr] ptr)
    {
        [|IntPtr|@System.IntPtr] a = IntPtr.Zero;
        return a;
    }
}
");
    }

    [Theory]
    [InlineData("UdonSharpBehaviour", "/* WHITELISTED */")]
    [InlineData("UdonSharpBehaviour[]", "/* WHITELISTED */")]
    [InlineData("TestBehaviour", "/* WHITELISTED */")]
    [InlineData("TestBehaviour[]", "/* WHITELISTED */")]
    [InlineData("int", "Type_SystemInt32")]
    [InlineData("VRCPlayerApi.TrackingDataType", "Type_VRCSDKBaseVRCPlayerApiTrackingDataType")]
    [InlineData("Type", "Type_SystemType")]
    public async Task TestNoDiagnostic_AllowedTypeTest(string t, string declaration)
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
    private {t} _variable;

    public {t} Property {{ get; set }}

    public {t} TestMethod({t} parameter)
    {{
        {t} variable = default; 
        return variable;
    }}
}}
", additionals);
    }

    [Fact]
    public async Task TestNoDiagnostic_SystemVoidTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod() {}
}
");
    }
}