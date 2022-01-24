// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

namespace NatsunekoLaboratory.UdonAnalyzer.Testing;

public class UdonSharpStandaloneProject : UnityStandaloneProject
{
    protected override IEnumerable<string> ExternalReferences()
    {
        foreach (var reference in base.ExternalReferences())
            yield return reference;

        foreach (var assembly in FindUdonSharpAssemblies())
            yield return assembly;
    }

    private static IEnumerable<string> FindUdonSharpAssemblies()
    {
        var variable = Environment.GetEnvironmentVariable("UDON_ANALYZER_TARGET_PROJECT");
        if (string.IsNullOrWhiteSpace(variable))
            throw new ArgumentNullException(variable);

        var assemblies = Path.Combine(variable, "Library", "ScriptAssemblies");

        yield return Path.Combine(assemblies, "VRC.Udon.dll");
        yield return Path.Combine(assemblies, "UdonSharp.Runtime.dll");
    }
}