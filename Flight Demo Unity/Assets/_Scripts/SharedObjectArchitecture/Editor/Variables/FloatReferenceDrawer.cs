using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Devboys.SharedObjects.Variables;

namespace Devboys.SharedObjects.EditorObjects
{
    [CustomPropertyDrawer(typeof(FloatReference))]
    public class FloatReferenceDrawer : SharedReferenceDrawerBase
    {
        protected override void HandlePreGUI()
        {
            SetVariableNames("LocalVariable", "SharedVariable", "UseLocalVariable");
        }

        protected override void DrawLocalVariable(Rect position, SerializedProperty property, GUIContent label)
        {
            _localVarValue.floatValue = EditorGUI.FloatField(position, _localVarValue.floatValue);
        }

        protected override void DrawSharedVariable(Rect position, SerializedProperty property, GUIContent label)
        {
            _sharedVariableValue.objectReferenceValue = EditorGUI.ObjectField(position, _sharedVariableValue.objectReferenceValue, typeof(FloatVariable), false);
        }
    }
}