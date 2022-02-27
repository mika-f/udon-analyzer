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

[Describe(typeof(DoesNotYetSupportLabeledStatementsAnalyzer), "VSC")]
public class DoesNotYetSupportLabeledStatementsAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotYetSupportLabeledStatementsAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_LabeledStatementOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public int TestMethod()
    {
        goto label1;

[|label1:
        return 1;|]
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_LabeledStatementOnMonoBehaviour()
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