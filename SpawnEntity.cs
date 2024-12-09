using System;
using UnityEngine;

namespace CoreUtility {
    public static class Entity {
        public static event Action<GameObject> OnSpawn;
        public static event Action<GameObject> OnDestroy;
        
        public static GameObject Spawn(GameObject prefab, Vector3? position = null, Quaternion? rotation = null, Transform parent = null) {
            var target = UnityEngine.Object.Instantiate(prefab, position ?? Vector3.zero,
                rotation ?? Quaternion.identity, parent);
            
            OnSpawn?.Invoke(target);
            return UnityEngine.Object.Instantiate(prefab, position ?? Vector3.zero, rotation ?? Quaternion.identity, parent);
        }

        public static void Destroy(GameObject prefab, float timeToDestroy) {
            OnDestroy?.Invoke(prefab);
            
            UnityEngine.Object.Destroy(prefab, timeToDestroy);
        }
    } 
}