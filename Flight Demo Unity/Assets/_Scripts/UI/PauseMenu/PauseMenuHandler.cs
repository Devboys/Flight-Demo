using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenuHandler : MonoBehaviour
{

    [ReadOnly] public bool pauseMenuReturning;
    
    private bool inFirstFrameSinceActivated; //very jank

    public SceneReference MainMenuScene;
    public GameObject firstSelectedObject;

    private PlayerInputActions playerInput;
    private bool onBaseMenu;

    private EventSystem _eventSystem;

    private void Awake()
    {
        playerInput = FindObjectOfType<SharedPlayerInput>().GetPlayerInput();
        _eventSystem = EventSystem.current;
        
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
        }
    }


    public void OnEnable()
    {
        SubscribeControls();

        onBaseMenu = true;
        inFirstFrameSinceActivated = true;

        //select first gameobject from eventsystem and manually notify it that it was selected. Otherwise transition will not play.
        GameObject selectedObject = _eventSystem.firstSelectedGameObject;
        _eventSystem.SetSelectedGameObject(selectedObject);
        selectedObject.GetComponent<Selectable>().OnSelect(null);
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
