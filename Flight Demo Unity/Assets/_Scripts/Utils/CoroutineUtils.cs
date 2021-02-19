using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoroutineUtils
{
    /// <summary>
    /// Waits for a number of seconds and then invokes a callback function.
    /// </summary>
    public static IEnumerator WaitThenCallBack(float waitTime, Action callback)
    {
        yield return new WaitForSeconds(waitTime);
        callback.Invoke();
    }

    public static IEnumerator InvokeNextFrame(Action callback)
    {
        yield return null; //skip this frame
        callback.Invoke();
    }

}