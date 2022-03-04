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

[Describe(typeof(TypesMustMatchBetweenPropertyAndVariableChangeFieldAnalyzer), "VSC")]
public class TypesMustMatchBetweenPropertyAndVariableChangeFieldAnalyzerTest : UdonSharpDiagnosticVerifier<TypesMustMatchBetweenPropertyAndVariableChangeFieldAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_TypesDoesNotMatchBetweenFieldAndPropertyOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    [|[FieldChangeCallback(nameof(Foo))]
    public string _foo;|]

    public int Foo { get; set; }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_TypesMatchBetweenFieldAndPropertyOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    [FieldChangeCallback(nameof(Foo))]
    public string _foo;

    public string Foo { get; set; }
}
");
    }
}