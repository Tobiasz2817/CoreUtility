using System;
using System.Collections;
using UnityEngine;

namespace CoreUtility {
    public class StaticCoroutine : StaticPersistent<StaticCoroutine> {
        void Awake() =>
            Application.quitting += StopAllCoroutines;
        void OnDestroy() => 
            StopAllCoroutines();
        public static Coroutine RunCoroutine(IEnumerator enumerator) =>
            Instance.StartCoroutine(enumerator);
        
        public static Coroutine RunCoroutine(string name) =>
            Instance.StartCoroutine(name);
        
        public static Coroutine RunCoroutine(string name, object val) =>
            Instance.StartCoroutine(name, val);

        public static void AbortCoroutine(Coroutine coroutine) {
            if (coroutine == null)
                return;
            
            Instance.StopCoroutine(coroutine);
        }
        
        public static void AbortCoroutine(string name) {
            if (string.IsNullOrEmpty(name))
                return;
            
            Instance.StopCoroutine(name);
        }
    }
}