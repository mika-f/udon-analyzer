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

[Describe(typeof(DoesNotYetSupportNamespaceAliasDirectivesAnalyzer), "VSC")]
public class DoesNotYetSupportNamespaceAliasDirectivesAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotYetSupportNamespaceAliasDirectivesAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_NamespaceAliasDirectiveOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
[|using US = UdonSharp;|]

class TestBehaviour : US.UdonSharpBehaviour {}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_NamespaceAliasDirectiveOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UE = UnityEngine;

class TestBehaviour : UE.MonoBehaviour {}
");
    }
}