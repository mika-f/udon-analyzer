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

[Describe(typeof(DoesNotYetSupportInParametersOnUserDefinedMethodsAnalyzer), "VSC")]
public class DoesNotYetSupportInParametersOnUserDefinedMethodsAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotYetSupportInParametersOnUserDefinedMethodsAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_MethodDeclarationHasInParameterOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod([|in int a|], [|in int b|]) { }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_MethodDeclarationHasInParameterOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{
    public void TestMethod(in int a, in int b) { }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_MethodDeclarationOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod(int a, int b) {}
}
");
    }
}