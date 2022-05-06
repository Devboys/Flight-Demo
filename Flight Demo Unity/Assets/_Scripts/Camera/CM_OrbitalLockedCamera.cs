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
    [Header("Target Settings")]
    [SerializeField] private Vector3 offset;

    [Header("Horizontal Orbit")]
    [SerializeField] private float maxHoriAngleDiff = 45;
    [SerializeField] private float maxHoriSpeed = 50; //in deg/s
    [SerializeField] private float minHoriSpeed = 10; //in deg/s

    [Header("Vertical Orbit")]
    [SerializeField] private float maxVertAngleDiff = 30;
    [SerializeField] private float maxVertSpeed = 50; //in deg/s
    [SerializeField] private float minVertSpeed = 5; //in deg/s
    [SerializeField] private float maxVertAngle = 30; //in deg
    [SerializeField] private float minVertAngle = -30; //in deg

    [Header("Read Only")]
    [ReadOnly][SerializeField] private float trackingRadius = 5;
    //cached components
    private CinemachineVirtualCamera _CMVirtual;

    //We move the camera through an intermediate GO, cameraPuppet. 
    //cameraPuppet is used because its forward vector can be manipulated directly (unlike CM, which relies on "look at" algorithm)
    private Transform cameraPuppet;
    private Transform targetTransform;

    private void OnValidate()
    {
        minVertAngle = Mathf.Min(minVertAngle, 0);
        maxVertAngle = Mathf.Max(maxVertAngle, 0);
    }

    private void Awake()
    {
        GameObject go = new GameObject("CameraPuppet");
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

        //enforce vertical angle extrema
        float currVertAngle = Vector3.SignedAngle(Vector3.ProjectOnPlane(currentDir, Vector3.up).normalized, currentDir, vertAxis);
        vertDelta = Mathf.Min(vertDelta, maxVertAngle - currVertAngle);
        vertDelta = Mathf.Max(vertDelta, minVertAngle - currVertAngle);

        dampedRot *= Quaternion.AngleAxis(vertDelta, vertAxis); //add vertical to composed rotation
        
        //Apply rotation and move puppet
        currentDir = dampedRot * currentDir;
        Vector3 targetPos = targetTransform.position - currentDir * trackingRadius;
        cameraPuppet.SetPositionAndRotation(targetPos, Quaternion.identity);
        cameraPuppet.LookAt(targetTransform.position, Vector3.up);

        //Finally, move camera to puppet
        transform.SetPositionAndRotation(cameraPuppet.position, Quaternion.identity);
    }

    public void SetTrackingRadius(float val)
    {
        trackingRadius = val;
    }
}