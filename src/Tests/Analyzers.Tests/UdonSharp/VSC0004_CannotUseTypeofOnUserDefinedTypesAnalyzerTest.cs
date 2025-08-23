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

[Describe(typeof(CannotUseTypeofOnUserDefinedTypesAnalyzer), "VSC")]
public class CannotUseTypeofOnUserDefinedTypesAnalyzerTest : UdonSharpDiagnosticVerifier<CannotUseTypeofOnUserDefinedTypesAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_TypeOfOnUserDefinedTypes()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        var t = [|typeof(TestBehaviour0)|];
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_TypeOfOnUnityEnginePredefinedTypes()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using VRC.SDKBase;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        var t = typeof(VRCPlayerApi);
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_TypeOfOnVRChatPredefinedTypes()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        var t = typeof(GameObject);
    }
}
");
    }
}