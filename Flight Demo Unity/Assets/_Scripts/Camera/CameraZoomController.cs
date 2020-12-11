using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devboys.SharedObjects.Variables;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraZoomController : MonoBehaviour
{
    [Header("Player speed")]
    public FloatReference playerSpeed;
    public float maxSpeed = 20;
    public float minSpeed = 5;

    [Header("Zoom vars")]
    public float minZoom = 1;
    public float maxZoom = 2;
    public float zoomSpeed = 1;

    //cached components
    private CinemachineVirtualCamera _CMCamera;
    private CinemachineTransposer CMTransposer;

    private float initialRigRadius;
    private float targetRigRadius;

    #region - Unity Messages -
    private void Start()
    {
        _CMCamera = this.GetComponent<CinemachineVirtualCamera>();
        CMTransposer = _CMCamera.GetCinemachineComponent<CinemachineTransposer>();
        initialRigRadius = CMTransposer.m_FollowOffset.z;

        Vector3 offset = CMTransposer.m_FollowOffset;
        offset.z = minZoom;
        CMTransposer.m_FollowOffset = offset;
    }

    public void OnValidate()
    {
        //keep zooms to negative vals only
        minZoom = Mathf.Clamp(minZoom, -9999, 0);
        maxZoom = Mathf.Clamp(maxZoom, -9999, 0);
    }

    public void Update()
    {
        float modifier = GetNormalizedSpeed() * maxZoom;

        targetRigRadius = minZoom + modifier;
        //float delta = (targetRigRadius - CMTransposer.m_FollowOffset.z) / ;

        CMTransposer.m_FollowOffset.z = Mathf.MoveTowards(CMTransposer.m_FollowOffset.z, targetRigRadius, zoomSpeed * Time.deltaTime);

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