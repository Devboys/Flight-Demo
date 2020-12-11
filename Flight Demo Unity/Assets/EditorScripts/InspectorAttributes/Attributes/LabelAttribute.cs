using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Decorator drawer to add notes in the unity inspector.
/// </summary>
public class LabelAttribute : PropertyAttribute
{
    public string text;

    public LabelAttribute(string text)
    {
        this.text = text;
    }
}
