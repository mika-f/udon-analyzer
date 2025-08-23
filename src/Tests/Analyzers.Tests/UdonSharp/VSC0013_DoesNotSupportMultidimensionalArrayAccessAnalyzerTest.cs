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

[Describe(typeof(DoesNotSupportMultidimensionalArrayAccessAnalyzer), "VSC")]
public class DoesNotSupportMultidimensionalArrayAccessAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotSupportMultidimensionalArrayAccessAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_MultidimensionalArrayAccessTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        var arr = new int[1, 2];
        [|arr[0, 0]|] = 1;
    }
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_JaggedArrayAccessTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        var arr = new int[5][];
        arr[0] = new int[5];
        arr[0][0] = 1;
    }
}
");
    }
}