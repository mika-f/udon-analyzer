// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// -------------------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace NatsunekoLaboratory.UdonAnalyzer;

public static class DiagnosticDescriptorFactory
{
    private const string HelpLinkBaseUri = "https://docs.natsuneko.cat/udon-analyzer/diagnostics/";

    public static DiagnosticDescriptor Create(string id, string title, string messageFormat, string category, DiagnosticSeverity defaultSeverity, bool isEnabledByDefault = true, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(description))
            description = messageFormat;
        return new DiagnosticDescriptor(id, title, messageFormat, category, defaultSeverity, isEnabledByDefault, description, HelpLinkBaseUri + id);
    }
}