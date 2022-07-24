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

[Describe(typeof(TargetPropertyForFieldChangeCallbackAttributeWasNotFoundAnalyzer), "VSC")]
public class TargetPropertyForFieldChangeCallbackAttributeWasNotFoundAnalyzerTest : UdonSharpDiagnosticVerifier<TargetPropertyForFieldChangeCallbackAttributeWasNotFoundAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_TargetPropertyWasNotFoundTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

public class TestBehaviour : UdonSharpBehaviour
{
    [[|FieldChangeCallback(""MissingProperty"")|]]
    private string _str1;
}
");
    }

    [Fact]
    public async Task TestDiagnostic_TargetPropertyWasNotFoundButFieldWasFoundTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

public class TestBehaviour : UdonSharpBehaviour
{
    [[|FieldChangeCallback(""MissingProperty"")|]]
    private string _str1;

    private string MissingProperty;
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_TargetPropertyWasFoundTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

public class TestBehaviour : UdonSharpBehaviour
{
    [FieldChangeCallback(""MissingProperty"")]
    private string _str1;

    public string MissingProperty { get; set; }
}
");
    }
}