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

[Describe(typeof(DoesNotCurrentlySupportNullConditionalOperatorsAnalyzer), "VSC")]
public class DoesNotCurrentlySupportNullConditionalOperatorsAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotCurrentlySupportNullConditionalOperatorsAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_NullConditionalOperatorOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod()
    {
        string a = null;
        string b = [|a?.ToString()|];
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_NullConditionalOperaotrOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{
    public void TestMethod()
    {
        string a = null;
        string b = a?.ToString();
    }
}
");
    }
}