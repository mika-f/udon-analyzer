// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NatsunekoLaboratory.UdonAnalyzer.Extensions;

public static class PropertyDeclarationSyntaxExtensions
{
    public static bool HasAttribute(this PropertyDeclarationSyntax node, string fullyQualifiedMetadataName, SemanticModel model)
    {
        return node.AttributeLists.Any(w => w.Target == null && w.Attributes.Any(v => v.IsEquivalentType(fullyQualifiedMetadataName, model)));
    }

    public static IEnumerable<AttributeSyntax> GetAttributes(this PropertyDeclarationSyntax node, string fullyQualifiedMetadataName, SemanticModel model)
    {
        return node.AttributeLists
                   .Where(w => w.Target == null)
                   .SelectMany(w => w.Attributes)
                   .Where(w => w.IsEquivalentType(fullyQualifiedMetadataName, model));
    }

    public static bool HasModifiersExact(this PropertyDeclarationSyntax node, params SyntaxKind[] modifiers)
    {
        return node.Modifiers.All(w => modifiers.Contains(w.Kind()));
    }

    public static bool HasModifiers(this PropertyDeclarationSyntax node, params SyntaxKind[] modifiers)
    {
        return node.Modifiers.Any(w => modifiers.Contains(w.Kind()));
    }
}