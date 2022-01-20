// -------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// -------------------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

using NatsunekoLaboratory.UdonAnalyzer.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.Extensions;
using NatsunekoLaboratory.UdonAnalyzer.Models;

namespace NatsunekoLaboratory.UdonAnalyzer.Internal;

public abstract class BaseDiagnosticAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(SupportedDiagnostic);

    public abstract DiagnosticDescriptor SupportedDiagnostic { get; }

    protected Lazy<RequireUdonVersionAttribute?> RequiredUdonVersion => new(FetchRequiredCustomAttribute<RequireUdonVersionAttribute>);

    protected Lazy<RequireUdonSharpCompilerVersionAttribute?> RequiredUdonSharpCompilerVersion => new(FetchRequiredCustomAttribute<RequireUdonSharpCompilerVersionAttribute>);

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
    }

    protected void RunAnalyzer(SyntaxNodeAnalysisContext context, Action<SyntaxNodeAnalysisContext> callback)
    {
        if (RequiredUdonVersion.Value?.IsFulfill(CurrentUdonRuntimeVersion(context)) != true)
            return;

        if (RequiredUdonSharpCompilerVersion.Value?.IsFulfill(CurrentUdonSharpCompilerVersion(context)) != true)
            return;

        if (IsRequireUdonSharpBehaviourInherit(context) && IsSyntaxNodeInsideOfClassNotInheritedFromSpecifiedClass(context))
            return;

        if (IsSyntaxNodeInsideOfIgnoringPreprocessor(context))
            return;

        callback.Invoke(context);
    }

    private T? FetchRequiredCustomAttribute<T>() where T : Attribute
    {
        var attr = Attribute.GetCustomAttribute(GetType(), typeof(T));
        return attr as T;
    }

    private static bool IsSyntaxNodeInsideOfClassNotInheritedFromSpecifiedClass(SyntaxNodeAnalysisContext context)
    {
        var classDecl = context.Node.FirstAncestorOrSelf<ClassDeclarationSyntax>();
        if (classDecl == null)
            return true;

        var symbol = context.SemanticModel.GetDeclaredSymbol(classDecl);
        if (symbol == null)
            return true;

        return symbol.BaseType?.Equals(context.SemanticModel.Compilation.GetTypeByMetadataName(CurrentSpecifiedBehaviourInheritFullName(context)), SymbolEqualityComparer.Default) != true;
    }

    private static bool IsSyntaxNodeInsideOfIgnoringPreprocessor(SyntaxNodeAnalysisContext context)
    {
        var options = CSharpParseOptions.Default.WithPreprocessorSymbols(CurrentUdonSharpCompilerPreprocessor(context));
        var tree = CSharpSyntaxTree.ParseText(context.Node.SyntaxTree.GetText(), options);
        var matched = tree.GetRoot().FindNode(context.Node.Span);

        return context.Node.Span.Start != matched.Span.Start || context.Node.Span.End != matched.Span.End;
    }

    private static string CurrentUdonRuntimeVersion(SyntaxNodeAnalysisContext context)
    {
        var v = context.GetEditorConfigValue(AnalyzerOptionDescriptors.UdonVirtualMachineVersion)!;
        return v == "auto" ? CSharpSolutionContext.FetchUdonRuntimeVersion(context) : v;
    }

    private static string CurrentUdonSharpCompilerVersion(SyntaxNodeAnalysisContext context)
    {
        var v = context.GetEditorConfigValue(AnalyzerOptionDescriptors.UdonSharpCompilerVersion)!;
        return v == "auto" ? CSharpSolutionContext.FetchUdonSharpCompilerVersion(context) : v;
    }

    private static bool IsRequireUdonSharpBehaviourInherit(SyntaxNodeAnalysisContext context)
    {
        return context.GetEditorConfigValue(AnalyzerOptionDescriptors.RequireBehaviourInherit);
    }

    private static string CurrentSpecifiedBehaviourInheritFullName(SyntaxNodeAnalysisContext context)
    {
        return context.GetEditorConfigValue(AnalyzerOptionDescriptors.BehaviourInheritFrom)!;
    }

    private static string CurrentUdonSharpCompilerPreprocessor(SyntaxNodeAnalysisContext context)
    {
        return context.GetEditorConfigValue(AnalyzerOptionDescriptors.UdonSharpCompilerIgnoringPreprocessor)!;
    }
}