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

[Describe(typeof(DoesNotSupportInstantiatingNonGameObjectTypesAnalyzer), "VRC")]
public class DoesNotSupportInstantiatingNonGameObjectTypesAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotSupportInstantiatingNonGameObjectTypesAnalyzer>
{
    [Fact(Skip = "Skip this test because UdonSharpDiagnosticVerifier does not resolves some assemblies????")]
    [Example]
    public async Task TestDiagnostic_InstantiatingNonGameObjectTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using UnityEngine;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        [|Instantiate(new TrailRenderer())|];
    }
}
");
    }

    [Fact(Skip = "Skip this test because UdonSharpDiagnosticVerifier does not resolves some assemblies????")]
    public async Task TestDiagnostic_InstantiatingNullObjectTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using UnityEngine;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        [|Instantiate(null)|];
    }
}
");
    }

    [Fact(Skip = "Skip this test because UdonSharpDiagnosticVerifier does not resolves some assemblies????")]
    public async Task TestNoDiagnostic_InstantiatingGameObjectTest()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

using UnityEngine;

class TestBehaviour0 : UdonSharpBehaviour
{
    public void TestMethod()
    {
        Instantiate(new GameObject());
    }
}
");
    }
}