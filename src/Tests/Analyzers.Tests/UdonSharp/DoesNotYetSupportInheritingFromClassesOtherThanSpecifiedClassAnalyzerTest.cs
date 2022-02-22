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

[Describe(typeof(DoesNotYetSupportInheritingFromClassesOtherThanSpecifiedClassAnalyzer), "VSC")]
public class DoesNotYetSupportInheritingFromClassesOtherThanSpecifiedClassAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotYetSupportInheritingFromClassesOtherThanSpecifiedClassAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_InheritingFromClassesOtherThanSpecifiedClassWhenEnableWorkspaceAnalyzing()
    {
        var editorconfig = new Dictionary<string, string>
        {
            { "udon_analyzer.enable_workspace_analyzing", "true" }
        };

        await VerifyAnalyzerAsync(@"
class SomeBehaviour {}

class TestBehaviour : [|SomeBehaviour|] {}
", editorconfig);
    }

    [Fact]
    public async Task TestNoNoDiagnostic_InheritingFromClassesOtherThanSpecifiedClass()
    {
        await VerifyAnalyzerAsync(@"
class SomeBehaviour {}

class TestBehaviour : SomeBehaviour {}
");
    }

    [Fact]
    public async Task TestNoNoDiagnostic_InheritingFromUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour {}
");
    }
}