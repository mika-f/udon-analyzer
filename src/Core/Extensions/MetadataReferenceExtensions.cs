// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// -------------------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace NatsunekoLaboratory.UdonAnalyzer.Extensions;

internal static class MetadataReferenceExtensions
{
    public static string? ToFilePath(this MetadataReference reference)
    {
        if (reference is PortableExecutableReference portable)
            return portable.FilePath;

        return reference.Display;
    }
}