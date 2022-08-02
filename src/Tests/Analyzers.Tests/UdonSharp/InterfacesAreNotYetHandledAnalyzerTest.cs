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

[Describe(typeof(InterfacesAreNotYetHandledAnalyzer), "VSC")]
public class InterfacesAreNotYetHandledAnalyzerTest : UdonSharpDiagnosticVerifier<InterfacesAreNotYetHandledAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_ImplementInterface()
    {
        await VerifyAnalyzerAsync(@"
using System;

using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour, [|IDisposable|]
{
    public void Dispose() {}
}
");
    }

    [Fact]
    public async Task TestDiagnostic_ImplementInterfaces()
    {
        await VerifyAnalyzerAsync(@"
using System;

using UdonSharp;

class TestBehaviour0 : UdonSharpBehaviour, [|IDisposable|], [|ICloneable|]
{
    public void Dispose() {}

    public object Clone() => default;
}
");
    }
}