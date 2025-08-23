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

[Describe(typeof(DuplicateFieldChangeCallbackTargetAnalyzer), "VSC")]
public class DuplicateFieldChangeCallbackTargetAnalyzerTest : UdonSharpDiagnosticVerifier<DuplicateFieldChangeCallbackTargetAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_DuplicatedTargetProperty()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    [[|FieldChangeCallback(nameof(SomeProperty))|]]
    private string _str1;

    [[|FieldChangeCallback(""SomeProperty"")|]]
    private string _str2;

    public string SomeProperty { get; set; }

}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_DistinctedTargetProperty()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    [FieldChangeCallback(nameof(SomeProperty1))]
    private string _str1;

    [FieldChangeCallback(""SomeProperty2"")]
    private string _str2;

    public string SomeProperty1 { get; set; }
    public string SomeProperty2 { get; set; }

}
");
    }
}