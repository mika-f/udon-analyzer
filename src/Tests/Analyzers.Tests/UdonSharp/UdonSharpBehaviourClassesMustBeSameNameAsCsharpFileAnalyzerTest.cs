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

[Describe(typeof(UdonSharpBehaviourClassesMustBeSameNameAsCsharpFileAnalyzer), "VSC")]
public class UdonSharpBehaviourClassesMustBeSameNameAsCsharpFileAnalyzerTest : UdonSharpDiagnosticVerifier<UdonSharpBehaviourClassesMustBeSameNameAsCsharpFileAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_NoSameNameAsCSharpFileNameTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

[|class TestSource2 : UdonSharpBehaviour {}|]
");
    }

    [Fact]
    public async Task TestNoDiagnostic_SameNameAsCSharpFileNameTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestSource0 : UdonSharpBehaviour {}
");
    }
}