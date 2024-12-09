using System;
using System.Collections;
using UnityEngine;

namespace CoreUtility {
    public static class StaticTimer {
        public static void RunCountdown(float duration, Func<bool> condition = null, Action onComplete = null, Action<float> onTick = null) {
            var timerData = new TimerData {
                Duration = duration,
                RunType = TimerType.CountDown,
                Condition = condition
            };

            if(onComplete != null)
                timerData.OnComplete += onComplete;
            if (onTick != null) 
                timerData.OnTick += onTick;
            
            StaticCoroutine.RunCoroutine(ProcessTimer(timerData));
        }

        static IEnumerator ProcessTimer(TimerData timerData) {
            //TODO: based on TimerType
            var time = timerData.Duration;
            timerData.OnTick?.Invoke(time / timerData.Duration);
            while (time > 0) {
                if(timerData.Condition != null && timerData.Condition.Invoke())
                    break;
                
                time -= Time.deltaTime;
                timerData.OnTick?.Invoke(time / timerData.Duration);
                yield return null;
            }
            
            timerData.OnComplete?.Invoke();
        }
    }

    public struct TimerData {
        internal float Duration;
        internal TimerType RunType;
        internal Action OnComplete;
        internal Action<float> OnTick;
        internal Func<bool> Condition;
    }

    enum TimerType {
        CountDown
    }
}