// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NatsunekoLaboratory.UdonAnalyzer.Extensions;

public static class ExpressionSyntaxExtensions
{
    public static object? InvokeConstantValue(this ExpressionSyntax syntax, SemanticModel model)
    {
        var constantValue = model.GetConstantValue(syntax);
        if (constantValue.HasValue)
            return constantValue.Value!;

        return syntax switch
        {
            TypeOfExpressionSyntax t1 => (model.GetTypeInfo(t1.Type).Type as INamedTypeSymbol)?.InvokeAsType(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}