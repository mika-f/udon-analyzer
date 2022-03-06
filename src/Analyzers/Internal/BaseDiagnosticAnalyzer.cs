// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Linq;

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

    protected Lazy<RequireCSharpLanguageFeatureAttribute?> RequiredCSharpLanguageFeature => new(FetchRequiredCustomAttribute<RequireCSharpLanguageFeatureAttribute>);

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
    }

    protected void RunAnalyzer(SyntaxNodeAnalysisContext context, bool isRequireInherit, Action<SyntaxNodeAnalysisContext> callback)
    {
        if (RequiredUdonVersion.Value?.IsFulfill(CurrentUdonRuntimeVersion(context)) == false)
            return;

        if (RequiredUdonSharpCompilerVersion.Value?.IsFulfill(CurrentUdonSharpCompilerVersion(context)) == false)
            return;

        if (RequiredCSharpLanguageFeature.Value?.IsFulfill(CurrentCSharpLanguageFeature(context)) == false)
            return;

        if (IsSyntaxNodeInsideOfIgnoringPreprocessor(context))
            return;

        if (IsEnableWorkspaceAnalyzing(context))
        {
            if (isRequireInherit && IsSyntaxNodeInsideOfClassNotInheritedFromSpecifiedClass(context) && IsSyntaxNodeOutsideOfClassNotInheritedFromSpecifiedClass(context))
                return;
        }
        else
        {
            if (IsSyntaxNodeInsideOfClassNotInheritedFromSpecifiedClass(context) && IsSyntaxNodeOutsideOfClassNotInheritedFromSpecifiedClass(context))
                return;
        }

        callback.Invoke(context);
    }

    private T? FetchRequiredCustomAttribute<T>() where T : Attribute
    {
        var attr = Attribute.GetCustomAttribute(GetType(), typeof(T));
        return attr as T;
    }

    private static bool IsSyntaxNodeOutsideOfClassNotInheritedFromSpecifiedClass(SyntaxNodeAnalysisContext context)
    {
        var declarations = SyntaxNodeHelper.EnumerateClassDeclarations(context.SemanticModel.SyntaxTree.GetRoot());
        if (declarations.Any(w => w.IsInheritOf(CurrentSpecifiedBehaviourInheritFullName(context), context.SemanticModel)))
            return false;
        return true;
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
        var v = context.GetEditorConfigValue(AnalyzerOptionDescriptors.UdonVirtualMachineVersion);
        return v == "auto" ? CSharpSolutionContext.FetchUdonRuntimeVersion(context) : v;
    }

    private static string CurrentUdonSharpCompilerVersion(SyntaxNodeAnalysisContext context)
    {
        var v = context.GetEditorConfigValue(AnalyzerOptionDescriptors.UdonSharpCompilerVersion);
        return v == "auto" ? CSharpSolutionContext.FetchUdonSharpCompilerVersion(context) : v;
    }

    private static string CurrentCSharpLanguageFeature(SyntaxNodeAnalysisContext context)
    {
        return context.GetEditorConfigValue(AnalyzerOptionDescriptors.CSharpLanguageFeature);
    }

    private static bool IsEnableWorkspaceAnalyzing(SyntaxNodeAnalysisContext context)
    {
        return context.GetEditorConfigValue(AnalyzerOptionDescriptors.EnableWorkspaceAnalyzing);
    }

    private static string CurrentSpecifiedBehaviourInheritFullName(SyntaxNodeAnalysisContext context)
    {
        return context.GetEditorConfigValue(AnalyzerOptionDescriptors.BehaviourInheritFrom);
    }

    private static string CurrentUdonSharpCompilerPreprocessor(SyntaxNodeAnalysisContext context)
    {
        return context.GetEditorConfigValue(AnalyzerOptionDescriptors.UdonSharpCompilerIgnoringPreprocessor);
    }
}