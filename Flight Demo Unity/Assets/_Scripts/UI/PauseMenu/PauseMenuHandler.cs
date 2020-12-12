using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenuHandler : MonoBehaviour
{

    [ReadOnly] public bool pauseMenuReturning;
    
    private bool inFirstFrameSinceActivated; //very jank

    public SceneReference MainMenuScene;

    private PlayerInputActions playerInput;
    private bool onBaseMenu;

    private void Awake()
    {
        playerInput = FindObjectOfType<SharedPlayerInput>().GetPlayerInput();
    }

    private void SubscribeControls()
    {
        playerInput.UI.Cancel.performed += HandleReturn;
    }

    private void UnsubscribeControls()
    {
        playerInput.UI.Cancel.performed -= HandleReturn;
    }

    private void HandleReturn(InputAction.CallbackContext context)
    {
        if (!inFirstFrameSinceActivated)
        {
            //if we're on the base menu level, mark this "pause" session as finished.
            if (onBaseMenu) ReturnToGame();
            Debug.Log("returning");
        }
    }


    public void OnEnable()
    {
        SubscribeControls();
        onBaseMenu = true;
        inFirstFrameSinceActivated = true;

        EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
    }

    void OnDisable()
    {
        UnsubscribeControls();
    }

    private void Update()
    {
        /* The start-button is bound in both action-maps. Since we switch action maps in a single frame 
        * when we pause, we sometimes end up in a situation where the action is consumed by both 
        * action-maps simoultaneously, causing the pause operation to unpause immediately. So this 
        * is a janky solution where we just wait for a single frame after entering pause menu before
        * we can exit it again */
        inFirstFrameSinceActivated = false;
    }


    #region Button Controls
    public void ReturnToGame()
    {
        pauseMenuReturning = true;
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(MainMenuScene);
    }
    #endregion
}
