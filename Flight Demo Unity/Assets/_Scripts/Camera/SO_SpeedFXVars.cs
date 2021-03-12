using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedFXSettings", menuName = "ProprietarySO/SpeedFXSettings")]
public class SO_SpeedFXVars : ScriptableObject
{
    public float smoothSpeed = 0.2f;
    public float minZoom;
    public float maxZoom;

    public float minFOV;
    public float maxFOV;

    public float minSpeed;
    public float maxSpeed;


    public void OnValidate()
    {
        //keep zoom settings to positive vals only
        minZoom = Mathf.Clamp(minZoom, 0, 9999);
        maxZoom = Mathf.Clamp(maxZoom, 0, 9999);
    }

    public float ZoomToFOV(float ZoomVal)
    {
        float ratio = (maxFOV - minFOV) / (maxZoom - minZoom);
        float rawZoom = ZoomVal - minZoom;
        float FOVval = minFOV + rawZoom * ratio;

        return FOVval;
    }
}