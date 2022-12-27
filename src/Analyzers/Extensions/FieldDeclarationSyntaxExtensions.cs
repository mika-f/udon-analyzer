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

public static class FieldDeclarationSyntaxExtensions
{
    public static bool HasAttribute(this FieldDeclarationSyntax node, string fullyQualifiedMetadataName, SemanticModel model)
    {
        return node.AttributeLists.Any(w => w.Attributes.Any(v => v.IsEquivalentType(fullyQualifiedMetadataName, model)));
    }

    public static AttributeSyntax? GetAttribute(this FieldDeclarationSyntax node, string fullyQualifiedMetadataName, SemanticModel model)
    {
        return GetAttributes(node, fullyQualifiedMetadataName, model).FirstOrDefault();
    }

    public static IEnumerable<AttributeSyntax> GetAttributes(this FieldDeclarationSyntax node, string fullyQualifiedMetadataName, SemanticModel model)
    {
        return node.AttributeLists
                   .SelectMany(w => w.Attributes)
                   .Where(w => w.IsEquivalentType(fullyQualifiedMetadataName, model));
    }

    public static bool HasModifiersExact(this FieldDeclarationSyntax node, params SyntaxKind[] modifiers)
    {
        return node.Modifiers.All(w => modifiers.Contains(w.Kind()));
    }

    public static bool HasModifier(this FieldDeclarationSyntax node, params SyntaxKind[] modifiers)
    {
        return node.Modifiers.Any(w => modifiers.Contains(w.Kind()));
    }
}