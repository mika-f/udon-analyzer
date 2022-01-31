// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

using NatsunekoLaboratory.UdonAnalyzer.AnalyzerSpec.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.Attributes;
using NatsunekoLaboratory.UdonAnalyzer.CodeGeneration.CSharp.Workspace;
using NatsunekoLaboratory.UdonAnalyzer.Extensions;

namespace NatsunekoLaboratory.UdonAnalyzer.CodeGeneration;

public class UdonAnalyzerMetadata
{
    private readonly List<AnalyzerMetadata> _metadata;
    private readonly string _root;
    private UdonAnalyzerSolution? _solution;

    public ImmutableArray<AnalyzerMetadata> Metadata => _metadata.ToImmutableArray();

    public UdonAnalyzerMetadata(string root)
    {
        _root = root;
        _metadata = new List<AnalyzerMetadata>();
    }

    public async Task<bool> TryAnalyzingAsync()
    {
        _metadata.Clear();

        try
        {
            await TryLoadUdonAnalyzerSolutionAsync().Stay();
            await TryLoadAnalyzersAsync().Stay();
            await TryLoadCodeFixesAsync().Stay();

            return true;
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync(e.Message).Stay();

            return false;
        }
    }

    private async Task TryLoadUdonAnalyzerSolutionAsync()
    {
        var path = GetSourcePath("UdonAnalyzer.sln");
        if (!File.Exists(path))
            throw new FileNotFoundException(path);

        _solution = await UdonAnalyzerSolution.CreateFromPathAsync(path).Stay();
    }

    private async Task TryLoadAnalyzersAsync()
    {
        if (_solution == null)
            throw new InvalidOperationException();

        if (!_solution.TryGetProject(UdonAnalyzerProjects.AnalyzersTests, out var project))
            throw new InvalidOperationException();

        var specs = await EnumerateTestClassesAsync(project).Stay();

        foreach (var (document, node) in specs)
        {
            var metadata = await AnalyzeMetadataFromSourceAsync(document, node).Stay();
            if (metadata != null)
                _metadata.Add(metadata);
        }
    }

    private async Task TryLoadCodeFixesAsync()
    {
        if (_solution == null)
            throw new InvalidOperationException();

        if (!_solution.TryGetProject(UdonAnalyzerProjects.CodeFixesTests, out var project))
            throw new InvalidOperationException();

        var tests = await EnumerateTestClassesAsync(project).Stay();
    }

    private string GetSourcePath(string project, string? category = null)
    {
        return Path.GetFullPath(Path.Combine(_root, "src", category ?? "", project));
    }

    private static async Task<IEnumerable<(Document Document, ClassDeclarationSyntax Node)>> EnumerateTestClassesAsync(Project project)
    {
        if (project.Language != LanguageNames.CSharp)
            return new List<(Document Document, ClassDeclarationSyntax Node)>();

        return await project.Documents
                            .ToAsyncEnumerable()
                            .WhereAwait(async w =>
                            {
                                if (w.SourceCodeKind != SourceCodeKind.Regular)
                                    return false;
                                var root = await w.GetSyntaxRootAsync().Stay();
                                return root != null;
                            })
                            .SelectManyAwait(async w =>
                            {
                                var node = await w.GetSyntaxRootAsync().Stay();
                                if (node == null)
                                    return new List<(Document Document, ClassDeclarationSyntax Node)>().ToAsyncEnumerable();
                                return SyntaxNodeHelper.EnumerateClassDeclarations(node).Select(v => (Document: w, Node: v)).ToAsyncEnumerable();
                            })
                            .WhereAwait(async w =>
                            {
                                var (document, node) = w;
                                var model = await document.GetSemanticModelAsync().Stay();
                                if (model == null)
                                    return false;
                                return node.HasAttribute<DescribeAttribute>(model);
                            })
                            .ToListAsync()
                            .ConfigureAwait(false);
    }

