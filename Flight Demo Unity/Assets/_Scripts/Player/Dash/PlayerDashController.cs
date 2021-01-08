using Devboys.SharedObjects.Variables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerDashController : PlayerMovementStateBase
{
    public VectorReference currentDashTarget;
    public float dashSpeed = 35f;

    public FloatVariable currentSpeed;

    private Vector3 dashTarget;
    private Vector3 dashDirection;
    private float dashTimer; //time to reach dashtarget with dashspeed;
    private float dashDistance;

    private CharacterController _charController;

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
    }

    public override void HandleStateActivate()
    {
        //prep dash vars
        dashTarget = currentDashTarget.CurrentValue;
        dashDirection = (dashTarget - transform.position).normalized;
        dashDistance = Vector3.Distance(dashTarget, transform.position);
        dashTimer = dashDistance / dashSpeed;

        currentSpeed.CurrentValue = dashSpeed;
    }

    public override void ActiveUpdate()
    {
        transform.LookAt(this.transform.position + dashDirection.normalized, Vector3.up);

        _charController.Move(dashDirection * currentSpeed.CurrentValue * Time.deltaTime);

        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0)
        {
            parentHandler.SwitchToState<PlayerFlightController>();
        }
    }
}
