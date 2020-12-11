using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LabelAttribute))]
public class LabelDrawer : DecoratorDrawer
{
    private static readonly float widthSpacing = 10;
    private static readonly GUIStyle labelStyle = EditorStyles.wordWrappedLabel;
    LabelAttribute Target
    {
        get { return ((LabelAttribute)attribute);  }
    }

    public override float GetHeight()
    {
        labelStyle.fontStyle = FontStyle.Italic;
        GUIContent content = new GUIContent(Target.text);
        labelStyle.CalcMinMaxWidth(content, out float minWidth, out float maxWidth);
        int numLines = Mathf.CeilToInt((minWidth - widthSpacing * 2) / Screen.width);
        numLines += 1;

        return numLines * EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect position)
    {
        position.x += widthSpacing;
        position.width -= widthSpacing * 2;
        EditorGUI.TextArea(position, Target.text, labelStyle);
    }
}
