// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Versioning;

using Microsoft.Win32;

namespace NatsunekoLaboratory.UdonAnalyzer.Testing;

public class UnityStandaloneProject : StandaloneProject
{
    protected virtual string TargetedUnityVersion => "2019.4.31f1";

    protected override IEnumerable<string> ExternalReferences()
    {
        var path = FindUnityPath();

        var managed = Path.Combine(path, "Managed");
        yield return Path.Combine(path, managed, "UnityEditor.dll");
        yield return Path.Combine(path, managed, "UnityEngine.dll");

        var mono = Path.Combine(path, "MonoBleedingEdge", "lib", "mono", "4.7.1-api");
        yield return Path.Combine(mono, "mscorlib.dll");
        yield return Path.Combine(mono, "System.dll");
    }

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
    private string FindUnityPath()
    {
        var hub = Environment.OSVersion.Platform switch
        {
            PlatformID.Win32NT => FindUnityPathFromRegistry(),
            PlatformID.Unix => FindUnityPathFromEnvironmentVariable(),
            PlatformID.MacOSX => FindUnityPathFromApplications(),
            _ => throw new ArgumentOutOfRangeException()
        };

        var info = new ProcessStartInfo(hub, "-- --headless editors --installed")
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardError = false,
            RedirectStandardOutput = true
        };

        using var process = Process.Start(info);
        if (process == null)
            throw new InvalidOperationException();

        process.WaitForExit();

        var editors = process.StandardOutput.ReadToEnd();
        foreach (var editor in editors.Split(Environment.NewLine))
        {
            var version = editor.Split(",")[0].Trim();
            if (version != TargetedUnityVersion)
                continue;

            var path = editor.Split(",")[1].Trim();
            return Path.GetDirectoryName(path["installed at".Length..].Trim())!;
        }

        throw new FileNotFoundException("specified Unity is does not installed on this computer");
    }

    [SupportedOSPlatform("windows")]
    private static string FindUnityPathFromRegistry()
    {
        var registry = Registry.LocalMachine.OpenSubKey(@"Software\Unity Technologies\Hub");
        if (registry == null)
            throw new NotSupportedException("UnityHub is not installed on this computer");

        var path = registry.GetValue("InstallLocation") as string;
        if (string.IsNullOrWhiteSpace(path))
            throw new NotSupportedException("UnityHub is not installed on this computer");

        registry.Close();
        return Path.Combine(path, "Unity Hub.exe");
    }

    [SupportedOSPlatform("linux")]
    private static string FindUnityPathFromEnvironmentVariable()
    {
        throw new NotImplementedException();
    }

    [SupportedOSPlatform("macos")]
    private static string FindUnityPathFromApplications()
    {
        throw new NotImplementedException();
    }
}