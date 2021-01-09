using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Intended for use in OnValidate methods
public static class ValidateUtils
{
    public static void EnsurePositiveOrZero(ref float val)
    {
        val = Mathf.Clamp(val, 0, 9999);
    }

    public static void EnsurePositiveOrZero(ref int val)
    {
        val = Mathf.Clamp(val, 0, 9999);
    }

    public static void EnsureNegativeOrZero(ref float val)
    {
        val = Mathf.Clamp(val, -9999, 0);
    }

    public static void EnsureNegativeOrZero(ref int val)
    {
        val = Mathf.Clamp(val, -9999, 0);
    }

}