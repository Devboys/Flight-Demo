using Devboys.SharedObjects.Variables;
using Devboys.SharedObjects.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerDiveController : PlayerMovementStateBase
{

    private CharacterController _charController;

    [Header("Refs")] 
    public VectorVariable playerDir;
    public FloatVariable playerSpeed;

    public float airResistance = 1f;

    private Vector3 playerVec;
    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
    }

    public override void HandleStateActivate()
    {
        playerVec = playerDir.CurrentValue * playerSpeed.CurrentValue;
    }

    public override void ActiveUpdate()
    {
        playerVec -= new Vector3(airResistance/2f, 0, airResistance/2f) * Time.deltaTime;
    }

    public override void HandleStateDeactivate()
    {
        
    }
}
