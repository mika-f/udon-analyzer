// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// -------------------------------------------------------------------------------------------

using NatsunekoLaboratory.UdonAnalyzer.Enums;

namespace NatsunekoLaboratory.UdonAnalyzer;

public static class AnalyzerOptionDescriptors
{
    public static readonly AnalyzerOptionDescriptor<bool> EnableWorkspaceAnalyzing = new("udon_analyzer.enable_workspace_analyzing", false);
    public static readonly AnalyzerOptionDescriptor<string> BehaviourInheritFrom = new("udon_analyzer.inherit_from", "UdonSharp.UdonSharpBehaviour");
    public static readonly AnalyzerOptionDescriptor<string> UdonVirtualMachineVersion = new("udon_analyzer.vrchat_sdk", "auto");
    public static readonly AnalyzerOptionDescriptor<string> UdonSharpCompilerVersion = new("udon_analyzer.udon_sharp", "auto");
    public static readonly AnalyzerOptionDescriptor<string> CSharpLanguageFeature = new("udon_analyzer.csharp_version", "7.2");
    public static readonly AnalyzerOptionDescriptor<string> UdonSharpCompilerIgnoringPreprocessor = new("udon_analyzer.udon_sharp_ignore", "COMPILER_UDONSHARP");
    public static readonly AnalyzerOptionDescriptor<DictionaryMode> UdonApiDictionaryMode = new("udon_analyzer.api_dictionary_mode", DictionaryMode.Direct);
    public static readonly AnalyzerOptionDescriptor<DictionaryMode> UdonNetworkDictionaryMode = new("udon_analyzer.network_dictionary_mode", DictionaryMode.Direct);
}