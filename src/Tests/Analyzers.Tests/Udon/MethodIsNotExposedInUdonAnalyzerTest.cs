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

[Describe(typeof(MethodIsNotExposedInUdonAnalyzer), "VRC")]
public class MethodIsNotExposedInUdonAnalyzerTest : UdonSharpDiagnosticVerifier<MethodIsNotExposedInUdonAnalyzer>
{
    [Fact]
    [Example]
    public async Task TestDiagnostic_DisallowedMethodOnUdonSharpBehaviour()
    {
        AddAdditionalFile("PublicAPI.Shipped.test.txt", "");

        await VerifyAnalyzerAsync(@"
using UdonSharp;

using UnityEngine;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod()
    {
        [|GetComponent<Rigidbody>()|];
    }
}
");
    }

    [Theory]
    [InlineData("GetComponent<Rigidbody>()", "M:UnityEngine.Component.GetComponent``1~``0")]
    [InlineData("Invoke(\"SomeMethod\")", "M:UnityEngine.MonoBehaviour.Invoke(System.String)")]
    [InlineData("name.ToString()", "M:System.String.ToString~System.String")]
    public async Task TestNoDiagnostic_AllowedMethodOnUdonSharpBehaviour(string invocation, string declaration)
    {
        AddAdditionalFile("PublicAPI.Shipped.test.txt", declaration);

        await VerifyAnalyzerAsync(@$"
using UdonSharp;

using UnityEngine;

class TestBehaviour : UdonSharpBehaviour
{{
    public void TestMethod()
    {{
        {invocation};
    }}
}}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_UserDefinedMethodOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    public void TestMethod()
    {
        SomeMethod();
    }

    public void SomeMethod() {}
}
");
    }

    [Fact]
    public async Task TestNoDiagnostic_UserDefinedMethodInOtherBehaviourOnUdonSharpBehaviour()
    {
        await VerifyAnalyzerAsync(@"
using UdonSharp;

class TestBehaviour : UdonSharpBehaviour
{
    private SomeBehaviour _behaviour;

    public void TestMethod()
    {
        _behaviour.TestMethod();
    }
}

class SomeBehaviour : UdonSharpBehaviour
{
    public void TestMethod() {}
}
");
    }
}