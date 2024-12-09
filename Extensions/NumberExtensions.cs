using UnityEngine;
#if ENABLED_UNITY_MATHEMATICS
using Unity.Mathematics;
#endif

namespace CoreUtility.Extensions {
    public static class NumberExtensions {
        public static float PercentageOf(this int part, int whole) {
            if (whole == 0) return 0; // Handling division by zero
            return (float) part / whole;
        }

        public static int AtLeast(this int value, int min) => Mathf.Max(value, min);
        public static int AtMost(this int value, int max) => Mathf.Min(value, max);

#if ENABLED_UNITY_MATHEMATICS
        public static half AtLeast(this half value, half max) => MathfExtension.Max(value, max);
        public static half AtMost(this half value, half max) => MathfExtension.Min(value, max);
#endif

        public static float AtLeast(this float value, float min) => Mathf.Max(value, min);
        public static float AtMost(this float value, float max) => Mathf.Min(value, max);

        public static double AtLeast(this double value, double min) => MathF.Max(value, min);
        public static double AtMost(this double value, double min) => MathF.Min(value, min);
        
        [Tooltip("From 1% - 100%")]
        public static int IntRoundPercentFrom(this int value, float percent, bool isRoundToUpper = true) {
            var percentValue = PercentFrom(value, percent);
            var roundTo = isRoundToUpper ? Mathf.Ceil(percentValue) : Mathf.Clamp(percentValue, 1, int.MaxValue);
            
            return Mathf.RoundToInt(roundTo);
        }
        
        [Tooltip("From 1% - 100%")]
        public static float PercentFrom(this int value, float percent) {
            var x1 = value * percent;
            const int x2 = 100;
            
            var x = x1 / x2;

            return x;
        }
    }
}