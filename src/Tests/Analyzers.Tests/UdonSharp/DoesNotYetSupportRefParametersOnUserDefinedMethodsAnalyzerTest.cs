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

[Describe(typeof(DoesNotYetSupportRefParametersOnUserDefinedMethodsAnalyzer), "VSC")]
public class DoesNotYetSupportRefParametersOnUserDefinedMethodsAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotYetSupportRefParametersOnUserDefinedMethodsAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_MethodDeclarationHasRefParameterOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod([|ref int a|], [|ref int b|])
    {
        a = b = 1;
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_MethodDeclarationHasRefParameterOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{
    public void TestMethod(ref int a, ref int b)
    {
        a = b = 1;
    }
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