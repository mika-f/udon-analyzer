// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace NatsunekoLaboratory.UdonAnalyzer.Testing.Extensions;

internal static class DocumentExtensions
{
    public static async Task<T?> FindEquivalentNodeAsync<T>(this Document document, T node, CancellationToken cancellationToken = default) where T : SyntaxNode
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken);
        return root!.DescendantNodesAndSelf(_ => true).OfType<T>().FirstOrDefault(w => w.IsEquivalentTo(node, true));
    }

    public static async Task<SyntaxNode> FindNodeAsync(this Document document, TextSpan span, CancellationToken cancellationToken = default)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken);
        return root!.FindNode(span);
    }
}