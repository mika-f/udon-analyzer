// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Threading.Tasks;

using NatsunekoLaboratory.UdonAnalyzer.AnalyzerSpec.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.Testing;
using NatsunekoLaboratory.UdonAnalyzer.Udon;

using Xunit;

namespace Analyzers.Tests.Udon
{
    [Describe(typeof(DoesNotCurrentlySupportSyncingOfTheTypeAnalyzer), "VRC")]
    public class DoesNotCurrentlySupportSyncingOfTheTypeAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotCurrentlySupportSyncingOfTheTypeAnalyzer>
    {
        [Fact]
        [Example]
        public async Task TestDiagnostic_CannotSyncVariableTypeTest()
        {
            await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    [|[UdonSynced]
    private TestBehaviour0 _some;|]
}
");
        }

        [Theory]
        [InlineData("bool")]
        [InlineData("char")]
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
        [InlineData("string")]
        [InlineData("Color")]
        [InlineData("Color32")]
        [InlineData("Vector2")]
        [InlineData("Vector3")]
        [InlineData("Vector4")]
        [InlineData("Quaternion")]
        [InlineData("VRCUrl")]
        [InlineData("bool[]")]
        [InlineData("char[]")]
        [InlineData("byte[]")]
        [InlineData("uint[]")]
        [InlineData("int[]")]
        [InlineData("long[]")]
        [InlineData("sbyte[]")]
        [InlineData("ulong[]")]
        [InlineData("float[]")]
        [InlineData("double[]")]
        [InlineData("short[]")]
        [InlineData("ushort[]")]
        [InlineData("string[]")]
        [InlineData("Color[]")]
        [InlineData("Color32[]")]
        [InlineData("Vector2[]")]
        [InlineData("Vector3[]")]
        [InlineData("Vector4[]")]
        [InlineData("Quaternion[]")]
        [InlineData("VRCUrl[]")]
        public async Task TestNoDiagnostic_CanSyncVariableTypesTest(string t)
        {
            await VerifyAnalyzerAsync($@"
using UdonSharp;

using UnityEngine;

using VRC.SDKBase;

class TestBehaviour0 : UdonSharpBehaviour
{{
    [UdonSynced]
    private {t} _some;
}}
");
        }

        [Fact]
        public async Task TestNoDiagnostic_NoSyncVariableTest()
        {
            await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    private TestBehaviour0 _some;
}
");
        }
    }
}