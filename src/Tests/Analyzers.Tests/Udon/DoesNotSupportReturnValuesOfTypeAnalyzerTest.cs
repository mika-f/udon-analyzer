// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

using NatsunekoLaboratory.UdonAnalyzer.AnalyzerSpec.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.Testing;
using NatsunekoLaboratory.UdonAnalyzer.Udon;

using Xunit;

namespace Analyzers.Tests.Udon;

[Describe(typeof(DoesNotSupportReturnValuesOfTypeAnalyzer), "VRC")]
public class DoesNotSupportReturnValuesOfTypeAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotSupportReturnValuesOfTypeAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_DisallowedReturnTypeOnMethodDeclarationOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    [|public void TestMethod() {}|]
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_AllowedReturnTypeOnMethodDeclarationOnUdonSharpBehaviour()
    {
        var additionals = new List<(string Filename, string Content)>
        {
            ("PublicAPI.Shipped.test.txt", "T:System.Void")
        };


        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod() {}
}
", additionals);
    }
}