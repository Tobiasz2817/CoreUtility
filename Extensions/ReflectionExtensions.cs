using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace CoreUtility.Extensions {
    public static class ReflectionExtensions {
        public static bool EqualParameters(this MethodInfo methodInfo, MethodInfo equalTo, bool showMessage = false) {
            bool equal = methodInfo.GetParameters()
                .Select(p => p.ParameterType)
                .SequenceEqual(equalTo.GetParameters().Select(p => p.ParameterType));
#if UNITY_EDITOR
            if (showMessage && !equal) 
                Debug.LogWarning($"Method: {methodInfo.Name} has different parameters than {equalTo.Name}. ");
#endif
            return equal;
        }

        public static bool IsMemberType(this MemberInfo info, MemberInfo targetInfo) => 
            IsMemberType(info, targetInfo.GetMemberType());
        
        public static bool IsMemberType(this MemberInfo info, Type targetInfo) {
            if (info == null || targetInfo == null) {
                return false;
            }
            
            return info.GetMemberType() == targetInfo;
        }
        
        public static void SetMemberValue(this MemberInfo member, object obj, object value) {
            if (member as FieldInfo != null) {
                ((FieldInfo)member).SetValue(obj, value);
            }
            else {
                MethodInfo methodInfo = member as PropertyInfo != null ? ((PropertyInfo)member).GetSetMethod(true) : throw new ArgumentException("Can't set value of " + member.GetType().Name);
                if (!(methodInfo != null))
                    throw new ArgumentException("Property " + member.Name + " has no setter");
                
                methodInfo.Invoke(obj, new object[1]{ value });
            }
        }
        
        public static object GetMemberValue(this MemberInfo member, object obj) {
            if (member as FieldInfo != null)
                return ((FieldInfo)member).GetValue(obj);

            if (member is not PropertyInfo info) return null;
            
            return info.GetGetMethod(true).Invoke(obj,null) ?? info.GetValue(obj) ?? 
                    throw new ArgumentException("Can't get value of " + info.GetType().Name);
        }
        
        public static Type GetMemberType(this MemberInfo memberInfo) {
            switch (memberInfo.MemberType) {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).PropertyType;
                case MemberTypes.Method:
                    return ((MethodInfo)memberInfo).ReturnType;
                case MemberTypes.Event:
                    return ((EventInfo)memberInfo).EventHandlerType;
                case MemberTypes.Constructor:
                    return memberInfo.DeclaringType; 
                default:
                    throw new NotImplementedException($"Comparison not implemented for MemberType {memberInfo.MemberType}");
            }
        }
        
        public static List<MemberInfo> GetMembers(this object injector, MemberTypes types = default, BindingFlags bindingFlags = default) {
            var members = new List<MemberInfo>();
            var sourceType = injector as Type ?? injector.GetType();

            if (types.HasFlag(MemberTypes.Field)) 
                members.AddRange(sourceType.GetFields(bindingFlags));
            if (types.HasFlag(MemberTypes.Property)) 
                members.AddRange(sourceType.GetProperties(bindingFlags));
            //TODO: Future new types implement

            return members;
        }
    }
}