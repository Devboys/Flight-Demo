using UnityEditor;
using UnityEngine;
using Devboys.SharedObjects.Variables;

namespace Devboys.SharedObjects.EditorObjects
{
    [CustomPropertyDrawer(typeof(TransformReference))]
    public class TransformReferenceDrawer : SharedReferenceDrawerBase
    {
        protected override void HandlePreGUI()
        {
            SetVariableNames("LocalVariable", "SharedVariable", "UseLocalVariable");
        }

        protected override void DrawLocalVariable(Rect position, SerializedProperty property, GUIContent label)
        {
            _localVarValue.objectReferenceValue = EditorGUI.ObjectField(position, _localVarValue.objectReferenceValue, typeof(Transform), true);
        }

        protected override void DrawSharedVariable(Rect position, SerializedProperty property, GUIContent label)
        {
            _sharedVariableValue.objectReferenceValue = EditorGUI.ObjectField(position, _sharedVariableValue.objectReferenceValue, typeof(TransformVariable), false);
        }
    }
}
