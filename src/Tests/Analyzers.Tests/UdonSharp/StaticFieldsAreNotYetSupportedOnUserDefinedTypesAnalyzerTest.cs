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

[Describe(typeof(StaticFieldsAreNotYetSupportedOnUserDefinedTypesAnalyzer), "VSC")]
public class StaticFieldsAreNotYetSupportedOnUserDefinedTypesAnalyzerTest : UdonSharpDiagnosticVerifier<StaticFieldsAreNotYetSupportedOnUserDefinedTypesAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_StaticFieldDeclarationsTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    [|public static string _field;|]

    [|public static string Property { get; set; }|]
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_InstanceFieldDeclarationTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public string Field;
}
");
    }

    [Fact]
    public async Task TestDiagnostic_InstancePropertyDeclarationTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public string Property { get; set; }
}
");
    }
}