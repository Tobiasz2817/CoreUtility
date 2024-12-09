using UnityEngine;

namespace CoreUtility.Extensions {
    public static class RigidbodyExtension {
        public static bool IsGravityEqual(this Rigidbody2D rb2, float gravityValue) =>  
            Mathf.Approximately(rb2.gravityScale, gravityValue);

        public static bool SetGravityScale(this Rigidbody2D rigidbody2D, float gravityScale, bool condition = true) {
            if (condition && !Mathf.Approximately(rigidbody2D.gravityScale, gravityScale))
                rigidbody2D.gravityScale = gravityScale;
            
            return condition;
        }
        
        public static bool SetVelocity(this Rigidbody2D rigidbody2D, float? x = null, float? y = null, bool condition = true) {
            if (condition) {
                var vel = rigidbody2D.linearVelocity;
                var xVal = x ?? vel.x;
                var yVal = y ?? vel.y;
                
                if (Mathf.Approximately(vel.x, xVal) && Mathf.Approximately(vel.y, yVal))
                    return false;
                
                rigidbody2D.linearVelocity = new Vector2(xVal, yVal);
            }

            return condition;
        }
    }
}