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

[Describe(typeof(NullableTypesAreNotCurrentlySupportedAnalyzer), "VSC")]
public class NullableTypesAreNotCurrentlySupportedAnalyzerTest : UdonSharpDiagnosticVerifier<NullableTypesAreNotCurrentlySupportedAnalyzer>
{
    [Fact]
    public async Task TestDiagnostic_NonNullableTypeDeclaration()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public int _field;

    public int Property { get; set; }

    public int Method(int args) { }
}
");
    }

    [Fact]
    [Example]
    public async Task TestDiagnostic_NullableTypeDeclaration()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public [|int?|] _field;

    public [|int?|] Property { get; set; }

    public [|int?|] Method([|int?|] args) { }
}
");
    }
}