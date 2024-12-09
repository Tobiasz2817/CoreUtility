using System.Linq;
using UnityEngine;

namespace CoreUtility.Extensions {
    public static class CompareExtensions {
        public static Transform[] EqualTransforms(this Transform[] objects, params string[] names) {
            var obj = objects?.Where(go => {
                return names.All(part =>
                    go.name.IndexOf(part, System.StringComparison.OrdinalIgnoreCase) != -1);
            });

            return obj?.ToArray();
        }
    }
}