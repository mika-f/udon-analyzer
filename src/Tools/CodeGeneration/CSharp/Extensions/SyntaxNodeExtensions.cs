// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.CSharp.Extensions;

public static class SyntaxNodeExtensions
{
    public static T WithNewLineTrivia<T>(this T obj) where T : SyntaxNode
    {
        return obj.WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed, SyntaxFactory.CarriageReturnLineFeed);
    }
}