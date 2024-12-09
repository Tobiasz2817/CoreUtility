using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CoreUtility.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CoreUtility {
    public static class Utility{
        public static void ClearConsoleLogs() {
            var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method?.Invoke(new object(), null);
        }
        
        /// <summary>
        /// Return the values of 3 state: IsGrounded, IsWalling, IsCelling    
        /// </summary>
        /// <returns> order of the return values
        /// IsGrounded
        /// Wall Id
        /// IsCelling
        /// </returns>
        public static (bool, int, bool) GetSurfaceContacts(Vector2 normal, float collisionThreshold = 0.1f) => (
            normal.y > collisionThreshold, 
            (int)normal.x,
            normal.y < -collisionThreshold);
        
        // TODO: Obsolete in unity 6
        [Obsolete("Obsolete")]
        public static bool TryAddDefineSymbol(string symbolName) {
            var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);

            if (defines.Contains(symbolName)) 
                return false;
            
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines + ";" + symbolName);
            return true;
        }

        public static T FindInScene<T>(Scene scene) {
            var rootObjects = scene.GetRootGameObjects();

            foreach (var rootObject in rootObjects) {
                var self = rootObject.GetComponent<T>();
                T component = self != null ? self : rootObject.GetComponentInChildren<T>();
                if (component != null) 
                    return component;
            }

            return default; 
        }
        
        public static T FindOrAddInScene<T>(Scene scene, bool allowPrefabs = true) where T: Component{
            foreach (var rootObject in scene.GetRootGameObjects()) {
                var component = rootObject.GetComponentInChildren<T>();

                if (!allowPrefabs && !PrefabUtility.IsPartOfPrefabAsset(rootObject)) 
                    continue;

                return component;
            }

            return new GameObject(typeof(T).Name).AddComponent<T>(); 
        }
        
        //TODO: Check if work propertly
        public static MonoBehaviour[] FindInScene(Scene scene, params Type[] types) {
            var componentsList = new List<MonoBehaviour>();

            GameObject[] rootObjects = scene.GetRootGameObjects();

            foreach (var rootObject in rootObjects) {
                foreach (var type in types) {
                    var components = rootObject.GetComponentsInChildren(type, true);
                    foreach (var component in components) 
                        if (component is MonoBehaviour monoBehaviour)
                            componentsList.Add(monoBehaviour); 
                }
            }

            return componentsList.ToArray();
        }
        
        public static bool IsUserScript(MonoBehaviour component) {
            var assembly = component.GetType().Assembly;
            return assembly != typeof(MonoBehaviour).Assembly &&
                   !assembly.FullName.StartsWith("UnityEngine") &&
                   !assembly.FullName.StartsWith("UnityEditor") &&
                   !assembly.FullName.StartsWith("Unity");
        }
        
        public static async Task WaitUntil(Func<bool> condition) {
            while (condition.Invoke()) 
                await Task.Yield();

            await Task.Delay(1);
        }
    } 
}