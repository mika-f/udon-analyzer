﻿// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;

namespace NatsunekoLaboratory.UdonAnalyzer.Extensions;

// ReSharper disable once InconsistentNaming
public static class ISymbolExtensions
{
    private static readonly Dictionary<string, string> RemappedRegistry;

    static ISymbolExtensions()
    {
        RemappedRegistry = new Dictionary<string, string>
        {
            { "VRC.SDKBase.VRC_AvatarPedestal", "VRC.SDK3.Components.VRCAvatarPedestal" },
            { "VRC.SDKBase.VRC_Interactable", "VRC.SDK3.Components.VRCInteractable" },
            { "VRC.SDKBase.VRCMirrorReflection", "VRC.SDK3.Components.VRCMirrorReflection" },
            { "VRC.SDKBase.VRC_Pickup", "VRC.SDK3.Components.VRCPickup" },
            { "VRC.SDKBase.VRC_PortalMarker", "VRC.SDK3.Components.VRCPortalMarker" },
            { "VRC.SDKBase.VRC_SceneDescriptor", "VRC.SDK3.Components.VRCSceneDescriptor" },
            { "VRC.SDKBase.VRC_SpatialAudioSource", "VRC.SDK3.Components.VRCSpatialAudioSource" },
            { "VRC.SDKBase.VRCStation", "VRC.SDK3.Components.VRCStation" },
            { "VRC.SDKBase.VRC_UiShape", "VRC.SDK3.Components.VRCUiShape" },
            { "VRC.SDKBase.VRC_VisualDamage", "VRC.SDK3.Components.VRCVisualDamage" },
            { "VRC.SDK3.Video.Components.VRCUnityVideoPlayer", "VRC.SDK3.Video.Components.Base.BaseVRCVideoPlayer" },
            { "VRC.SDK3.Video.Components.AVPro.VRCAVProVideoPlayer", "VRC.SDK3.Video.Components.Base.BaseVRCVideoPlayer" },
            { "UdonSharp.UdonSharpBehaviour", "VRC.Udon.Common.Interfaces.IUdonEventReceiver" },
            { "VRC.Udon.UdonBehaviour", "VRC.Udon.Common.Interfaces.IUdonEventReceiver" }
        };
    }

    // ReSharper disable once InconsistentNaming
    public static string ToVRChatDeclarationId(this ISymbol symbol, ISymbol? receiver = null)
    {
        switch (symbol)
        {
            case IArrayTypeSymbol ats:
            {
                var sig = $"{ats.ElementType.ToVRChatDeclarationId()}Array";
                return sig.CountOf("Array") >= 2 ? sig[..(sig.IndexOf("Array", StringComparison.InvariantCulture) + "Array".Length)] : sig;
            }

            case IMethodSymbol ms:
            {
                var parameters = string.Join("_", ms.Parameters.Select(w => w.ToVRChatDeclarationId()));
                if (parameters.Length > 0)
                    return $"{(receiver ?? ms.ContainingType).ToVRChatDeclarationId()}.__{ms.Name}__{parameters}__{ms.ReturnType.ToVRChatDeclarationId()}";
                return $"{(receiver ?? ms.ContainingType).ToVRChatDeclarationId()}.__{ms.Name}__{ms.ReturnType.ToVRChatDeclarationId()}";
            }

            case IParameterSymbol ps:
                return ps.Type.ToVRChatDeclarationId();

            case INamedTypeSymbol nts:
                if (nts.ContainingType != null)
                    return RemapInternalComponents($"{nts.ContainingType.ToVRChatDeclarationId()}{nts.Name}");
                return RemapInternalComponents($"{FlattenNamespace(symbol)}{nts.Name}");

            case ITypeParameterSymbol tps:
                return tps.Name;
        }

        return "INVALID";
    }

    // ReSharper disable once InconsistentNaming
    public static string ToVRChatDeclarationId(this ISymbol symbol, ISymbol? receiver, bool isGetterContext)
    {
        switch (symbol)
        {
            case IArrayTypeSymbol ats:
            {
                var sig = $"{ats.ElementType.ToVRChatDeclarationId()}Array";
                return sig.CountOf("Array") >= 2 ? sig[..(sig.IndexOf("Array", StringComparison.InvariantCulture) + "Array".Length)] : sig;
            }

            case IFieldSymbol fs:
                return $"{(receiver ?? fs.ContainingType).ToVRChatDeclarationId()}.__{(isGetterContext ? "get" : "set")}_{fs.Name}__{fs.Type.ToVRChatDeclarationId()}";

            case INamedTypeSymbol nts:
                return RemapInternalComponents($"{FlattenNamespace(symbol)}{nts.Name}");

            case IParameterSymbol:
                return $"{FlattenNamespace(symbol)}";

            case IPropertySymbol ps:
                return $"{(receiver ?? ps.ContainingType).ToVRChatDeclarationId()}.__{(isGetterContext ? "get" : "set")}_{ps.Name}__{ps.Type.ToVRChatDeclarationId()}";
        }

        return "INVALID";
    }

    private static string FlattenNamespace(ISymbol symbol)
    {
        return symbol.ContainingNamespace.ToDisplayString().Replace(".", "");
    }

    private static string RemapInternalComponents(string str)
    {
        foreach (var key in RemappedRegistry.Keys)
            str = str.Replace(key.Replace(".", ""), RemappedRegistry[key]);

        return str.Replace(".", "");
    }
}