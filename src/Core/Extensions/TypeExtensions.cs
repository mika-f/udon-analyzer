// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;

using Microsoft.CodeAnalysis;

namespace NatsunekoLaboratory.UdonAnalyzer.Extensions;

public static class TypeExtensions
{
    public static ConstructorInfo? FindBestMatchingConstructor(this Type t, IMethodSymbol symbol, SemanticModel model)
    {
        return t.GetConstructors().FirstOrDefault(w =>
        {
            var actualParameters = w.GetParameters();
            var providedParameters = symbol.Parameters;

            if (actualParameters.Length != providedParameters.Length)
                return false;

            foreach (var (actualParameter, i) in actualParameters.Select((v, i) => (v, i)))
            {
                var providedParameter = providedParameters[i].Type;
                var typeSymbol = model.Compilation.GetTypeByMetadataName(actualParameter.ParameterType.FullName ?? throw new InvalidOperationException());

                if (!providedParameter.Equals(typeSymbol, SymbolEqualityComparer.Default))
                    return false;
            }

            return true;
        });
    }
}