using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devboys.SharedObjects.Variables;
using Cinemachine;

/// <summary>
/// Zoom controller for use with freelook cameras.
/// </summary>
[RequireComponent(typeof(CinemachineFreeLook))]
public class FreelookZoomController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FloatReference lockedCamZoom;

    [Header("Base values")]
    [SerializeField] private float lockedZoomRadius = 10;
    [SerializeField] private float lowerUpperOffset = -3;
    [Tooltip("When transitioning from locked to freelook camera, this value controls how fast the zoom returns to the locked zoom radius")]
    [SerializeField] private float zoomSpeed = 2;

    //cached components
    private CinemachineFreeLook _CMCamera;

    private float[] rigOffsets;
    private float smoothedRadius; //used to smoothly transition between zoom-levels.

    private bool zoomLocked;
    /// <summary>
    /// value only used in inspector. Dont use this for anything else.
    /// </summary>
    [ReadOnly] public float currentZoom = 0;

    #region - Unity Messages -
    private void Start()
    {
        rigOffsets = new float[3];

        zoomLocked = false;
        _CMCamera = GetComponent<CinemachineFreeLook>();

        for(int j = 0; j <= 2; j++)
        {
            rigOffsets[j] = (j == 1) ? 0 : lowerUpperOffset;
        }
    }

    public void Update()
    {
        float targetRadius;

        if (zoomLocked)
        {
            //transition toward minZoom;
            smoothedRadius = Mathf.MoveTowards(smoothedRadius, lockedZoomRadius, zoomSpeed * Time.deltaTime);
            targetRadius = smoothedRadius;
        }
        else
        {
            //follow locked cam zoom
            targetRadius = lockedCamZoom.CurrentValue;
        }

        currentZoom = targetRadius;

        SetRigRadii(targetRadius);
    }
    #endregion

    /// <summary>
    /// Sets every rig radii in the targeted freelook camera to the given value + their offset defined in rigOffsets.
    /// </summary>
    private void SetRigRadii(float centerRadius)
    {
        //update camera zoom based on player speed;
        for (int i = 0; i <= 2; i++)
        {
            _CMCamera.m_Orbits[i].m_Radius = centerRadius + rigOffsets[i];
        }
    }

    /// <summary>
    /// Locks the zoom of this controller to 
    /// </summary>
    public void LockZoom()
    {
        smoothedRadius = lockedCamZoom.CurrentValue;
        zoomLocked = true;
    }

    public void UnlockZoom()
    {
        //we have to wait a bit before re-synchronizing camera because cinemachine needs time to transition based on this camera's current position.
        StartCoroutine(CoroutineUtils.WaitThenCallBack(0.3f, () => { zoomLocked = false; }));
    }
}