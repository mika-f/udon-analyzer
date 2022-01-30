// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Immutable;
using System.Threading.Tasks;

using NatsunekoLaboratory.UdonAnalyzer.AnalyzerSpec.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.Testing;
using NatsunekoLaboratory.UdonAnalyzer.Udon;

using Xunit;

namespace Analyzers.Tests.Udon;

[Describe(typeof(TryCatchFinallyIsNotSupportedAnalyzer), "VRC")]
public class TryCatchFinallyIsNotSupportedAnalyzerTest : UdonSharpDiagnosticVerifier<TryCatchFinallyIsNotSupportedAnalyzer>
{
    protected override ImmutableArray<string> FilteredDiagnosticIds => ImmutableArray.Create("CS0168");

    [Fact]
    public async Task TestDiagnostic_TryCatchFinallyAsync()
    {
        await VerifyAnalyzerAsync(@"
using System;

using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public void Test()
    {
        [|try
        {
        }
        catch (Exception)
        {
            // ignored
        }
        finally
        {
        }|]
    }
}
");
    }

    [Fact]
    public async Task TestDiagnostic_TryCatchWithCatchDeclarationAsync()
    {
        await VerifyAnalyzerAsync(@"
using System;

using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public void Test()
    {
        [|try
        {
        }
        catch (Exception e)
        {
            // ignored
        }|]
    }
}
");
    }

    [Fact]
    [Example]
    public async Task TestDiagnostic_TryCatchWithoutCatchDeclarationAsync()
    {
        await VerifyAnalyzerAsync(@"
using System;

using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public void Test()
    {
        [|try
        {
        }
        catch (Exception)
        {
            // ignored
        }|]
    }
}
");
    }
}