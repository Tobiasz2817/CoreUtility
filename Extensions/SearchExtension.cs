using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CoreUtility.Extensions
{
    public static class SearchExtension
    {
        /// <summary>
        /// Finds the closest object on the screen based on the specified X direction. 
        /// With Filter: 
        /// First -> taking object by tolerance
        /// Next -> Sorting by the closest object on y axis
        /// Next -> Sorting by on the x axis.
        /// X direction should be in the range from -1 to 1.
        /// </summary>
        public static GameObject FindClosestByScreenPosX(this IEnumerable<GameObject> objects, Vector2 currentPosition, float xDirection, float tolerance)  {
            return objects
                .Where(obj => Mathf.Clamp(xDirection ,-1, 1) * (obj.transform.position.x - currentPosition.x) > tolerance)
                .OrderBy(obj => Mathf.Abs(obj.transform.position.y - currentPosition.y))
                .ThenBy(obj => Mathf.Abs(obj.transform.position.x - currentPosition.x))
                .FirstOrDefault();
        }
        
        
        /// <summary>
        /// Finds the closest object on the screen based on the specified X direction. 
        /// With Filter: 
        /// First -> taking object by tolerance
        /// Next -> Sorting by the closest object on x axis
        /// Next -> Sorting by on the y axis.
        /// Y direction should be in the range from -1 to 1.
        /// </summary>
        public static GameObject FindClosestByScreenPosY(this IEnumerable<GameObject> objects, Vector2 currentPosition, float yDirection, float tolerance) {
            return objects
                .Where(obj => Mathf.Clamp(yDirection ,-1, 1) * (obj.transform.position.y - currentPosition.y) > tolerance)
                .OrderBy(obj => Mathf.Abs(obj.transform.position.x - currentPosition.x))
                .ThenBy(obj => Mathf.Abs(obj.transform.position.y - currentPosition.y))
                .FirstOrDefault();
        }
    }
}