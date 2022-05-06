using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerMovementStateBase : MonoBehaviour
{
    [ReadOnly] public bool active = false;
    [HideInInspector] public PlayerMovementStateHandler moveStateHandler;

    public virtual void HandleStateActivate(Vector3 prevStateDirection, float prevStateSpeed) {}
    public virtual void HandleStateDeactivate() {}

    public virtual void StateFixedUpdate(){}
    public virtual void StateUpdate(){}
    public virtual void StateDrawGizmos() {}
}
