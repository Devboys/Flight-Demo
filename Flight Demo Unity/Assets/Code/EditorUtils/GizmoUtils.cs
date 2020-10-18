using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GizmoUtils
{
    
    public static void DrawVector(float length, float cubeSize, Vector3 origin, Vector3 vector, Color color)
    {
        Color originalColor = Gizmos.color;
        Gizmos.color = color;

        Gizmos.DrawRay(origin, vector * length);
        Gizmos.DrawCube(origin + (vector * length), new Vector3(cubeSize, cubeSize, cubeSize));

        Gizmos.color = originalColor;
    }
}
