using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerMovementStateBase : MonoBehaviour
{
    [ReadOnly] public bool active = false;
    [HideInInspector] public PlayerMovementStateHandler parentHandler;

    public virtual void HandleStateActivate() {}
    public virtual void HandleStateDeactivate() {}

    public virtual void ActiveFixedUpdate(){}
    public virtual void ActiveUpdate(){}
    public virtual void ActiveDrawGizmos() {}
}
