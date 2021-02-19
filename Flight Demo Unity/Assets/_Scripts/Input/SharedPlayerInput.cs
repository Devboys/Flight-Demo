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
        playerInput = new PlayerInputActions();

        playerInput.UI.Disable();
        playerInput.Player.Enable();

        if (FindObjectsOfType<SharedPlayerInput>().Length > 1) Debug.LogError("Multiple Shared Input Handlers in Scene!");
    }

    public void OnEnable()
    {
        playerInput.Enable();
    }

    public void OnDisable()
    {
        playerInput.Disable();
    }

    public PlayerInputActions GetPlayerInput()
    {
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
