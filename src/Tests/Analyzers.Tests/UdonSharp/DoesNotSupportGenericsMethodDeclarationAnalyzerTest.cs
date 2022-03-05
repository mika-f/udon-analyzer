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

[Describe(typeof(DoesNotSupportGenericsMethodDeclarationAnalyzer), "VSC")]
public class DoesNotSupportGenericsMethodDeclarationAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotSupportGenericsMethodDeclarationAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_MethodDeclarationWithGenericsOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    [|void Test<T>() {}|]
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_MethodDeclarationWithGenericsOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{
    void Test<T>() {}
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_MethodDeclarationWithoutGenericsOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    void Test() {}
}
");
    }
}