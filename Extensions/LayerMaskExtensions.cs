using UnityEngine;

namespace CoreUtility.Extensions
{
    public static class LayerMaskExtensions {
        /// <summary>
        /// Checks if the given layer number is contained in the LayerMask.
        /// </summary>
        /// <param name="mask">The LayerMask to check.</param>
        /// <param name="layerNumber">The layer number to check if it is contained in the LayerMask.</param>
        /// <returns>True if the layer number is contained in the LayerMask, otherwise false.</returns>
        public static bool Contains(this LayerMask mask, int layerNumber) {
            return mask == (mask | (1 << layerNumber));
        }
        
        /// <summary>
        /// Connect passed layer with wanted layer number 
        /// </summary>
        /// <param name="mask">The LayerMask to check.</param>
        /// <param name="extendNumber">The layer number to check if it is contained in the LayerMask.</param>
        /// <returns>Return new LayerMask with extend layer</returns>
        public static LayerMask Add(this LayerMask mask, int extendNumber) {
            return mask | extendNumber;
        }
        
        /// <summary>
        /// Removed from layer mask other mask
        /// </summary>
        /// <param name="mask">The LayerMask to check.</param>
        /// <param name="removeNumber">The layer number to check if it is contained in the LayerMask.</param>
        /// <returns>Return new LayerMask without removed layer</returns>
        public static LayerMask Remove(this LayerMask mask, int removeNumber) {
            return mask & removeNumber;
        }
    }
}