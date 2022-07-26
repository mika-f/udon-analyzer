// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

using NatsunekoLaboratory.UdonAnalyzer.Extensions;

namespace NatsunekoLaboratory.UdonAnalyzer.Models;

public class SymbolDictionary
{
    private static SymbolDictionary? _instance;
    private static readonly List<string> WhitelistRegistry;
    private readonly Dictionary<string, ImmutableArray<byte>> _cached;
    private readonly object _lockObj;
    private readonly Dictionary<string, List<string>> _symbols;

    public static SymbolDictionary Instance => _instance ??= new SymbolDictionary();

    static SymbolDictionary()
    {
        WhitelistRegistry = new List<string>
        {
            "VRCUdonCommonInterfacesIUdonEventReceiver.__get_gameObject__UnityEngineGameObject",
            "VRCUdonCommonInterfacesIUdonEventReceiver.__GetProgramVariable__SystemString__T",
            "VRCUdonCommonInterfacesIUdonEventReceiver.__SetProgramVariable__SystemString_T__SystemVoid",
            "Type_SystemVoid"
        };
    }

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

        var declarationId = $"Type_{symbol.ToVRChatDeclarationId()}";
        return _symbols.SelectMany(w => w.Value).Any(w => w == declarationId) || WhitelistRegistry.Contains(declarationId);
    }

    public bool IsSymbolIsAllowed(ISymbol symbol, ISymbol? receiver, SyntaxNodeAnalysisContext context)
    {
        if (IsUserDefinedSymbol(symbol))
            return true;

        LoadDictionaryFromAdditionalFiles(context);

        var declarationId = symbol.ToVRChatDeclarationId(receiver);
        return _symbols.SelectMany(w => w.Value).Any(w => w == declarationId) || WhitelistRegistry.Contains(declarationId);
    }

    public bool IsSymbolIsAllowed(ISymbol symbol, ISymbol? receiver, bool isGetterContext, SyntaxNodeAnalysisContext context)
    {
        if (IsUserDefinedSymbol(symbol))
            return true;

        LoadDictionaryFromAdditionalFiles(context);

        if (isGetterContext && receiver is INamedTypeSymbol { EnumUnderlyingType: not null })
            return IsSymbolIsAllowed(receiver, context);

        var declarationId = symbol.ToVRChatDeclarationId(receiver, isGetterContext);
        return _symbols.SelectMany(w => w.Value).Any(w => w == declarationId) || WhitelistRegistry.Contains(declarationId);
    }

    private static bool IsUserDefinedSymbol(ISymbol symbol)
    {
        return symbol switch
        {
            IArrayTypeSymbol a => IsUserDefinedSymbol(a.ElementType),
            // UdonSharp.UdonSharpBehaviour is located in source, but specified in this line for tests
            INamedTypeSymbol t => t.BaseType?.ToDisplayString() == "UdonSharp.UdonSharpBehaviour" || t.ToDisplayString() == "UdonSharp.UdonSharpBehaviour" || t.Locations.All(w => w.IsInSource),
            IMethodSymbol m => IsUserDefinedSymbol(m.ReceiverType ?? throw new InvalidOperationException()),
            IFieldSymbol f => IsUserDefinedSymbol(f.ContainingType ?? throw new InvalidOperationException()),
            IPropertySymbol p => IsUserDefinedSymbol(p.ContainingType ?? throw new InvalidOperationException()),
            _ => false
        };
    }

    private void LoadDictionaryFromAdditionalFiles(SyntaxNodeAnalysisContext context)
    {
        lock (_lockObj)
        {
            var sources = context.Options
                                 .AdditionalFiles
                                 .Where(w =>
                                 {
                                     var filename = Path.GetFileName(w.Path);
                                     return filename.StartsWith("PublicAPI.Shipped.") && filename.EndsWith(".txt");
                                 })
                                 .ToList();

            if (sources.Count != _cached.Count)
            {
                _cached.Clear();
                _symbols.Clear();
            }

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