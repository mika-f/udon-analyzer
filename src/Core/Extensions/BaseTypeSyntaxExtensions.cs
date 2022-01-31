// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NatsunekoLaboratory.UdonAnalyzer.Extensions;

public static class BaseTypeSyntaxExtensions
{
    public static bool IsInterface(this BaseTypeSyntax obj, SemanticModel model)
    {
        return obj.Type.IsInterface(model);
    }
}