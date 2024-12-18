using System.Collections.Generic;
using CoreUtility.Extensions;
using UnityEditor;
using System.Linq;
using UnityEngine;
using System.IO;

namespace CoreUtility {
    public class Assemblify : MonoBehaviour {
        [MenuItem("Tools/ScanAssemblies")]
        static void ScanAssemblies() {
            Dictionary<string, string[]> assemblies = new(){
                // provided assembly - which reference want the assembly
                {"CoreUtility", new []{"Signals", "Ability", "CharControl2D", "Inflowis", "Tools", "Storex"}}, 
                {"Signals", new []{"Ability", "CharControl2D"}},
                {"Storex", new []{"Ability"}},
            };
            
            foreach (var assembly in assemblies) {
                var path = GetAssemblyPath(assembly.Key);
                if (string.IsNullOrEmpty(path)) {
                    Debug.LogWarning($"Didn't find the assembly definition asset of name: {assembly.Key}");
                    continue;
                }
                
                string targetGuid = AssetDatabase.AssetPathToGUID(path);
                foreach (var targetAssembly in assembly.Value) {
                    string targetPath = GetAssemblyPath(targetAssembly);
                    
                    if (string.IsNullOrEmpty(targetPath)) {
                        Debug.LogWarning($"Didn't find the assembly definition asset of name: {targetAssembly}");
                        continue;
                    }

                    var jsonContent = File.ReadAllText(targetPath);
                    var targetData = JsonUtility.FromJson<AssemblyDefinitionData>(jsonContent);
                    if (targetData.references.Contains($"GUID:{targetGuid}"))
                        continue;
                    
                    targetData.references = targetData.references.Add($"GUID:{targetGuid}");
                    
                    var json = JsonUtility.ToJson(targetData, true);
                    File.WriteAllText(targetPath, json);
                }
                
            }
            
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        static string GetAssemblyPath(string assemblyName) =>
            AssetDatabase.GUIDToAssetPath(            
                AssetDatabase.FindAssets($"{assemblyName} t: AssemblyDefinitionAsset", null).FirstOrDefault());
    }
    
    //TODO: Add more parameters
    // Without parameters on scanning remove others params
    [System.Serializable]
    public class AssemblyDefinitionData {
        public string name;
        public string rootNamespace;
        public string[] references;
        public string[] includePlatforms;
        public string[] excludePlatforms;
        public bool allowUnsafeCode;
        public bool overrideReferences;
        public string[] precompiledReferences;
        public bool autoReferenced;
        public string[] defineConstraints;
        public string[] versionDefines;
        public bool noEngineReferences;
    }
}
