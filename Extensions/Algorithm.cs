using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CoreUtility.Extensions {
    #region Convex Hull
    
    public static class ConvexHull2D {
        public static List<Vector2> JarvisAlgorithm(this List<Vector2> points) {
            var firstPoint = points.OrderBy((point) => point.x).First();
            Debug.Log(firstPoint);
            if (firstPoint == default) return null;
            
            List<Vector2> convexPoints = new List<Vector2>();
            List<Vector2> collinearPoints  = new List<Vector2>();
            convexPoints.Add(firstPoint);

            Vector2 pointOnHull = convexPoints[0];
            Vector2 current = pointOnHull;
            
            //var i = 0;
            while (true) {
                var nextTarget = points[0];
                var endPoint = points[0];
                
                for (int j = 1; j < points.Count; j++) {
                    if (points[j] == current)  
                        continue;

                    var value =  MathF.Cross(current - nextTarget, current - points[j]);
                    switch (value) {
                        case > 0: {
                            collinearPoints = new List<Vector2>();
                            nextTarget = points[j];
                        }
                            break;

                        case 0: {
                            if (Vector2.Distance(current, nextTarget) < Vector2.Distance(current, points[j])) {  
                                collinearPoints.Add(nextTarget);  
                                nextTarget = points[j];  
                            }  
                            else  
                                collinearPoints.Add(points[j]); 
                        }
                            break;
                    }
                }
                
                if (nextTarget == convexPoints[0] || UnityEngine.Input.GetKeyDown(KeyCode.K)) break;
                
                //i++;
                convexPoints.Add(nextTarget);
                current = nextTarget;
            } 

            return convexPoints;
        }
        
        public static List<Vector2> MonotoneChainAlgorithm(List<Vector2> points)
        {
            points.Sort((a, b) => a.x != b.x ? a.x.CompareTo(b.x) : a.y.CompareTo(b.y));

            List<Vector2> lowerHull = new List<Vector2>();
            foreach (var point in points)
            {
                while (lowerHull.Count >= 2 && MathF.Cross(lowerHull[lowerHull.Count - 2], lowerHull[lowerHull.Count - 1], point) <= 0)
                {
                    lowerHull.RemoveAt(lowerHull.Count - 1);
                }
                lowerHull.Add(point);
            }

            List<Vector2> upperHull = new List<Vector2>();
            for (int i = points.Count - 1; i >= 0; i--)
            {
                Vector2 point = points[i];
                while (upperHull.Count >= 2 && MathF.Cross(upperHull[upperHull.Count - 2], upperHull[upperHull.Count - 1], point) <= 0)
                {
                    upperHull.RemoveAt(upperHull.Count - 1);
                }
                upperHull.Add(point);
            }

            upperHull.RemoveAt(upperHull.Count - 1);
            lowerHull.RemoveAt(lowerHull.Count - 1);

            lowerHull.AddRange(upperHull);
            return lowerHull;
        }
        
        public static List<Vector2> GrahamScanAlgorithm(List<Vector2> points) {
            if (points.Count < 3) return null;

            List<Vector2> hull = new List<Vector2>();
            
            int l = 0;
            
            for (int i = 1; i < points.Count; i++)
                if (points[i].x < points[l].x)
                    l = i;

            int p = l, q;
            do {
                hull.Add(points[p]);

                q = (p + 1) % points.Count;

                for (int i = 0; i < points.Count; i++) {
                    var cross = MathF.Cross(points[p], points[i], points[q]);
                    if (cross == 0) continue;

                    if (cross < 0) 
                        q = i;
                }

                p = q;

            } while (p != l);

            return hull;
        }
    }
    
    #endregion
}