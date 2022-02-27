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

[Describe(typeof(DoesNotCurrentlySupportStaticMethodDeclarationsAnalyzer), "VSC")]
public class DoesNotCurrentlySupportStaticMethodDeclarationsAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotCurrentlySupportStaticMethodDeclarationsAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_StaticMethodDeclarationOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    [|public static void TestMethod() {}|]
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_StaticMethodDeclarationOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{
    public static void TestMethod() {}
}
");
    }
}