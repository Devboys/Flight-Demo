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
            state.moveStateHandler = this;
        }
    }

    public void Start()
    {
        SwitchToState<PlayerFlightController>(new Vector3(0,0,0 ), 0); //init state
    }

    /// <summary>
    /// switches to the given movement state and returns its instance.
    /// </summary>
    public T SwitchToState<T>(Vector3 currentDirection, float currentVelocity) where T : PlayerMovementStateBase
    {

        if (activeState != null && activeState.GetType().Equals(typeof(T))) return activeState as T;

        foreach(PlayerMovementStateBase state in movementStates)
        {
            if(state is T && !state.active)
            {
                state.active = true;
                activeState = state;
                state.HandleStateActivate(currentDirection, currentVelocity);
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
        activeState.StateUpdate();
    }

    public void FixedUpdate()
    {
        activeState.StateFixedUpdate();
    }

    public void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            if(activeState != null)
                activeState.StateDrawGizmos();
        }
    }
}
