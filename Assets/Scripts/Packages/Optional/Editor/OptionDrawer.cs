using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Optional.Editor
{
    [CustomPropertyDrawer(typeof(Option<>))]
    public class OptionDrawer : PropertyDrawer
    {
        private static GUIStyle WarningStyle;
        private static GUIStyle ButtonStyle;

        static OptionDrawer()
        {
            WarningStyle = new GUIStyle();
            WarningStyle.normal.textColor = Color.yellow;
            WarningStyle.fontSize = 16;

            ButtonStyle = new GUIStyle();
            ButtonStyle.padding = new RectOffset(-6, 4, -6, 4);
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect someButtonRect = new Rect(position)
            {
                xMin = position.xMax - 18
            };
            position.xMax -= 18 + EditorGUIUtility.standardVerticalSpacing;

            var valueProperty = property.FindPropertyRelative("_value");
            var someProperty = property.FindPropertyRelative("_isSome");
            
            if (someProperty.boolValue)
            {
                if (GUI.Button(someButtonRect, EditorGUIUtility.IconContent("PrefabOverlayRemoved Icon", "Toggle to None"), ButtonStyle))
                {
                    someProperty.boolValue = false;
                }
                    
                if (valueProperty.propertyType == SerializedPropertyType.ObjectReference && valueProperty.objectReferenceValue == null)
                {
                    var warningRect = new Rect(position);
                    warningRect.xMin = position.xMax - 16;
                    position.xMax -= 16 + EditorGUIUtility.standardVerticalSpacing;
                    
                    GUI.Label(warningRect, EditorGUIUtility.IconContent("console.warnicon.sml","Cannot store a null value in an Option type"), WarningStyle);
                }
                EditorGUI.PropertyField(position, valueProperty, label);
            }
            else
            {
                if (GUI.Button(someButtonRect, EditorGUIUtility.IconContent("PrefabOverlayAdded Icon", "Toggle to None"), ButtonStyle))
                {
                    someProperty.boolValue = true;
                }

                var labelRect = EditorGUI.PrefixLabel(position, valueProperty.GetHashCode(), label);
                GUI.Label(labelRect, $"None ({valueProperty.GetUnderlyingType().HumanName()})");
            }
            
            EditorGUI.EndProperty();
        }
    }
    
}
