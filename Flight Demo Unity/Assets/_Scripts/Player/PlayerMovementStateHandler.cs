using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerMovementStateHandler : MonoBehaviour
{
    private List<PlayerMovementStateBase> movementStates;
    private PlayerMovementStateBase activeState;

    [Header("Gizmos")]
    public bool drawGizmos = true;

    public void Awake()
    {
        movementStates = new List<PlayerMovementStateBase>();
        movementStates.AddRange(GetComponents<PlayerMovementStateBase>());

        foreach (PlayerMovementStateBase state in movementStates)
        {
            state.parentHandler = this;
        }
    }

    public void Start()
    {
        SwitchToState<PlayerFlightController>();
    }

    /// <summary>
    /// switches to the given movement state and returns it.
    /// </summary>
    public T SwitchToState<T>() where T : PlayerMovementStateBase
    {
        foreach(PlayerMovementStateBase state in movementStates)
        {
            if(state is T && !state.active)
            {
                state.active = true;
                activeState = state;
                state.HandleStateActivate();
            }
            else if (state.active)
            {
                state.HandleStateDeactivate();
                state.active = false;
            }
        }

        return activeState as T;
    }

    public void Update()
    {
        activeState.ActiveUpdate();
    }

    public void FixedUpdate()
    {
        activeState.ActiveFixedUpdate();
    }

    public void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            if(activeState != null)
                activeState.ActiveDrawGizmos();
        }
    }
}
