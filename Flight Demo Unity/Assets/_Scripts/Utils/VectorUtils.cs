using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtils
{

    /// <summary>
    /// Returns the signed angle between the "from" and "to" vectors relative to the given axis. All input vectors are expected to be normalized.
    /// </summary>
    /// <param name="from">The first vector.</param>
    /// <param name="to">The second vector.</param>
    /// <param name="axis">The axis that the angle will be relative to.</param>
    /// <param name="clockwise">Whether the output angle will be measured in clockwise or counterclockwise.</param>
    public static float SignedAngleAboutAxis(Vector3 from, Vector3 to, Vector3 axis, bool clockwise = false)
    {
        Vector3 right;
        if (clockwise)
        {
            right = Vector3.Cross(from, axis);
            from = Vector3.Cross(axis, right);
        }
        else
        {
            right = Vector3.Cross(axis, from);
            from = Vector3.Cross(right, axis);
        }
        return Mathf.Atan2(Vector3.Dot(to, right), Vector3.Dot(to, from)) * Mathf.Rad2Deg;
    }

    public static void DebugDrawVectorAtPosition(Vector3 position, Vector3 dir, Color col, float length = 5)
    {
        Debug.DrawLine(position, position + dir * length, col);
    }
}