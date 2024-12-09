using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CoreUtility.Extensions {
    public static class TransformExtensions {
        /// <summary>
        /// Retrieves all the children of a given Transform.
        /// </summary>
        /// <remarks>
        /// This method can be used with LINQ to perform operations on all child Transforms. For example,
        /// you could use it to find all children with a specific tag, to disable all children, etc.
        /// Transform implements IEnumerable and the GetEnumerator method which returns an IEnumerator of all its children.
        /// </remarks>
        /// <param name="parent">The Transform to retrieve children from.</param>
        /// <returns>An IEnumerable&lt;Transform&gt; containing all the child Transforms of the parent.</returns>    
        public static IEnumerable<Transform> Children(this Transform parent) {
            foreach (Transform child in parent) {
                yield return child;
            }
        }

        /// <summary>
        /// Resets transform's position, scale and rotation
        /// </summary>
        /// <param name="transform">Transform to use</param>
        public static void Reset(this Transform transform) {
            transform.position = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
        
        public static void ResetLocal(this Transform transform) {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        /// <summary>
        /// Destroys all child game objects of the given transform.
        /// </summary>
        /// <param name="parent">The Transform whose child game objects are to be destroyed.</param>
        public static void DestroyChildren(this Transform parent) {
            parent.ForEveryChild(child => Object.Destroy(child.gameObject));
        }

        /// <summary>
        /// Immediately destroys all child game objects of the given transform.
        /// </summary>
        /// <param name="parent">The Transform whose child game objects are to be immediately destroyed.</param>
        public static void DestroyChildrenImmediate(this Transform parent) {
            parent.ForEveryChild(child => Object.DestroyImmediate(child.gameObject));
        }

        /// <summary>
        /// Enables all child game objects of the given transform.
        /// </summary>
        /// <param name="parent">The Transform whose child game objects are to be enabled.</param>
        public static void EnableChildren(this Transform parent) {
            parent.ForEveryChild(child => child.gameObject.SetActive(true));
        }

        /// <summary>
        /// Disables all child game objects of the given transform.
        /// </summary>
        /// <param name="parent">The Transform whose child game objects are to be disabled.</param>
        public static void DisableChildren(this Transform parent) {
            parent.ForEveryChild(child => child.gameObject.SetActive(false));
        }

        /// <summary>
        /// Executes a specified action for each child of a given transform.
        /// </summary>
        /// <param name="parent">The parent transform.</param>
        /// <param name="action">The action to be performed on each child.</param>
        /// <remarks>
        /// This method iterates over all child transforms in reverse order and executes a given action on them.
        /// The action is a delegate that takes a Transform as parameter.
        /// </remarks>
        public static void ForEveryChild(this Transform parent, System.Action<Transform> action) {
            for (var i = parent.childCount - 1; i >= 0; i--) {
                action(parent.GetChild(i));
            }
        }

        [Obsolete("Renamed to ForEveryChild")]
        static void PerformActionOnChildren(this Transform parent, System.Action<Transform> action) {
            parent.ForEveryChild(action);
        }
        
        public static bool FindInSelfOrParent<T>(this Transform transform, out T reference) {
            transform.TryGetComponent(out T t);
            reference = t != null ? t : transform.GetComponentInParent<T>();
            return reference != null;
        }
        
        public static bool FindInSelfOrChildren<T>(this Transform transform, out T reference) {
            transform.TryGetComponent(out T t);
            reference = t != null ? t : transform.GetComponentInChildren<T>();
            return reference != null;
        }
        
        public static bool FindInSelfAndChildren<T>(this Transform transform, out List<T> reference) {
            reference = transform.GetComponentsInChildren<T>().ToList();
            
            if(transform.TryGetComponent(out T t))
                reference.Insert(0, t);

            return reference.Count > 0;
        }
        
        public static Transform[] FindObjects(this Transform researchObject, params string[] names) {
            return researchObject.GetComponentsInChildren<Transform>().EqualTransforms(names);
        }
        
        public static T GetComponent<T>(this Transform transform, ReadType readingType) {
            if(readingType.HasFlag(ReadType.Self)) 
                return transform.GetComponent<T>();
            if(readingType.HasFlag(ReadType.Children))
                return transform.GetComponentInChildren<T>();
                
            return transform.GetComponentInParent<T>();
        }
        
        public static T[] GetComponents<T>(this Transform transform, ReadType readingType) =>
            transform.GetComponentsToList<T>(readingType).ToArray();
        
        public static List<T>GetComponentsToList<T>(this Transform transform, ReadType readingType) {
            List<T> list = new List<T>();

            if(readingType.HasFlag(ReadType.Self)) {
                var t1 = transform.GetComponent<T>();
                if(t1 != null)
                    list.Add(t1);
            }
            if(readingType.HasFlag(ReadType.Children))
                list.AddRange(transform.GetComponentsInChildren<T>(true));
            if(readingType.HasFlag(ReadType.Parent)) 
                list.AddRange(transform.GetComponentsInParent<T>());
            
            return list;
        }
    }
    
    [Flags]
    public enum ReadType
    {
        Self = 1 << 1,
        Children = 1 << 2,
        Parent = 1 << 3
    }
}