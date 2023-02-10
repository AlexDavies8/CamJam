using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace USync.Editor
{
    [CustomPropertyDrawer(typeof(Sync<>))]
    public class SyncPropertyDrawer : PropertyDrawer
    {
        private State _state;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            RestoreState(property, label);
            
            _state.ReorderableList.DoList(position);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            RestoreState(property, null);
            return _state.ReorderableList.GetHeight();
        }

        private void RestoreState(SerializedProperty property, GUIContent label)
        {
            _state ??= new();
            _state.Property ??= property;
            _state.Label ??= label;
            _state.ReorderableList ??= new ReorderableList(property.serializedObject,
                property.FindPropertyRelative("_listeners"), true, true, true, true)
            {
                drawHeaderCallback = DrawListHeader,
                headerHeight = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_value"), true) + EditorGUIUtility.standardVerticalSpacing,
                drawElementCallback = DrawListElement,
                elementHeight = 2 * EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
            };
        }

        private void DrawListElement(Rect rect, int index, bool isactive, bool isfocused)
        {
            var property = _state.Property.FindPropertyRelative("_listeners").GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, property);
        }

        private void DrawListHeader(Rect headerRect)
        {
            Rect labelRect = new Rect(headerRect)
            {
                width = EditorGUIUtility.labelWidth - EditorGUIUtility.standardVerticalSpacing
            };
            Rect propRect = new Rect(headerRect)
            {
                xMin = labelRect.xMax + EditorGUIUtility.standardVerticalSpacing
            };
            EditorGUI.LabelField(labelRect, _state.Label);
            GUI.enabled = false;
            EditorGUI.PropertyField(propRect, _state.Property.FindPropertyRelative("_value"), GUIContent.none, true);
            GUI.enabled = true;
        }

        private class State
        {
            public SerializedProperty Property;
            public GUIContent Label;
            public ReorderableList ReorderableList;
        }
    }

    [CustomPropertyDrawer(typeof(SyncListener<>))]
    public class SyncListenerPropertyDrawer : PropertyDrawer
    {
        private Dictionary<string, string[]> _fieldPaths = new();

        private bool _initialised = false;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            var target = property.FindPropertyRelative("target");
            var fieldPath = property.FindPropertyRelative("fieldPath");
            
            if (!_initialised)
            {
                _initialised = true;
                RebuildFieldPaths(property, property.GetUnderlyingType());
            }

            Rect targetRect = new Rect(position);
            targetRect.height = EditorGUIUtility.singleLineHeight;

            Rect fieldPathRect = new Rect(position);
            fieldPathRect.yMin = targetRect.yMax + EditorGUIUtility.standardVerticalSpacing;
            fieldPathRect.height = EditorGUIUtility.singleLineHeight;
            
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(targetRect, target, new GUIContent("Target Component"));
            property.serializedObject.ApplyModifiedProperties();
            if (EditorGUI.EndChangeCheck())
            {
                RebuildFieldPaths(property, property.GetUnderlyingType());
            }
            
            if (target.objectReferenceValue == null) GUI.enabled = false;
            var fieldPaths = GetFieldPaths(property);
            int currentFieldPathIndex = Math.Max(Array.IndexOf(fieldPaths, fieldPath.stringValue), 0);
            int fieldPathIndex = EditorGUI.Popup(fieldPathRect, currentFieldPathIndex, fieldPaths);
            if (target.objectReferenceValue == null) GUI.enabled = true;
            
            EditorGUI.BeginChangeCheck();
            fieldPath.stringValue = fieldPaths[fieldPathIndex];
            property.serializedObject.ApplyModifiedProperties();
            
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 2 * EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        private string[] GetFieldPaths(SerializedProperty property)
        {
            if (_fieldPaths.ContainsKey(property.propertyPath) && _fieldPaths[property.propertyPath].Length > 0) return _fieldPaths[property.propertyPath];
            return new[] { "No Fields Found on Component" };
        }

        private void RebuildFieldPaths(SerializedProperty property, Type wrapperType)
        {
            Component component = (Component)property.FindPropertyRelative("target").objectReferenceValue;
            if (!_fieldPaths.ContainsKey(property.propertyPath))
                _fieldPaths.Add(property.propertyPath, Array.Empty<string>());
            if (component == null)
            {
                _fieldPaths[property.propertyPath] =  Array.Empty<string>();
            }
            else
            {
                var valueType = wrapperType.GetGenericArguments()[0];
                _fieldPaths[property.propertyPath] = component
                    .GetType()
                    .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(field => field.FieldType == valueType)
                    .Select(field => field.Name)
                    .ToArray();
            }
        }
    }
}
