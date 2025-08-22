using System.Linq;
using System.Xml.Linq;

using UnityEditor;

namespace NatsunekoLaboratory.UdonAnalyzer.Hooks
{
    public class SolutionGeneratorHook : AssetPostprocessor
    {
        public static string OnGeneratedCSProject(string path, string content)
        {
            var document = XDocument.Parse(content);
            var @namespace = (XNamespace)"http://schemas.microsoft.com/developer/msbuild/2003";
            var project = document.Element(@namespace + "Project");

            // for Visual Studio Code (VSCode generated xmlns=None csproj)
            if (project == null)
            {
                project = document.Element("Project");
                @namespace = XNamespace.None;
            }

            var itemGroup = new XElement(@namespace + "ItemGroup");

            {
                var analyzers = AssetDatabase.FindAssets("l:RoslynAnalyzer").Select(AssetDatabase.GUIDToAssetPath).ToArray();
                foreach (var analyzer in analyzers)
                {
                    var include = new XAttribute("Include", analyzer);
                    var reference = new XElement(@namespace + "Analyzer", include);

                    itemGroup.Add(reference);
                }
            }

            {
                var additionalFiles = AssetDatabase.FindAssets("l:RoslynAdditionalFiles")
                                                   .Select(AssetDatabase.GUIDToAssetPath)
                                                   .Select(w => w.Replace("/", "\\"))
                                                   .ToArray();

                var items = project.Descendants(@namespace + "ItemGroup")
                                   .SelectMany(w => w.Descendants(@namespace + "None"))
                                   .Where(w => additionalFiles.Contains((string)w.Attribute("Include")))
                                   .ToArray();

                foreach (var item in items)
                {
                    var attr = new XElement(@namespace + "AdditionalFiles", new XAttribute("Include", (string)item.Attribute("Include")));
                    item.ReplaceWith(attr);
                }
            }

            project?.Add(itemGroup);

            return document.ToString();
        }
    }
}
