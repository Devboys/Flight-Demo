using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devboys.SharedObjects.Variables;
using Cinemachine;

public class CameraZoomController : MonoBehaviour
{
    [Header("Player speed")]
    public FloatReference playerSpeed;
    public float maxSpeed = 20;
    public float minSpeed = 5;

    [Header("Zoom vars")]
    public float zoomMod = 1;
    public float zoomSpeed = 1;

    //cached components
    private CinemachineFreeLook _CMCamera;

    private float[] initialRigRadii = new float[3];
    private float[] targetRigRadii = new float[3];

    #region - Unity Messages -
    private void Start()
    {
        _CMCamera = GetComponent<CinemachineFreeLook>();
        for(int i = 0; i < 2; i++)
        {
            initialRigRadii[i] = _CMCamera.m_Orbits[i].m_Radius;
        }
    }

    public void Update()
    {
        float modifier = GetNormalizedSpeed() * zoomMod;

        //update camera zoom based on player speed;
        for (int i = 0; i < 2; i++)
        {
            targetRigRadii[i] = initialRigRadii[i] + modifier;
            _CMCamera.m_Orbits[i].m_Radius = Mathf.MoveTowards(_CMCamera.m_Orbits[i].m_Radius, targetRigRadii[i], zoomSpeed * Time.deltaTime);
        }

    }
    #endregion

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