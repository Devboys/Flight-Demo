using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundSelf : MonoBehaviour
{
    [Header("Axes (deg/s)")]
    public float xSpeed = 100;
    public float ySpeed = 100;
    public float zSpeed = 100;

    private Transform _transform;
    private void Start()
    {
        _transform = this.transform;
    }
    private void Update()
    {
        Quaternion rot = _transform.rotation;
        rot *= Quaternion.Euler(new Vector3(xSpeed, ySpeed, zSpeed) * Time.deltaTime);
        _transform.rotation = rot;
    }
    
}