// ------------------------------------------------------------------------------------------
//  Copyright (c) Natsuneko. All rights reserved.
//  Licensed under the MIT License. See LICENSE in the project root for license information.
// ------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using UnityEditor;

using UnityEngine;

using VRC.Udon.Editor;
using VRC.Udon.EditorBindings;

namespace NatsunekoLaboratory.UdonAnalyzer
{
    public static class DictionaryExporter
    {
        [InitializeOnLoadMethod]
        public static void OnInitialize()
        {
            UdonEditorManager.Instance.GetNodeRegistries();

            var nodeDefinitions = UdonEditorManager.Instance.GetNodeDefinitions().Select(w => w.fullName).ToList();
            var builtinEvents = new Dictionary<string, string>();

            var interfaces = new UdonEditorInterface();

            foreach (var definition in interfaces.GetNodeDefinitions("Event_"))
            {
                if (definition.fullName == "Event_Custom")
                    continue;

                var str = definition.fullName.Substring("Event_".Length);
                var name = str.ToCharArray().Select((w, i) => i == 0 ? char.ToLowerInvariant(w) : w);

                if (!builtinEvents.ContainsKey(str))
                {
                    var lowercased = string.Join("", name);
                    builtinEvents.Add(str, "_" + lowercased);
                }
            }

            var sb = new StringBuilder();
            foreach (var definition in nodeDefinitions)
                sb.AppendLine(definition);

            var assemblies = AssetDatabase.FindAssets("t:asmdef");

            foreach (var assembly in assemblies.Select(AssetDatabase.GUIDToAssetPath))
            {
                if (!assembly.StartsWith("Assets"))
                    continue;

                var path = Path.GetDirectoryName(assembly);
                var shipped = Path.Combine(path, "PublicAPI.Shipped.txt");
                using (var sw = new StreamWriter(shipped))
                    sw.WriteLine(sb.ToString());

                AssetDatabase.Refresh();

                var obj = AssetDatabase.LoadAssetAtPath<TextAsset>(shipped);
                AssetDatabase.SetLabels(obj, new[] { "RoslynAdditionalFiles" });
            }

            // for Assembly-CSharp.csproj
            {
                var shipped = Path.Combine("Assets", "PublicAPI.Shipped.txt");
                using (var swr = new StreamWriter(shipped))
                    swr.WriteLine(sb.ToString());

                AssetDatabase.Refresh();

                var obj = AssetDatabase.LoadAssetAtPath<TextAsset>(shipped);
                AssetDatabase.SetLabels(obj, new[] { "RoslynAdditionalFiles" });
            }
        }
    }
}