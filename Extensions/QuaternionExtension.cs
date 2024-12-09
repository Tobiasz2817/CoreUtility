using UnityEngine;

namespace CoreUtility.Extensions
{
    public static class QuaternionExtension {
        public static Quaternion With(this Quaternion vector, float? x = null, float? y = null, float? z = null) {
            return Quaternion.Euler(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        }
    }
}