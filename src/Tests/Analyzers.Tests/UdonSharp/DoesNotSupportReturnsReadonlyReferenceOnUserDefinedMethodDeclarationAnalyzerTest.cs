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

[Describe(typeof(DoesNotSupportReturnsReadonlyReferenceOnUserDefinedMethodDeclarationAnalyzer), "VSC")]
public class DoesNotSupportReturnsReadonlyReferenceOnUserDefinedMethodDeclarationAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotSupportReturnsReadonlyReferenceOnUserDefinedMethodDeclarationAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_MethodDeclarationHasReadonlyReferenceReturnsOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    private readonly string[] _arr;

    public [|ref readonly string|] TestMethod()
    {
        return ref _arr[0];
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
    private readonly string[] _arr;

    public ref string TestMethod()
    {
        return ref _arr[0];
    }
}
");
    }

    [Fact]
    [Example]
    public async Task TestNoDiagnostic_MethodDeclarationHasReadonlyReferenceReturnsOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{
    private readonly string[] _arr;

    public ref readonly string TestMethod()
    {
        return ref _arr[0];
    }
}
");
    }
}