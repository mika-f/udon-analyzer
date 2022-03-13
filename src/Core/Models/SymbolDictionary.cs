// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace NatsunekoLaboratory.UdonAnalyzer.Models;

public class SymbolDictionary
{
    private static SymbolDictionary? _instance;
    private readonly Dictionary<string, ImmutableArray<byte>> _cached;
    private readonly object _lockObj;
    private readonly Dictionary<string, List<string>> _symbols;

    public static SymbolDictionary Instance => _instance ??= new SymbolDictionary();

    private SymbolDictionary()
    {
        _cached = new Dictionary<string, ImmutableArray<byte>>();
        _lockObj = new object();
        _symbols = new Dictionary<string, List<string>>();
    }

    public bool IsSymbolIsAllowed(ISymbol symbol, SyntaxNodeAnalysisContext context)
    {
        if (IsUserDefinedSymbol(symbol))
            return true;

        LoadDictionaryFromAdditionalFiles(context);

        var declarationId = DocumentationCommentId.CreateDeclarationId(symbol);
        return _symbols.SelectMany(w => w.Value).Any(w => w == declarationId);
    }

    private static bool IsUserDefinedSymbol(ISymbol symbol)
    {
        return symbol switch
        {
            INamedTypeSymbol t => t.BaseType?.ToDisplayString() == "UdonSharp.UdonSharpBehaviour",
            IMethodSymbol m => IsUserDefinedSymbol(m.ReceiverType ?? throw new InvalidOperationException()),
            IFieldSymbol f => IsUserDefinedSymbol(f.ContainingType ?? throw new InvalidOperationException()),
            _ => false
        };
    }

    private void LoadDictionaryFromAdditionalFiles(SyntaxNodeAnalysisContext context)
    {
        lock (_lockObj)
        {
            var sources = context.Options
                                 .AdditionalFiles
                                 .Where(w => w.Path.StartsWith("PublicAPI.Shipped.") && w.Path.EndsWith(".txt"))
                                 .ToList();

            foreach (var source in sources)
                if (_cached.ContainsKey(source.Path))
                {
                    var cached = _cached[source.Path];
                    if (cached.Equals(source.GetText()?.GetChecksum()))
                        continue;
                    LoadDictionaryIntoCache(source);
                }
                else
                {
                    LoadDictionaryIntoCache(source);
                    _cached.Add(source.Path, source.GetText()?.GetChecksum() ?? ImmutableArray<byte>.Empty);
                }
        }
    }

    private void LoadDictionaryIntoCache(AdditionalText text)
    {
        var source = text.GetText();
        if (source == null)
            return;

        if (!_symbols.ContainsKey(text.Path))
            _symbols.Add(text.Path, new List<string>());

        var symbols = _symbols[text.Path];
        symbols.Clear();
        symbols.AddRange(source.Lines.Select(line => line.ToString()));
    }
}