// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Linq;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NatsunekoLaboratory.UdonAnalyzer.Extensions;

public static class FieldDeclarationSyntaxExtensions
{
    public static bool HasModifiersExact(this FieldDeclarationSyntax node, params SyntaxKind[] modifiers)
    {
        return node.Modifiers.All(w => modifiers.Contains(w.Kind()));
    }

    public static bool HasModifier(this FieldDeclarationSyntax node, params SyntaxKind[] modifiers)
    {
        return node.Modifiers.Any(w => modifiers.Contains(w.Kind()));
    }
}