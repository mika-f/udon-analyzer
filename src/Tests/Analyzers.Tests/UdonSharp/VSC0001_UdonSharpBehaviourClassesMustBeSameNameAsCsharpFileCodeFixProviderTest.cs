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

[Describe(typeof(UdonSharpBehaviourClassesMustBeSameNameAsCsharpFileCodeFixProvider), "VSC")]
public class UdonSharpBehaviourClassesMustBeSameNameAsCsharpFileCodeFixProviderTest : UdonSharpCodeFixVerifier<UdonSharpBehaviourClassesMustBeSameNameAsCsharpFileAnalyzer, UdonSharpBehaviourClassesMustBeSameNameAsCsharpFileCodeFixProvider>
{
    [Fact]
    [Example]
    public async Task TestCodeFix_UdonSharpBehaviourClassNameIsDifferentFromCsharpFileName()
    {
        await VerifyCodeFixAsync("""
                                 using UdonSharp;

                                 [|class TestBehaviour2 : UdonSharpBehaviour {}|]
                                 """,
                                 """
                                 using UdonSharp;

                                 class TestSource0 : UdonSharpBehaviour {}
                                 """
        );
    }

    [Fact]
    public async Task TestCodeFix_UdonSharpBehaviourClassNameIsDifferentFromCsharpFileNameWithReference()
    {
        await VerifyCodeFixAsync("""
                                 using UdonSharp;

                                 [|class TestBehaviour2 : UdonSharpBehaviour
                                 {
                                    private TestBehaviour2 _ref;
                                 }|]
                                 """,
                                 """
                                 using UdonSharp;

                                 class TestSource0 : UdonSharpBehaviour
                                 {
                                    private TestSource0 _ref;
                                 }
                                 """
        );
    }
}