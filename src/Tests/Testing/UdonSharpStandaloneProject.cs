// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;

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

        if (File.Exists(variable))
        {
            using var sr = new StreamReader(variable);
            var document = new XPathDocument(sr);
            var navigator = document.CreateNavigator();
            var @namespace = new XmlNamespaceManager(navigator.NameTable);
            @namespace.AddNamespace("msbuild", "http://schemas.microsoft.com/developer/msbuild/2003");

            var node = navigator.Select("//msbuild:HintPath", @namespace);
            while (node.MoveNext())
                if (Path.IsPathRooted(node.Current!.Value))
                    yield return node.Current.Value;
                else
                    yield return Path.Combine(Path.GetDirectoryName(variable)!, node.Current!.Value);
        }

        var assemblies = Path.Combine(Path.GetDirectoryName(variable)!, "Library", "ScriptAssemblies");

        yield return Path.Combine(assemblies, "VRC.Udon.dll");
        yield return Path.Combine(assemblies, "UdonSharp.Runtime.dll");
    }
}