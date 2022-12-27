// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// -------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NatsunekoLaboratory.UdonAnalyzer;

public static class SyntaxNodeHelper
{
    public static List<ClassDeclarationSyntax> EnumerateClassDeclarations(SyntaxNode node)
    {
        return node.DescendantNodes().Where(w => w is MemberDeclarationSyntax).OfType<ClassDeclarationSyntax>().ToList();
    }
}