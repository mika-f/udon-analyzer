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

[Describe(typeof(DoesNotSupportGotoCaseStatementAnalyzer), "VSC")]
public class DoesNotSupportGotoCaseStatementAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotSupportGotoCaseStatementAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_GotoCaseStatement()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using UnityEngine;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod(int i)
    {
        switch (i)
        {
            case 0:
                Debug.Log(""zero"");
                [|goto case 1;|]

            case 1:
                break;
        }
    }
}
");
    }
}