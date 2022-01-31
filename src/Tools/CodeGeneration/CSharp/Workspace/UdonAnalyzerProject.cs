// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Diagnostics;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.CSharp.Workspace;

[DebuggerDisplay("{Category} - {DisplayName}")]
internal record UdonAnalyzerProject(string DisplayName, string CsProjName, string? Category = null);