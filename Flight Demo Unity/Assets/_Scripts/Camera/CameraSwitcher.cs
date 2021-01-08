using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Camera refs")]
    public CinemachineVirtualCamera lockedCamera = null; //locked behind player.
    public CinemachineFreeLook freeLookCamera = null; //used to orbit the player.

    // ---- Private vars ----
    private PlayerInputActions inputActions;
    //Stores the current transition coroutine. Used to indicate wether a transition is in progress and can be used to cancel it.
    private Coroutine transitionCoroutine;

    private FreelookZoomController freeZoomController;

    // ---- Shorthands ----
    private bool InFreeLook => freeLookCamera.Priority == 1;
    private bool InLocked => lockedCamera.Priority == 1;

    private void Start()
    {
        lockedCamera.Priority = 1;
        freeLookCamera.Priority = 0;

        inputActions = FindObjectOfType<SharedPlayerInput>().GetPlayerInput();
        inputActions.Player.Look.performed += Look_performed;
        inputActions.Player.ResetCamera.performed += ResetCamera_performed;

        transitionCoroutine = null;

        freeZoomController = freeLookCamera.GetComponent<FreelookZoomController>();
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
    /// Switches from orbital camera view to locked camera view. Transition manually handled to ensure that camera "rotates" around the player rather than zooms overhead.
    /// </summary>
    private void FreeToLockedCamera()
    {
        if (InLocked || transitionCoroutine != null) return;

        //transition the locked camera manually and then switch to locked camera.
        //transitionCoroutine = StartCoroutine(RecenterFreelook(1, () =>
        //    {
        //        lockedCamera.Priority = 1;
        //        freeLookCamera.Priority = 0;

        //        transitionCoroutine = null;
        //    }
        //    ));

        lockedCamera.Priority = 1;
        freeLookCamera.Priority = 0;

        transitionCoroutine = StartCoroutine(CoroutineUtils.WaitThenCallBack(0.3f, () => 
            {   
                freeLookCamera.m_XAxis.Value = 0f;
                freeLookCamera.m_YAxis.Value = 0.5f;

                transitionCoroutine = null;
            }));

        freeZoomController.UnlockZoom();
    }

    /// <summary>
    /// Switches from locked camera view to orbital camera view. Transition automatically handled by cinemachine.
    /// </summary>
    private void LockedToFreeCamera()
    {
        //if(transitionCoroutine != null) CancelRecenter();
        if (InFreeLook || transitionCoroutine != null) return;

        freeLookCamera.Priority = 1;
        lockedCamera.Priority = 0;

        freeZoomController.LockZoom();
    }

    private IEnumerator RecenterFreelook(float speed, Action endOfTransitionCallback)
    {
        float xValue = freeLookCamera.m_XAxis.Value;
        float yValue = freeLookCamera.m_YAxis.Value;

        while((Mathf.Abs(xValue) > 0.1f) && (Mathf.Abs(yValue) > 0.01f))
        {
            xValue = Mathf.MoveTowards(xValue, 0, speed * 180 * Time.deltaTime);
            yValue = Mathf.MoveTowards(yValue, 0.5f, speed * Time.deltaTime);
            freeLookCamera.m_XAxis.Value = xValue;
            freeLookCamera.m_YAxis.Value = yValue;
            yield return null;
        }
        endOfTransitionCallback.Invoke();
    }
    private void CancelRecenter()
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
            transitionCoroutine = null;
        }
    }
}
