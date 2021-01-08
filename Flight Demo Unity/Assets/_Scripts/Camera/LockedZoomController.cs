﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devboys.SharedObjects.Variables;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class LockedZoomController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FloatVariable currentZoom;
    [SerializeField] private FloatReference playerSpeed;
    
    [Header("Speed normalization")]
    [SerializeField] private float maxSpeed = 20;
    [SerializeField] private float minSpeed = 5;

    [Header("Zoom vars")]
    [SerializeField] private float minZoom = 1;
    [SerializeField] private float maxZoom = 2;
    [SerializeField] private float zoomSpeed = 1;

    //cached components
    private CinemachineVirtualCamera _CMCamera;
    private CinemachineTransposer CMTransposer;

    private Vector3 camOffsetNormalized;

    #region - Unity Messages -
    private void Start()
    {
        _CMCamera = this.GetComponent<CinemachineVirtualCamera>();
        CMTransposer = _CMCamera.GetCinemachineComponent<CinemachineTransposer>();

        camOffsetNormalized = CMTransposer.m_FollowOffset.normalized;

        CMTransposer.m_FollowOffset = camOffsetNormalized * minZoom;
    }

    public void OnValidate()
    {
        //keep zooms to negative vals only
        minZoom = Mathf.Clamp(minZoom, 0, 9999);
        maxZoom = Mathf.Clamp(maxZoom, 0, 9999);
    }

    public void Update()
    {
        float modifier = GetNormalizedSpeed() * (maxZoom - minZoom);

        float targetRadius = minZoom + modifier;
        Vector3 targetOffset = camOffsetNormalized * targetRadius;
        CMTransposer.m_FollowOffset = Vector3.MoveTowards(CMTransposer.m_FollowOffset, targetOffset, zoomSpeed * Time.deltaTime);

        currentZoom.CurrentValue = CMTransposer.m_FollowOffset.magnitude;
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