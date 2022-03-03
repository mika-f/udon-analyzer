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

[Describe(typeof(OnlyOneFieldMayTargetPropertyOnThisPropertyAnalyzer), "VSC")]
public class OnlyOneFieldMayTargetPropertyOnThisPropertyAnalyzerTest : UdonSharpDiagnosticVerifier<OnlyOneFieldMayTargetPropertyOnThisPropertyAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_MultipleFieldSpecifiedFieldChangeCallbackToSameProperty()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    [|[FieldChangeCallback(nameof(SomeProperty))]
    private string _foo;|@SomeProperty]

    [|[FieldChangeCallback(""SomeProperty"")]
    private string _bar;|@SomeProperty]

    public string SomeProperty { get; set; }
}

");
    }

    [Fact]
    public async Task TestNoDiagnostic_MultipleFieldSpecifiedFieldChangeCallbackToAnotherProperty()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    [FieldChangeCallback(nameof(SomeProperty))]
    private string _foo;

    [FieldChangeCallback(nameof(AnotherProperty))]
    private string _bar;

    public string SomeProperty { get; set; }

    public string AnotherProperty { get; set; }

}

");
    }
}