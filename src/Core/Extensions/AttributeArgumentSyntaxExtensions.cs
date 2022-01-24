// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NatsunekoLaboratory.UdonAnalyzer.Extensions;

public static class AttributeArgumentSyntaxExtensions
{
    [Obsolete("this method has security issues")]
    public static object? Invoke(this AttributeArgumentSyntax syntax, SemanticModel model)
    {
        return syntax.Expression.Invoke(model);
    }
}