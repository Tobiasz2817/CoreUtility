using CoreUtility.Extensions;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System;

namespace CoreUtility.Editor {
    public class ShowInterfaceAttribute : PropertyAttribute { }
    //TODO: Make custom serializer for everyone type
    //TODO: Including struct/interface
    //TODO: Current is showing and give ability to show interface 
    //[CustomPropertyDrawer(typeof(object), true)]
    [CustomPropertyDrawer(typeof(ShowInterfaceAttribute), true)]
    public class InterfaceDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            float height = 0f;
            if (property.hasVisibleChildren) {
                var subProperty = property.Copy();
                var nextElement = property.GetEndProperty();

                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                while (subProperty.NextVisible(true) && !SerializedProperty.EqualContents(subProperty, nextElement)) {
                    height += EditorGUI.GetPropertyHeight(subProperty) + EditorGUIUtility.standardVerticalSpacing;
                }
            }
            else {
                height = EditorGUI.GetPropertyHeight(property, true);
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var fieldType = fieldInfo.FieldType;
            
            if (!fieldType.IsInterface) {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            if (property.propertyType != SerializedPropertyType.ManagedReference) {
                EditorGUI.LabelField(position, $"{label.text}: Not a SerializeReference field");
                return;
            }

            var types = fieldType.GetDerivedTypes(false).ToArray();

            if (!types.Any()) {
                EditorGUI.LabelField(position, $"{label.text}: No implementable types found");
                return;
            }
            
            var currentTypeName = property.managedReferenceValue?.GetType().FullName;
            
            Rect popupRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            int selectedIndex = EditorGUI.Popup(popupRect, label.text,
                Mathf.Clamp(Array.FindIndex(types, t => t.FullName == currentTypeName), 0, types.Length - 1),
                types.Select(t => t.Name).ToArray());

            var selectedType = types[selectedIndex];
            if (currentTypeName != selectedType.FullName) {
                property.serializedObject.Update();
                property.managedReferenceValue = Activator.CreateInstance(selectedType);
                property.serializedObject.ApplyModifiedProperties();
            }

            ShowFields(property, position, true);
        }

        void ShowFields(SerializedProperty property, Rect position, bool indentFields = false) {
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            SerializedProperty iterator = property.Copy(); 
            SerializedProperty endProperty = property.GetEndProperty();

            if(indentFields)
                EditorGUI.indentLevel++;

            iterator.NextVisible(true);

            while (!SerializedProperty.EqualContents(iterator, endProperty))
            {
                position.height = EditorGUI.GetPropertyHeight(iterator, true);
                EditorGUI.PropertyField(position, iterator, true);
                position.y += position.height + EditorGUIUtility.standardVerticalSpacing;

                iterator.NextVisible(false);
            }

            if(indentFields)
                EditorGUI.indentLevel--;
        }
    }
}