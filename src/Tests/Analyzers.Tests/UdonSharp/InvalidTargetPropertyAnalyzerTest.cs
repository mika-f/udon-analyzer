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

[Describe(typeof(InvalidTargetPropertyAnalyzer), "VSC")]
public class InvalidTargetPropertyAnalyzerTest : UdonSharpDiagnosticVerifier<InvalidTargetPropertyAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_InvalidTargetPropertyOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    [|[FieldChangeCallback(""MissingProperty"")]
    public string _foo;|@MissingProperty]
}
");
    }

    [Fact]
    [Example]
    public async Task TestNoDiagnostic_ValidTargetPropertyOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    [FieldChangeCallback(""SomeProperty"")]
    public string _foo;

    public string SomeProperty { get; set; }
}
");
    }
}