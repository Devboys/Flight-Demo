using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTestInput : MonoBehaviour
{

    private PlayerInputActions inputObject;

    public float slowedTimescale = 0.2f;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        inputObject = this.GetComponent<SharedPlayerInput>().GetPlayerInput();
        //Subscribe test input
        inputObject.Player.TestButton.performed += Input_ToggleSlowTime;
    }

    private void OnDisable()
    {
        //unsubscribe test input
        inputObject.Player.TestButton.performed -= Input_ToggleSlowTime;
    }

    private void Input_ToggleSlowTime(InputAction.CallbackContext ctx)
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = slowedTimescale;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}