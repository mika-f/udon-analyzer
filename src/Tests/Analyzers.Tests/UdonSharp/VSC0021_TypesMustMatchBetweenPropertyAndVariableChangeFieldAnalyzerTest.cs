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
    public async Task TestDiagnostic_TypesAreMismatchBetweenFieldAndPropertyTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

public class TestBehaviour : UdonSharpBehaviour
{
    [[|FieldChangeCallback(""SomeProperty"")|@int]]
    private string _str1;

    private int SomeProperty { get; set; }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_TypesAreMatchBetweenFieldAndPropertyTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

public class TestBehaviour : UdonSharpBehaviour
{
    [FieldChangeCallback(""SomeProperty"")]
    private string _str1;

    private string SomeProperty { get; set; }
}
");
    }
}