using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Orbitally rotates the camera about the target to be placed directly behind the targets forward vector.
/// Rotation is damped based on angle difference and MaxAngleDiff variables
/// Intended to be used alongisde a CinemachineVirtualCamera with Body="Do nothing" and Aim="Hard Look At".
/// </summary>
[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CM_OrbitalLockedCamera : MonoBehaviour
{
    //
    [SerializeField] private float maxHoriAngleDiff = 45;
    [SerializeField] private float maxVertAngleDiff = 30;
    [SerializeField] private float maxHoriSpeed = 50; //in deg/s
    [SerializeField] private float minHoriSpeed = 10;
    [SerializeField] private float maxVertSpeed = 50; //in deg/s
    [SerializeField] private float minVertSpeed = 5;
    [SerializeField] private float maxYAngle = 40; //in deg
    [SerializeField] private float minYAngle = 40; //in deg

    [Header("Read Only")]
    [ReadOnly][SerializeField] private float trackingRadius = 5;
    //cached components
    private CinemachineVirtualCamera _CMVirtual;

    private Transform cameraPuppet;
    private Transform targetTransform;

    private void Awake()
    {
        GameObject go = new GameObject();
        go.name = "CameraPuppet";
        cameraPuppet = go.transform;
    }

    private void Start()
    {
        _CMVirtual = GetComponent<CinemachineVirtualCamera>();

        targetTransform = _CMVirtual.m_Follow;

        Vector3 targetDir = Vector3.ProjectOnPlane(targetTransform.forward, Vector3.up).normalized;
        Vector3 targetPos = _CMVirtual.m_Follow.position - targetDir * trackingRadius;
        cameraPuppet.SetPositionAndRotation(targetPos, Quaternion.identity);
        cameraPuppet.LookAt(targetTransform.position);
    }

    private void Update()
    {
        //Get base vectors
        Vector3 currentDir = cameraPuppet.forward;
        Vector3 targetDir = targetTransform.forward;
        Vector3 vertAxis = Vector3.Cross(targetDir, Vector3.up).normalized;

        Quaternion dampedRot = Quaternion.identity;

        //Rotate to match horizontal component based on angle diff
        float horiAngleDiff = VectorUtils.SignedAngleAboutAxis(currentDir, targetDir, Vector3.up); //angle diff about up axis
        float horiSpeed = Mathf.Lerp(0, maxHoriSpeed, Mathf.Abs(horiAngleDiff) / maxHoriAngleDiff) + minHoriSpeed; //rotation damping
        float horiDelta = horiSpeed * Mathf.Sign(horiAngleDiff) * Time.deltaTime;
        horiDelta = (Mathf.Abs(horiDelta) > Mathf.Abs(horiAngleDiff)) ? horiAngleDiff : horiDelta; //prevent overshoot
        dampedRot *= Quaternion.AngleAxis(horiDelta, Vector3.up); //add horizontal to composed rotation

        //Rotate to match vertical component based on angle diff
        float vertAngleDiff = VectorUtils.SignedAngleAboutAxis(currentDir, targetDir, vertAxis); //angle difference about vert axis
        float vertSpeed = Mathf.Lerp(0, maxVertSpeed, Mathf.Abs(vertAngleDiff) / maxVertAngleDiff) + minVertSpeed; //rotation damping
        float vertDelta = vertSpeed * Mathf.Sign(vertAngleDiff) * Time.deltaTime;
        vertDelta = (Mathf.Abs(vertDelta) > Mathf.Abs(vertAngleDiff)) ? vertAngleDiff : vertDelta; //prevent overshoot
        dampedRot *= Quaternion.AngleAxis(vertDelta, vertAxis); //add vertical to composed rotation
        
        //apply rotation and move puppet
        currentDir = dampedRot * currentDir;
        Vector3 targetPos = targetTransform.position - currentDir * trackingRadius;
        cameraPuppet.SetPositionAndRotation(targetPos, Quaternion.identity);
        cameraPuppet.LookAt(targetTransform.position, Vector3.up);

        //finally, move camera to puppet
        transform.SetPositionAndRotation(cameraPuppet.position, Quaternion.identity);
    }

    public void SetTrackingRadius(float val)
    {
        trackingRadius = val;
    }
}