using Cinemachine;
using Devboys.SharedObjects.Variables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalCameraSpeedEffect : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FloatReference zoomRef;
    [SerializeField] private SO_SpeedFXVars settingVars;

    //cached components
    private CinemachineVirtualCamera _CMCamera;
    private CM_OrbitalLockedCamera _CMOrbital;

    private Vector3 camOffsetNormalized;

    #region - Unity Messages -
    private void Start()
    {
        _CMCamera = GetComponent<CinemachineVirtualCamera>();
        _CMOrbital = GetComponent<CM_OrbitalLockedCamera>();
    }

    public void Update()
    {
        _CMOrbital.SetTrackingRadius(zoomRef.CurrentValue);
        _CMCamera.m_Lens.FieldOfView = settingVars.ZoomToFOV(zoomRef.CurrentValue);
    }
    #endregion
}