    private static async Task<AnalyzerMetadata?> AnalyzeMetadataFromSourceAsync(Document document, ClassDeclarationSyntax node)
    {
        var model = await document.GetSemanticModelAsync().Stay();
        if (model == null)
            return null;

        var attribute = node.GetAttribute<DescribeAttribute>(model);
        var analyzer = attribute?.GetAttributeValue<DescribeAttribute, Type>("analyzer", model);
        if (analyzer == null)
            return null;

        var instance = Activator.CreateInstance(analyzer);
        if (instance is not DiagnosticAnalyzer obj)
            return null;

        var descriptor = obj.SupportedDiagnostics.First();
        var runtime = Attribute.GetCustomAttribute(analyzer, typeof(RequireUdonVersionAttribute)) as RequireUdonVersionAttribute;
        var compiler = Attribute.GetCustomAttribute(analyzer, typeof(RequireUdonSharpCompilerVersionAttribute)) as RequireUdonSharpCompilerVersionAttribute;
        if (runtime == null || compiler == null)
            return null;

        var example = AnalyzeTestCodeForExampleSource(node, model);

        return new AnalyzerMetadata(descriptor.Id, descriptor.Title.ToString(), descriptor.Description.ToString(), descriptor.Category, descriptor.DefaultSeverity, runtime.VersionStr, compiler.VersionStr, example, null);
    }

    private static string? AnalyzeTestCodeForExampleSource(TypeDeclarationSyntax node, SemanticModel model)
    {
        var target = node.Members.FirstOrDefault(w => w is MethodDeclarationSyntax m && m.HasAttribute<ExampleAttribute>(model));
        if (target is not MethodDeclarationSyntax method)
            return null;

        var expression = method.Body!.DescendantNodes().FirstOrDefault(w =>
        {
            if (w is not AwaitExpressionSyntax { Expression: InvocationExpressionSyntax invocation })
                return false;

            var info = model.GetSymbolInfo(invocation);
            if (info.Symbol is not IMethodSymbol symbol)
                return false;

            return symbol.Name == "VerifyAnalyzerAsync";
        }) as AwaitExpressionSyntax;

        if (expression is not { Expression: InvocationExpressionSyntax invoke })
            return null;

        var code = model.GetConstantValue(invoke.ArgumentList.Arguments[0].Expression);
        return code.HasValue ? ParseAnnotatedSourceCodeToErrorCode(code.Value as string ?? throw new InvalidOperationException()) : null;
    }

    private static string ParseAnnotatedSourceCodeToErrorCode(string annotated)
    {
        var sb = new StringBuilder();
        var esb = new StringBuilder();
        var column = 1;
        var isReadingAnnotation = false;

        using var sr = new StringReader(annotated);
        while (sr.Peek() > -1)
        {
            var c = (char)sr.Read();
            switch (c)
            {
                case '\n':
                    sb.Append(c);

                    var str = esb.ToString();
                    if (!string.IsNullOrWhiteSpace(str))
                    {
                        sb.AppendLine(str);
                        esb.Clear();
                    }

                    column = 1;
                    break;

                case '\r':
                    break;

                case '[' when sr.Peek() == '|':
                    sr.Read();

                    (column - 1).Times(() => esb.Append(' '));
                    isReadingAnnotation = true;
                    break;

                case '|' when isReadingAnnotation && sr.Peek() == '@':
                    sr.Read();
                    while (sr.Peek() != ']')
                        sr.Read();
                    sr.Read();

                    isReadingAnnotation = false;
                    break;

                case '|' when isReadingAnnotation && sr.Peek() == ']':
                    sr.Read();

                    isReadingAnnotation = false;
                    break;

                case ' ' when isReadingAnnotation:
                    sb.Append(c);

                    esb.Append(string.IsNullOrWhiteSpace(esb.ToString()) ? ' ' : '~');
                    break;

                default:
                    sb.Append(c);
                    if (isReadingAnnotation)
                        esb.Append('~');
                    column++;
                    break;
            }
        }

        return sb.ToString().Trim();
    }
}