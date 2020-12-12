using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuActivator : MonoBehaviour
{
    public PauseMenuHandler pauseMenuHandler;

    public PlayerInputActions playerInput;

    public void Awake()
    {
        playerInput = FindObjectOfType<SharedPlayerInput>().GetPlayerInput();

        if (pauseMenuHandler.gameObject.activeSelf) pauseMenuHandler.gameObject.SetActive(false);
    }

    public void SubscribeControls()
    {
        playerInput.Player.Pause.performed += PauseGame;
    }

    public void UnsubscribeControls()
    {
        playerInput.Player.Pause.performed -= PauseGame;
    }

    private void OnEnable()
    {
        SubscribeControls();
    }

    private void OnDisable()
    {
        UnsubscribeControls();

        //if we exit the game from the pause menu, we must remember to reset the timescale.
        Time.timeScale = 1;
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        pauseMenuHandler.gameObject.SetActive(true);
        pauseMenuHandler.pauseMenuReturning = false;

        playerInput.Player.Disable();
        playerInput.UI.Enable();

        Time.timeScale = 0;

        StartCoroutine(WaitForResume());
    }

    private IEnumerator WaitForResume()
    {
        while (!pauseMenuHandler.pauseMenuReturning)
        {
            yield return null;
        }

        ResumeGame();
    }

    private void ResumeGame()
    {
        pauseMenuHandler.gameObject.SetActive(false);
        
        Time.timeScale = 1;

        playerInput.UI.Disable();
        playerInput.Player.Enable();
    }
}
