// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// -------------------------------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace NatsunekoLaboratory.UdonAnalyzer;

public static class DiagnosticHelper
{
    public static void ReportDiagnostic(SyntaxNodeAnalysisContext context, DiagnosticDescriptor descriptor, SyntaxNode node, params object[] messageArgs)
    {
        ReportDiagnostic(context, Diagnostic.Create(descriptor, node.GetLocation(), messageArgs));
    }

    public static void ReportDiagnostic(SyntaxNodeAnalysisContext context, Diagnostic diagnostic)
    {
        context.ReportDiagnostic(diagnostic);
    }
}