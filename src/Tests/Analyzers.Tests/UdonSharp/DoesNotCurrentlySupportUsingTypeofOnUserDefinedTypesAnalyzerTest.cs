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

[Describe(typeof(DoesNotCurrentlySupportUsingTypeofOnUserDefinedTypesAnalyzer), "VSC")]
public class DoesNotCurrentlySupportUsingTypeofOnUserDefinedTypesAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotCurrentlySupportUsingTypeofOnUserDefinedTypesAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_TypeofExpressionWithUserDefinedTypesOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod()
    {
        var t = [|typeof(TestBehaviour)|];
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_TypeofExpressionWithSystemDefinedTypeOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using UnityEngine;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod()
    {
        var t = typeof(Rigidbody);
    }
}

");
    }
}