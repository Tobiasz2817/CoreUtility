using System.Collections.Generic;
using UnityEngine;

namespace CoreUtility.Extensions {
    public static class CollisionExtension {
        /// <summary>
        /// Creating collider 2D with layer
        /// Will expand a layers length
        /// </summary>
        public static BoxCollider2D WithLayer(this BoxCollider2D collider2D, LayerMask targetMask) {
            var colliderLayers = collider2D.includeLayers;
            if (!colliderLayers.Contains(targetMask)) 
                collider2D.includeLayers = colliderLayers.Add(targetMask);
            
            return collider2D;
        }
        
        /// <summary>
        /// Return the values of 3 state: IsGrounded, IsWalling, IsCelling    
        /// </summary>
        /// <returns> order of the return values
        /// IsGrounded
        /// IsWalling
        /// IsCelling
        /// </returns>
        public static (bool, int, bool) GetSurfaceContacts(this ContactPoint2D[] contacts, float collisionThreshold = 0.1f) {
            var states = (false, 0, false);
            
            contacts.ForEach((contact) => {
                states.Item1 = states.Item1 || contact.normal.y > collisionThreshold;
                states.Item2 = states.Item2 != 0 ? (int)contact.normal.y : states.Item2;
                states.Item3 = states.Item3 || (states.Item1 || contact.normal.y < collisionThreshold);
            });
            
            return states;
        }
        
        /// <returns> order of the return values
        /// IsGrounded
        /// IsWalling
        /// IsCelling
        /// </returns>
        public static (bool, int, bool) GetSurfaceContacts(Vector2 normal, float collisionThreshold = 0.1f) => (
            normal.y > collisionThreshold, 
            (int)normal.y,
            normal.y > collisionThreshold || normal.y < collisionThreshold);
        
        
        public static (bool, int, bool) GetSurfaceContacts(this IEnumerable<Vector2> contacts, float collisionThreshold = 0.1f) {
            var states = (false, 0, false);
            
            contacts.ForEach((contact) => {
                states.Item1 = states.Item1 || contact.y > collisionThreshold;
                states.Item2 = states.Item2 != 0 ? (int)contact.y : states.Item2;
                states.Item3 = states.Item3 || (states.Item1 || contact.y < collisionThreshold);
            });

            
            return states;
        }
    }
}