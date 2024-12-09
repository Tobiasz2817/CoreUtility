using UnityEditor;
using UnityEngine;

namespace CoreUtility.Editor {
    
    
    [CustomPropertyDrawer(typeof(TestDr))]
    public class TestDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Wysokość pojedynczej linii
            float lineHeight = EditorGUIUtility.singleLineHeight;
        
            // Zmienna do iterowania przez właściwości
            SerializedProperty iterator = property.Copy();
            iterator.NextVisible(true); // Zacznij od pierwszej widocznej właściwości

            // Pozycja, na której zaczniemy rysowanie
            float yOffset = position.y;

            // Iteracja przez wszystkie właściwości w obiekcie
            while (iterator.NextVisible(false))  // Następna widoczna właściwość
            {
                // Stwórz prostokąt na podstawie bieżącej pozycji
                Rect propertyRect = new Rect(position.x, yOffset, position.width, lineHeight);
            
                // Jeśli to jest 'newVar', wyświetl ją pomiędzy 'd' i 'y'
                if (iterator.name == "newVar")
                {
                    EditorGUI.PropertyField(propertyRect, iterator, true);
                    yOffset += lineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
                else
                {
                    // Rysowanie innych zmiennych
                    EditorGUI.PropertyField(propertyRect, iterator, true);
                    yOffset += lineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            // Obliczanie wysokości na podstawie liczby właściwości
            float height = 0;
            SerializedProperty iterator = property.Copy();
            iterator.NextVisible(true); // Zacznij od pierwszej widocznej właściwości

            while (iterator.NextVisible(false)) {
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }

            return height;
        }
    }
}