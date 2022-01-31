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

[Describe(typeof(DoesNotSupportInheritingFromInterfacesAnalyzer), "VSC")]
public class DoesNotSupportInheritingFromInterfacesAnalyzerTest : UdonSharpDiagnosticVerifier<DoesNotSupportInheritingFromInterfacesAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_InheritingFromInterface()
    {
        await VerifyAnalyzerAsync(@"
using System;

using UdonSharp;

class TestBehaviour : UdonSharpBehaviour, [|IDisposable|]
{
    public void Dispose() {}
}
");
    }

    [Fact]
    public async Task TestDiagnostic_InheritingFromInterfaces()
    {
        await VerifyAnalyzerAsync(@"
using System;

using UdonSharp;

class TestBehaviour : UdonSharpBehaviour, [|IDisposable|], [|ICloneable|]
{
    public void Dispose() {}

    public object Clone()
    {
        return null;
    }
}
");
    }
}