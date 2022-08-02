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

[Describe(typeof(DoesNotSupportLinearInterpolationOfTheSyncedTypeAnalyzer), "VRC")]
public class DoesNotSupportLinearInterpolationOfTheSyncedTypeAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotSupportLinearInterpolationOfTheSyncedTypeAnalyzer>
{
    [Theory]
    [InlineData("byte")]
    [InlineData("uint")]
    [InlineData("int")]
    [InlineData("long")]
    [InlineData("sbyte")]
    [InlineData("ulong")]
    [InlineData("float")]
    [InlineData("double")]
    [InlineData("short")]
    [InlineData("ushort")]
    [InlineData("Color")]
    [InlineData("Color32")]
    [InlineData("Vector2")]
    [InlineData("Vector3")]
    [InlineData("Quaternion")]
    public async Task TestNoDiagnostic_SupportedLinearSyncedTypeTest(string t)
    {
        await VerifyAnalyzerAsync($@"
using UdonSharp;

using UnityEngine;

class TestBehaviour0 : UdonSharpBehaviour
{{
    [UdonSynced(UdonSyncMode.Linear)]
    private {t} _str;
}}
");
    }

    [Fact]
    [Example]
    public async Task TestDiagnostic_NotSupportedLinearSyncedTypeTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    [|[UdonSynced(UdonSyncMode.Linear)]
    private string _str;|@string]
}
");
    }
}