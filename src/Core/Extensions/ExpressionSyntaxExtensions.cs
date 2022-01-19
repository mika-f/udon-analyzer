// -----------------------------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the Microsoft Reference Source License. See LICENSE in the project root for license information.
// -----------------------------------------------------------------------------------------------------------------

using System;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NatsunekoLaboratory.UdonAnalyzer.Extensions;

public static class ExpressionSyntaxExtensions
{
    public static object? Invoke(this ExpressionSyntax syntax, SemanticModel model)
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