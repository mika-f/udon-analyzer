// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Diagnostics;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.Markdown.Syntax;

[DebuggerDisplay("{Kind,nq} {GetDebuggerDisplay(),nq} - DisplayAsBlock")]
public abstract class BlockNode : SyntaxNode { }