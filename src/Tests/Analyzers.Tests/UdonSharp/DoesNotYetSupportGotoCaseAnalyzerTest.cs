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

[Describe(typeof(DoesNotYetSupportGotoCaseAnalyzer), "VSC")]
public class DoesNotYetSupportGotoCaseAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotYetSupportGotoCaseAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_GotoCaseStatementOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public int TestMethod()
    {
        avr i = 0;

        switch (i)
        {
            case 0:
                [|goto case 1;|]

            case 1:
                goto default;

            default:
                break;
        }
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_GotoCaseStatementOnMonoBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UnityEngine;

class TestBehaviour : MonoBehaviour
{
    public int TestMethod()
    {
        avr i = 0;

        switch (i)
        {
            case 0:
                goto case 1;

            case 1:
                goto default;

            default:
                break;
        }
    }

}
");
    }
}