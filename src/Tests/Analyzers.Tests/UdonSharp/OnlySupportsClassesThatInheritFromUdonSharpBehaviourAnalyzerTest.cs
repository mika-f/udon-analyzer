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

[Describe(typeof(OnlySupportsClassesThatInheritFromUdonSharpBehaviourAnalyzer), "VSC")]
public class OnlySupportsClassesThatInheritFromUdonSharpBehaviourAnalyzerTest : UdonSharpDiagnosticVerifier<OnlySupportsClassesThatInheritFromUdonSharpBehaviourAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_ClassNotInheritSomeClasses()
    {
        var editorconfig = new Dictionary<string, string>
        {
            { "udon_analyzer.enable_workspace_analyzing", "true" }
        };

        await VerifyAnalyzerAsync(@"
[|class TestBehaviour {}|]
", editorconfig);
    }

    [Fact]
    public async Task TestNoDiagnostic_ClassInheritFromUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour {}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_ClassNotInheritSomeClasses()
    {
        await VerifyAnalyzerAsync(@"
class TestBehaviour {}
");
    }
}