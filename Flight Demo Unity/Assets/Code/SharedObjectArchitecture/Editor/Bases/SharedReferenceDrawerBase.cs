using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Devboys.SharedObjects.EditorObjects
{
    public abstract class SharedReferenceDrawerBase : PropertyDrawer
    {
        string localVarName, sharedVarName, useLocalVarName;

        protected SerializedProperty _useLocalVarValue;
        protected SerializedProperty _localVarValue;
        protected SerializedProperty _sharedVariableValue;

        /// <summary>
        /// Provides the backend of this referencedrawer with the names of the variables we're dealing with.
        /// </summary>
        public void SetVariableNames(string localVariableName, string sharedVariableName, string useLocalVariableName)
        {
            localVarName = localVariableName;
            sharedVarName = sharedVariableName;
            this.useLocalVarName = useLocalVariableName;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            HandlePreGUI();
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative(localVarName), label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            HandlePreGUI();

            //get sub-properties
            _useLocalVarValue = property.FindPropertyRelative(useLocalVarName);
            _localVarValue = property.FindPropertyRelative(localVarName);
            _sharedVariableValue = property.FindPropertyRelative(sharedVarName);

            //we set the label here to ensure that Tooltip attributes still work on this property.
            label = EditorGUI.BeginProperty(position, label, property);

            //draw label of field and update position to make subsequent gui elements not overlap label.
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            //get icon and style for use in dropdown button.
            GUIStyle iconButtonStyle = GUI.skin.FindStyle("IconButton") ?? EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).FindStyle("IconButton");
            GUIContent content = new GUIContent(EditorGUIUtility.IconContent("_Menu")); //built-in icon.

            int dropdownButtonSize = 20;

            //define dropdown button bounding box
            Rect buttonPos = new Rect(position.position, new Vector2(dropdownButtonSize, dropdownButtonSize));

            //Draws and handles the dropdown for choosing between local/shared variable
            if (EditorGUI.DropdownButton(buttonPos, content, FocusType.Passive, iconButtonStyle))
            {
                GenericMenu menu = new GenericMenu();

                menu.AddItem(new GUIContent("Use Local Variable"), (_useLocalVarValue.boolValue ? true : false), () =>
                {
                    _useLocalVarValue.boolValue = true;
                    _useLocalVarValue.serializedObject.ApplyModifiedProperties(); //must use this to apply change to bool
            });
                menu.AddItem(new GUIContent("Use Shared Variable"), (_useLocalVarValue.boolValue ? false : true), () =>
                {
                    _useLocalVarValue.boolValue = false;
                    _useLocalVarValue.serializedObject.ApplyModifiedProperties(); //must use this to apply change to bool
            });

                menu.ShowAsContext(); //shows the menu we just created.
            }

            position.x += dropdownButtonSize; //increment position origin so we don't cover the dropdown button with the next gui field.

            position.width -= dropdownButtonSize;

            //Draw the relevant(activated) variable in the inspector.
            if (_useLocalVarValue.boolValue == true)
            {
                DrawLocalVariable(position, property, label);
            }
            else
            {
                DrawSharedVariable(position, property, label);
            }

            EditorGUI.EndProperty();
        }

        protected abstract void DrawLocalVariable(Rect position, SerializedProperty property, GUIContent label);

        protected abstract void DrawSharedVariable(Rect position, SerializedProperty property, GUIContent label);

        protected abstract void HandlePreGUI();
    }
}
