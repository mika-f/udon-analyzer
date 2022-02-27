// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

using NatsunekoLaboratory.UdonAnalyzer.AnalyzerSpec.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.Testing;
using NatsunekoLaboratory.UdonAnalyzer.UdonSharp;

using Xunit;

namespace Analyzers.Tests.UdonSharp;

[Describe(typeof(OnlyOneClassDeclarationPerFileIsCurrentlySupportedAnalyzer), "VSC")]
public class OnlyOneClassDeclarationPerFileIsCurrentlySupportedAnalyzerTest : UdonSharpDiagnosticVerifier<OnlyOneClassDeclarationPerFileIsCurrentlySupportedAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_MultipleClassDeclarationsOnSameFileOnUdonSharpBehaviour_AllClassInheritedFromUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour1 : UdonSharpBehaviour {}

[|class TestBehaviour2 : UdonSharpBehaviour {}|]
");
    }

    [Fact]
    public async Task TestDiagnostic_MultipleClassDeclarationsOnSameFileOnUdonSharpBehaviour_FirstClassInheritedFromUdonSharpBehaviour()
    {
        var editorconfig = new Dictionary<string, string>
        {
            { "udon_analyzer.enable_workspace_analyzing", "true" }
        };

        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour1 : UdonSharpBehaviour {}

[|class TestBehaviour2 {}|]
", editorconfig);
    }

    [Fact]
    public async Task TestDiagnostic_MultipleClassDeclarationsOnSameFileOnUdonSharpBehaviour_LastClassInheritedFromUdonSharpBehaviour()
    {
        var editorconfig = new Dictionary<string, string>
        {
            { "udon_analyzer.enable_workspace_analyzing", "true" }
        };

        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour1 {}

[|class TestBehaviour2: UdonSharpBehaviour {}|]
", editorconfig);
    }

    [Fact]
    public async Task TestNoDiagnostic_MultipleClassDeclarationOnSameFile()
    {
        await VerifyAnalyzerAsync(@"
class TestBehaviour1 {}

class TestBehaviour2 {}
");
    }
}