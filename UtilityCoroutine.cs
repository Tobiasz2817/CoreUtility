using System;
using System.Collections;
using UnityEngine;

namespace CoreUtility {
    public static class UtilityCoroutine {
        public static Coroutine RunAction(float duration = 0, Action action = null) =>
            StaticCoroutine.RunCoroutine(RunActionCoroutine(action, duration));
        
        static IEnumerator RunActionCoroutine(Action action, float duration) {
            yield return new WaitForSeconds(duration);
            action?.Invoke();
        }
        
        public static Coroutine RunOnValueChange(Func<bool> tickCondition, params (Action action, Func<bool> valueReference, bool? targetValue, bool current)[] valueData) =>
             StaticCoroutine.RunCoroutine(OnValueChange(tickCondition ,valueData));
        
        static IEnumerator OnValueChange(Func<bool> tickCondition, params (Action action, Func<bool> valueReference, bool? targetValue, bool current)[] valueData) {
            while (tickCondition.Invoke()) {
                for (var i = 0; i < valueData.Length; i++) {
                    var data = valueData[i];
                    var refValue = data.valueReference.Invoke();
                    
                    if (data.current != refValue) {
                        var tmpValue = data.current;
                        valueData[i].current = refValue;
                        
                        if(data.targetValue != null && data.targetValue == tmpValue) 
                            continue;
                        
                        data.action.Invoke();
                    }
                }

                yield return null;   
            }
        }
    }
}