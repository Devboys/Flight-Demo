using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devboys.SharedObjects.Variables;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class LockedZoomController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FloatReference zoomRef;

    public float fovMultiplier = 2;

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
    }

    public void Update()
    {
        Vector3 targetOffset = camOffsetNormalized * zoomRef.CurrentValue;
        CMTransposer.m_FollowOffset = targetOffset;
    }
    #endregion
}