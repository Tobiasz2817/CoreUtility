using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;

namespace CoreUtility.Extensions
{
    public static class TypeExtensions
    {
        public static bool Inherits(this System.Type type, System.Type to) => to.IsAssignableFrom(type);
        public static bool IsStruct(this Type type) => type.IsValueType && !type.IsEnum;    
     
        public static bool IsAssigned(this object value) {
            if (value == null)
                return true;
            
            var type = value.GetType();
            if (type.IsClass) 
                return value is not UnityEngine.Object val || !val;
            
            return type.IsStruct() && Equals(value, Activator.CreateInstance(type));
        }
        
        public static IEnumerable<Type> GetDerivedTypes(this Type type, bool withBaseType = true) =>
            AppDomain.CurrentDomain.GetAssemblies().
                Where(asm => UtilityAssemblies.Interfaces.Any(include => asm.FullName.StartsWith(include))).
                SelectMany(s => s.GetTypes()).
                Where(t => type.IsAssignableFrom(t) && (withBaseType || t != type));

        public static bool ContainsConstructor(this Type type, bool onlyDefault = false) =>
            type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).
                Where(constr => !onlyDefault || constr.GetParameters().Length == 0).ToList().Count > 0;
    }
}