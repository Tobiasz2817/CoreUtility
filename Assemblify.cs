using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoreUtility.Extensions;
using UnityEditor;
using UnityEngine;

namespace CoreUtility {
    public class Assemblify : MonoBehaviour {
        static Dictionary<string, string[]> _assemblies = new(){
            // Wanted - Target 
            {"CoreUtility", new []{"Signals", "Ability", "CharControl2D", "Inflowis", "Tools", "Storex"}}, 
            {"Signals", new []{"Ability", "CharControl2D"}},
            {"Storex", new []{"Ability"}},
        };
        
        [MenuItem("Tools/ScanAssemblies")]
        static void ScanAssemblies() {
            foreach (var assembly in _assemblies) {
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
    [System.Serializable]
    public class AssemblyDefinitionData
    {
        public string name;
        public string[] references;
        public string[] optionalUnityReferences;
        public string[] versionDefines;
        public bool allowUnsafeCode;
    }
}
