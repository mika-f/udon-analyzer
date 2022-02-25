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

[Describe(typeof(DefaultLiteralExpressionsAreNotYetSupportedAnalyzer), "VSC")]
public class DefaultLiteralExpressionsAreNotYetSupportedAnalyzerTest : UdonSharpDiagnosticVerifier<DefaultLiteralExpressionsAreNotYetSupportedAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_DefaultLiteralExpressionOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public int SomeMethod()
    {
        return [|default|];
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_DefaultExpressionOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{
    public int SomeMethod()
    {
        return default;
    }
}
");
    }
}