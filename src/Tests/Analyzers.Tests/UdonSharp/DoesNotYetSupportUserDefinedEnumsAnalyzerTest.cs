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

[Describe(typeof(DoesNotYetSupportUserDefinedEnumsAnalyzer), "VSC")]
public class DoesNotYetSupportUserDefinedEnumsAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotYetSupportUserDefinedEnumsAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_EnumDeclarationWhenEnableWorkspaceAnalyzing()
    {
        var editorconfig = new Dictionary<string, string>
        {
            { "udon_analyzer.enable_workspace_analyzing", "true" }
        };

        await VerifyAnalyzerAsync(@"
[|enum SomeEnum {}|]
", editorconfig);
    }

    [Fact]
    public async Task TestNoNoDiagnostic_EnumDeclaration()
    {
        await VerifyAnalyzerAsync(@"
enum SomeEnum {}
");
    }
}