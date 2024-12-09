using UnityEditor;

namespace CoreUtility.Extensions {
    public static class HandleExtension {
        public static Handles.CapFunction CapFunction(this Shape shape) {
            switch (shape) {
                case Shape.Circle:
                    return Handles.CircleHandleCap;
                case Shape.Cone:
                    return Handles.ConeHandleCap;
                case Shape.Cube:
                    return Handles.CubeHandleCap;
                case Shape.Cylinder:
                    return Handles.CylinderHandleCap;
                case Shape.Rectangle:
                    return Handles.RectangleHandleCap;
                case Shape.Arrow:
                    return Handles.ArrowHandleCap;
                case Shape.Dot:
                    return Handles.DotHandleCap;
                case Shape.Sphere:
                    return Handles.SphereHandleCap;
                
            }

            return Handles.SphereHandleCap;
        }
    }
    
    public enum Shape {
        Circle = 0,
        Cone = 1,
        Cube = 2,
        Cylinder = 3,
        Rectangle = 4,
        Arrow = 5,
        Dot = 6,
        Sphere = 7
    }
}