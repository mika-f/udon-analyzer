// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Linq;

using Microsoft.CodeAnalysis;

namespace NatsunekoLaboratory.UdonAnalyzer.Extensions;

// ReSharper disable once InconsistentNaming
public static class ISymbolExtensions
{
    // ReSharper disable once InconsistentNaming
    public static string ToVRChatDeclarationId(this ISymbol symbol, ISymbol? receiver = null)
    {
        switch (symbol)
        {
            case IArrayTypeSymbol ats:
                return $"{ats.ElementType.ToVRChatDeclarationId()}Array";

            case IMethodSymbol ms:
            {
                var parameters = string.Join("__", ms.Parameters.Select(w => w.ToVRChatDeclarationId()));
                if (parameters.Length > 0)
                    return $"{(receiver ?? ms.ContainingType).ToVRChatDeclarationId()}.__{ms.Name}__{parameters}__{ms.ReturnType.ToVRChatDeclarationId()}";
                return $"{(receiver ?? ms.ContainingType).ToVRChatDeclarationId()}.__{ms.Name}__{ms.ReturnType.ToVRChatDeclarationId()}";
            }

            case IParameterSymbol ps:
                return ps.Type.ToVRChatDeclarationId();

            case INamedTypeSymbol nts:
                return RemapUdonSharpBehaviourToIUdonEventReceiver($"{FlattenNamespace(symbol)}{nts.Name}");

            case ITypeParameterSymbol tps:
                return tps.Name;
        }

        return "INVALID";
    }

    // ReSharper disable once InconsistentNaming
    public static string ToVRChatDeclarationId(this ISymbol symbol, bool isGetterContext)
    {
        switch (symbol)
        {
            case INamedTypeSymbol:
                return $"{FlattenNamespace(symbol)}";

            case IParameterSymbol:
                return $"{FlattenNamespace(symbol)}";

            case IFieldSymbol:
                return $"{FlattenNamespace(symbol)}";

            case IPropertySymbol:
                return $"{FlattenNamespace(symbol)}";
        }

        return "INVALID";
    }

    private static string FlattenNamespace(ISymbol symbol)
    {
        return symbol.ContainingNamespace.ToDisplayString().Replace(".", "");
    }

    private static string RemapUdonSharpBehaviourToIUdonEventReceiver(string str)
    {
        return str.Replace("UdonSharpUdonSharpBehaviour", "VRCUdonCommonInterfacesIUdonEventReceiver");
    }
}