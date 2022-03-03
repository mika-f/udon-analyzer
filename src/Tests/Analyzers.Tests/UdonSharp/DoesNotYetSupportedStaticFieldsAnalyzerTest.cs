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

[Describe(typeof(DoesNotYetSupportedStaticFieldsAnalyzer), "VSC")]
public class DoesNotYetSupportedStaticFieldsAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotYetSupportedStaticFieldsAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_StaticDeclarationOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    [|public static readonly string FooBar = ""a"";|]

    [|public static string _var;|]
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_StaticDeclarationOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{
    public static readonly string FooBar = ""a"";

    public static string _var;
}

");
    }
}