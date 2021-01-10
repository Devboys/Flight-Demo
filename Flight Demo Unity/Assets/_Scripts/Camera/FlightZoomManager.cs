using Devboys.SharedObjects.Variables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightZoomManager : MonoBehaviour
{

    [Header("Input refs")]
    public FloatReference playerSpeed = null;

    [Header("Output refs")]
    public FloatVariable currentZoom = null;
    public FloatVariable smoothedZoom = null;

    [Header("Settings")]
    public float minZoom;
    public float maxZoom;
    public float zoomSpeed;

    public float minSpeed;
    public float maxSpeed;


    public void OnValidate()
    {
        //keep zooms to positive vals only
        minZoom = Mathf.Clamp(minZoom, 0, 9999);
        maxZoom = Mathf.Clamp(maxZoom, 0, 9999);
    }

    public void Update()
    {
        float modifier = GetNormalizedSpeed() * (maxZoom - minZoom);
        float targetRadius = minZoom + modifier;

        currentZoom.CurrentValue = targetRadius;
        smoothedZoom.CurrentValue = Mathf.MoveTowards(smoothedZoom.CurrentValue, currentZoom.CurrentValue, zoomSpeed * Time.deltaTime);
    }

    private float GetNormalizedSpeed()
    {
        float speed = playerSpeed.CurrentValue;

        if (speed < minSpeed) return 0;
        else if (speed > maxSpeed) return 1;
        else
        {
            return (speed - minSpeed) / (maxSpeed - minSpeed);
        }

    }
}