using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Camera refs")]
    [SerializeField] private CinemachineVirtualCamera lockedCamera = null; //locked behind player.
    [SerializeField] private CinemachineFreeLook freeLookCamera = null; //used to orbit the player.

    [Header("Switch settings")]
    [Tooltip("The amount of seconds to wait before resetting free-look camera back to locked camera. Reset when player moves the camera.")]
    [SerializeField] private float resetWaitTime = 10;

    // ---- Private vars ----
    private PlayerInputActions inputActions;
    //Stores the current transition coroutine. Used to indicate wether a transition is in progress and can be used to cancel it.
    private Coroutine transitionCoroutine;

    private float resetCamTimer;

    // ---- Cached Components ----
    private FreelookZoomController freeZoomController;

    // ---- Shorthands ----
    private bool InFreeLook => freeLookCamera.Priority == 1;
    private bool InLocked => lockedCamera.Priority == 1;

    private void Start()
    {
        lockedCamera.Priority = 1;
        freeLookCamera.Priority = 0;

        resetCamTimer = 0;

        inputActions = FindObjectOfType<SharedPlayerInput>().GetPlayerInput();
        inputActions.Player.Look.performed += Look_performed;
        inputActions.Player.ResetCamera.performed += ResetCamera_performed;

        transitionCoroutine = null;

        freeZoomController = freeLookCamera.GetComponent<FreelookZoomController>();
    }

    private void Update()
    {
        if(resetCamTimer > 0)
        {
            resetCamTimer -= Time.deltaTime;
            if (resetCamTimer <= 0) FreeToLockedCamera();
        }
    }

    private void ResetCamera_performed(InputAction.CallbackContext obj)
    {
        if (obj.ReadValueAsButton())
        {
            FreeToLockedCamera();
        }
    }

    private void Look_performed(InputAction.CallbackContext obj)
    {
        if (obj.ReadValue<Vector2>().sqrMagnitude > 0.01f)
        {
            LockedToFreeCamera();
        }
    }

    /// <summary>
    /// Switches from orbital camera view to locked camera view.
    /// </summary>
    private void FreeToLockedCamera()
    {
        if (InLocked || transitionCoroutine != null) return;

        lockedCamera.Priority = 1;
        freeLookCamera.Priority = 0;

        //We have to delay resetting the freelook camera for a bit because we need the transitions to play out properly in cinemachine.
        transitionCoroutine = StartCoroutine(CoroutineUtils.WaitThenCallBack(0.3f, () => 
            {   
                freeLookCamera.m_XAxis.Value = 0f;
                freeLookCamera.m_YAxis.Value = 0.5f;

                transitionCoroutine = null;
            }));

        freeZoomController.UnlockZoom();
    }

    /// <summary>
    /// Switches from locked camera view to orbital camera view.
    /// </summary>
    private void LockedToFreeCamera()
    {
        resetCamTimer = resetWaitTime;
        if (InFreeLook || transitionCoroutine != null) return;

        freeLookCamera.Priority = 1;
        lockedCamera.Priority = 0;

        freeZoomController.LockZoom();
    }
}
