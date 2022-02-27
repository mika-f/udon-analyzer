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

[Describe(typeof(DoesNotYetSupportOutParametersOnUserDefinedMethodsAnalyzer), "VSC")]
public class DoesNotYetSupportOutParametersOnUserDefinedMethodsAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotYetSupportOutParametersOnUserDefinedMethodsAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_MethodDeclarationHasOutParameterOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod([|out int a|], [|out int b|])
    {
        a = b = 1;
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_MethodDeclarationHasOutParameterOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{
    public void TestMethod(out int a, out int b)
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