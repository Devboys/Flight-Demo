using Devboys.SharedObjects.Variables;
using Devboys.SharedObjects.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerDashController : PlayerMovementStateBase
{
    public TransformReference currentDashTarget;
    public float dashSpeed = 35f;

    [Header("Events")]
    [SerializeField] private GameEvent dashStartEvent;
    [SerializeField] private GameEvent dashEndEvent;

    public FloatVariable currentSpeed;

    private Vector3 dashTargetPos;
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
        dashTargetPos = currentDashTarget.CurrentValue.position;
        dashDirection = (dashTargetPos - transform.position).normalized;
        dashDistance = Vector3.Distance(dashTargetPos, transform.position);
        dashTimer = dashDistance / dashSpeed;

        currentSpeed.CurrentValue = dashSpeed;

        dashStartEvent.Raise();
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

    public override void HandleStateDeactivate()
    {
        dashEndEvent.Raise();
    }
}
