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

[Describe(typeof(DoesNotYetSupportGotoAnalyzer), "VSC")]
public class DoesNotYetSupportGotoAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotYetSupportGotoAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_GotoStatementOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public int TestMethod()
    {
        [|goto label1;|]

label1:
        return 1;
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_GotoStatementOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{
    public int TestMethod()
    {
        goto label1;

label1:
        return 1;
    }
}
");
    }
}