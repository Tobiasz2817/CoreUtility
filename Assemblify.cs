using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace CoreUtility {
    public class Assemblify : MonoBehaviour {
        string[,] _assemblies = new[,]{
            // Wanted - Target 
            {"CoreUtility", "Signals"}, 
            {"CoreUtility", "Ability"}, 
            {"CoreUtility", "CharControl2D"}, 
            {"CoreUtility", "Inflowis"}, 
            {"CoreUtility", "Tools"}, 
        };
        
        [MenuItem("Tools/ScanAssemblies")]
        static void ScanAssemblies() {
            var path = AssetDatabase.GUIDToAssetPath(            
                AssetDatabase.FindAssets("CoreUtility t: AssemblyDefinitionAsset", null)[0]);

            var obj = AssetDatabase.LoadAssetAtPath(path, typeof(AssemblyDefinitionAsset));

            if (obj is AssemblyDefinitionAsset def) {
                Debug.Log(def);
                
                string assetPath = AssetDatabase.GetAssetPath(def);
                string jsonContent = File.ReadAllText(assetPath);

                jsonContent = jsonContent.Replace("\"references\": [", "\"references\": [\"GUID:newGuidHere\",");
            
                File.WriteAllText(assetPath, jsonContent);

                AssetDatabase.Refresh();
                EditorUtility.SetDirty(def);
                AssetDatabase.SaveAssets();
            }
        }
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
