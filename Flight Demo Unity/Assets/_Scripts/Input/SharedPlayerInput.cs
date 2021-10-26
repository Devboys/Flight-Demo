using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SharedPlayerInput : MonoBehaviour
{
    //single class to handle shared inputactions

    private PlayerInputActions playerInput;

    public void Awake()
    {
        if (FindObjectsOfType<SharedPlayerInput>().Length > 1) Debug.LogError("Multiple Shared Input Handlers in Scene!");
    }

    private void CreateInputObject()
    {
        playerInput = new PlayerInputActions();

        playerInput.UI.Disable();
        playerInput.Player.Enable();
    }

    public void OnEnable()
    {
        GetPlayerInput().Enable();
    }

    public void OnDisable()
    {
        GetPlayerInput().Disable();
    }

    public PlayerInputActions GetPlayerInput()
    {
        if (playerInput == null)
        {
            CreateInputObject();
        }
        return playerInput;
    }

    /// <summary>
    /// Returns this input object from the current scene (if it exists)
    /// </summary>
    public static SharedPlayerInput GetSceneInstance()
    {
        return FindObjectOfType<SharedPlayerInput>();
    }
}
