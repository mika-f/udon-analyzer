// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Threading.Tasks;

using NatsunekoLaboratory.UdonAnalyzer.AnalyzerSpec.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.Testing;
using NatsunekoLaboratory.UdonAnalyzer.Udon;

using Xunit;

namespace Analyzers.Tests.Udon;

[Describe(typeof(DoesNotSupportThrowingExceptionsAnalyzer), "VRC")]
public class DoesNotSupportThrowingExceptionsAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotSupportThrowingExceptionsAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_ThrowExceptionStatementTest()
    {
        await VerifyAnalyzerAsync(@"
using System;

using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        [|throw new NotSupportedException();|]
    }
}
");
    }

    [Fact]
    public async Task TestDiagnostic_ThrowExceptionExpressionTest()
    {
        await VerifyAnalyzerAsync(@"
using System;

using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod() => [|throw new NotSupportedException()|];
}
");
    }
}