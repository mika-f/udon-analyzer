// -----------------------------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the Microsoft Reference Source License. See LICENSE in the project root for license information.
// -----------------------------------------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NatsunekoLaboratory.UdonAnalyzer.Extensions;

public static class AttributeArgumentSyntaxExtensions
{
    public static object? Invoke(this AttributeArgumentSyntax syntax, SemanticModel model)
    {
        return syntax.Expression.Invoke(model);
    }
}