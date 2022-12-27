// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// -------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis.Diagnostics;

using NatsunekoLaboratory.UdonAnalyzer.Extensions;

namespace NatsunekoLaboratory.UdonAnalyzer.Models;

public static class CSharpSolutionContext
{
    private const string UdonRuntimeVersionGuid = "067f9b5cc16a52649985a5947e355556";
    private const string UdonSharpCompilerVersionGuid = "cbbe64479c0543f45bdf2fde11738ac2";

    // ReSharper disable once InconsistentNaming
    private const string SDKAssemblyName = "VRC.Udon.Wrapper.dll";

    private static string? _udonRuntimeVersion;
    private static string? _udonSharpCompilerVersion;

    private static readonly string DefaultVersion = new GenericVersion("0.0.0").ToString();
    private static readonly Regex VersionRegex = new(@"""version""\s*:\s*""(.*?)""", RegexOptions.Compiled);

    public static string FetchUdonRuntimeVersion(SyntaxNodeAnalysisContext context)
    {
        if (string.IsNullOrWhiteSpace(_udonRuntimeVersion))
        {
            var references = context.Compilation.ExternalReferences.Select(w => w.ToFilePath()).Where(w => !string.IsNullOrWhiteSpace(w)).Cast<string>().ToList();
            if (HasVRChatSDKAssembly(references, out var path))
            {
                var paths = FindUnityRootDirectory(path);
                if (TryReadSpecifiedGuidFileAsJsonStringFromPaths(paths, new[] { UdonRuntimeVersionGuid }, out var version))
                    _udonRuntimeVersion = version;
            }
        }

        return _udonRuntimeVersion ?? DefaultVersion;
    }

    public static string FetchUdonSharpCompilerVersion(SyntaxNodeAnalysisContext context)
    {
        if (string.IsNullOrWhiteSpace(_udonSharpCompilerVersion))
        {
            var references = context.Compilation.ExternalReferences.Select(w => w.ToFilePath()).Where(w => !string.IsNullOrWhiteSpace(w)).Cast<string>().ToList();
            if (HasVRChatSDKAssembly(references, out var path))
            {
                var paths = FindUnityRootDirectory(path);
                if (TryReadSpecifiedGuidFileAsJsonStringFromPaths(paths, new[] { UdonSharpCompilerVersionGuid }, out var version))
                    _udonSharpCompilerVersion = version;
            }
        }

        return _udonSharpCompilerVersion ?? DefaultVersion;
    }

    // ReSharper disable once InconsistentNaming
    private static bool HasVRChatSDKAssembly(IEnumerable<string> references, [NotNullWhen(true)] out string? path)
    {
        path = references.FirstOrDefault(w => w.EndsWith(SDKAssemblyName));
        return !string.IsNullOrWhiteSpace(path);
    }

    private static IEnumerable<string> FindUnityRootDirectory(string path)
    {
        const string unityAssetsDirectory = "Packages";
        var paths = new List<string>();
        var lastIndex = 0;

        while (path.IndexOf(unityAssetsDirectory, lastIndex, StringComparison.InvariantCulture) >= 0)
        {
            var i = path.IndexOf(unityAssetsDirectory, lastIndex, StringComparison.InvariantCulture);
            paths.Add(path[..i]);

            lastIndex = i + unityAssetsDirectory.Length;
        }

        paths.Reverse();
        return paths;
    }

    private static bool TryReadSpecifiedGuidFileAsJsonStringFromPaths(IEnumerable<string> paths, string[] guid, [NotNullWhen(true)] out string? version)
    {
        foreach (var path in paths)
        {
            var metas = Directory.GetFiles(Path.Combine(path, "Packages"), "package.json.meta", SearchOption.AllDirectories);
            foreach (var meta in metas)
                if (guid.Any(w => HasSpecifiedGuid(meta, w)))
                {
                    version = ReadContentFromMetaPath(meta);
                    if (VersionRegex.IsMatch(version))
                    {
                        var match = VersionRegex.Match(version);
                        version = match.Groups[1].Value;
                        return true;
                    }

                    return false;
                }
        }

        version = null;
        return false;
    }

    private static bool HasSpecifiedGuid(string path, string guid)
    {
        using var sr = new StreamReader(path);
        return sr.ReadToEnd().IndexOf(guid, StringComparison.InvariantCulture) >= 0;
    }

    private static string ReadContentFromMetaPath(string path)
    {
        var actual = Path.Combine(Path.GetDirectoryName(path) ?? throw new InvalidOperationException(), Path.GetFileNameWithoutExtension(path));
        using var sr = new StreamReader(actual);
        return sr.ReadToEnd().Trim();
    }
}