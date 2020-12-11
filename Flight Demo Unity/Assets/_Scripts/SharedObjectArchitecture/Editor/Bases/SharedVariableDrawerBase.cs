using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Devboys.SharedObjects.Variables;

namespace Devboys.SharedObjects.EditorObjects {
    public abstract class SharedVariableDrawerBase<T1, T2> : PropertyDrawer where T2: SharedNumericVariableBase<T1>
    {
        private static readonly float ValuePreviewWidth = 50;
        private static readonly float ValuePreviewSpacing = 2;
        private static readonly float EdgeSpacing = 5; //seems to be unity standard inspector edge spacing.

        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return false;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
            
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            label = EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);


            //draw value preview of variable reference 
            if (property.objectReferenceValue != null)
            {
                position.width -= ValuePreviewWidth + ValuePreviewSpacing;

                Rect valuePos = position;
                valuePos.x = Screen.width - ValuePreviewWidth - EdgeSpacing;
                valuePos.width = ValuePreviewWidth;

                SharedNumericVariableBase<T1> targetVariable = (SharedNumericVariableBase<T1>)property.objectReferenceValue;
                GUI.enabled = false;
                DrawPreviewField(valuePos, targetVariable.CurrentValue);
                GUI.enabled = true;
            }


            property.objectReferenceValue = EditorGUI.ObjectField(position, property.objectReferenceValue, typeof(T2), false);

            EditorGUI.EndProperty();
        }

        public abstract void DrawPreviewField(Rect position, T1 currentVariableValue);

    }
}
