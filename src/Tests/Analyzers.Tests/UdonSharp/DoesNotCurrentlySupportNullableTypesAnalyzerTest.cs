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

[Describe(typeof(DoesNotCurrentlySupportNullableTypesAnalyzer), "VSC")]
public class DoesNotCurrentlySupportNullableTypesAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotCurrentlySupportNullableTypesAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_NullableTypeDeclarationOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    private [|short?|] _a = null;

    public [|int?|] TestMethod()
    {
        [|long?|] a = null;
        return default;
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_NullableTypeDeclarationOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{
    private short? _a = null;

    public int? TestMethod()
    {
        long? a = null;
        return default;
    }
}
");
    }
}