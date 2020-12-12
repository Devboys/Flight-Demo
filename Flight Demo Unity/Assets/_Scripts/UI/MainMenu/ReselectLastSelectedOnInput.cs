using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// The purpose of this script is to handle the situation where the player has de-selected all elements from the UI (for example if they mouse-click on the background).
/// The event system is somehow not set up to handle this situation natively and will cause controller navigation to break if not handled properly.
/// This script simply caches the last-selected element of the UI and re-selects this element if the player performs a navigation input when no element is currently selected.
/// </summary>
public class ReselectLastSelectedOnInput : MonoBehaviour
{
    private EventSystem _eventSystem;
    private PlayerInputActions input;

    private GameObject lastSelectedObject;

    private void Awake()
    {
        //cache current event system for future use.
        _eventSystem = EventSystem.current;

        input = new PlayerInputActions();
        //reselect the last selected element on any navigation event.
        input.UI.Navigate.performed += x => ReselectLastSelected();
    }

    private void ReselectLastSelected()
    {
        if (!EventSystemHasObjectSelected())
        {
            _eventSystem.SetSelectedGameObject(lastSelectedObject);
        }
    }

    private bool EventSystemHasObjectSelected()
    {
        return _eventSystem.currentSelectedGameObject != null;
    }

    private void Update()
    {
        if (EventSystemHasObjectSelected()) lastSelectedObject = _eventSystem.currentSelectedGameObject;
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }



}
