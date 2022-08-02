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

[Describe(typeof(UseTheNamespaceDeclarationToAvoidClassNameConflictsAnalyzer), "VSC")]
public class UseTheNamespaceDeclarationToAvoidClassNameConflictsAnalyzerTest : UdonSharpDiagnosticVerifier<UseTheNamespaceDeclarationToAvoidClassNameConflictsAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_ClassDeclarationWithoutNamespace()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

[|class TestBehaviour0 : UdonSharpBehaviour {}|]
");
    }

    [Fact]
    public async Task TestNoDiagnostic_ClassDeclarationWithFileScopedNamespace()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

namespace NatsunekoLaboratory;

class TestBehaviour0 : UdonSharpBehaviour {}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_ClassDeclarationWithNamespace()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

namespace NatsunekoLaboratory
{
    class TestBehaviour0 : UdonSharpBehaviour {}
}
");
    }
}