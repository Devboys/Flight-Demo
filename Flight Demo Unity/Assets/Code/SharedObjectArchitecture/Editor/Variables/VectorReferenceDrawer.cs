using UnityEditor;
using UnityEngine;
using Devboys.SharedObjects.Variables;

namespace Devboys.SharedObjects.EditorObjects
{
    [CustomPropertyDrawer(typeof(VectorReference))]
    public class VectorReferenceDrawer : SharedReferenceDrawerBase
    {
        protected override void HandlePreGUI()
        {
            SetVariableNames("LocalVariable", "SharedVariable", "UseLocalVariable");
        }

        protected override void DrawLocalVariable(Rect position, SerializedProperty property, GUIContent label)
        {
            _localVarValue.vector3Value = EditorGUI.Vector3Field(position, GUIContent.none, _localVarValue.vector3Value);/*EditorGUI.Vector3Field(position, _localVarValue.intValue);*/
        }

        protected override void DrawSharedVariable(Rect position, SerializedProperty property, GUIContent label)
        {
            _sharedVariableValue.objectReferenceValue = EditorGUI.ObjectField(position, _sharedVariableValue.objectReferenceValue, typeof(VectorVariable), false);
        }
    }
}
