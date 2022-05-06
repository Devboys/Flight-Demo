using Devboys.SharedObjects.Variables;
using Devboys.SharedObjects.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerDiveController : PlayerMovementStateBase
{

    [Header("Physics")]
    public float airResistance = 1f;
    public float gravity = 9.82f;
    public float maxDownVelocity;

    [Header("Air Control")]
    public bool allowHorizontalControl = true;
    public bool allowVerticalControl = false;
    public float airControlFactor = 0.1f;


    [Header("Refs")]
    [SerializeField] private FloatVariable currentSpeed = null;
    [SerializeField] private VectorVariable currentPositionVar = null;
    [SerializeField] private VectorVariable flyDirectionVar = null;

    private Vector3 diveVelocity;

    //cached components
    private CharacterController _charController;
    private Animator _animator;
    private PlayerInputActions inputObject;

    private Vector2 movementInput;

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private bool LockPosition;
#endif

    private void OnValidate()
    {
        gravity = -Mathf.Abs(gravity);
    }

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        inputObject = SharedPlayerInput.GetSceneInstance().GetPlayerInput();
    }

    public override void HandleStateActivate(Vector3 prevStateDirection, float prevStateSpeed)
    {
        diveVelocity = prevStateDirection * prevStateSpeed;
        currentSpeed.CurrentValue = prevStateSpeed;
        SubscribeInput();
    }

    public override void HandleStateDeactivate()
    {
        UnsubscribeInput();
    }

    public override void StateUpdate()
    {
        //simple projectile motion
        diveVelocity.y += gravity * Time.deltaTime;

        //apply air control
        Vector3 controlMod = GetAirControlMod();
        Vector3 delta = (diveVelocity.normalized + controlMod * Time.deltaTime).normalized * diveVelocity.magnitude;

#if UNITY_EDITOR
        if (LockPosition)
        {
            delta = Vector3.zero;
        }
#endif
        diveVelocity = delta;

        //commit rotation & movement
        transform.LookAt(transform.position + delta, Vector3.up);
        _charController.Move(delta * Time.deltaTime);

        //update shared vars
        flyDirectionVar.CurrentValue = diveVelocity.normalized;
        currentSpeed.CurrentValue = diveVelocity.magnitude;
        currentPositionVar.CurrentValue = transform.position;
    }

    private Vector3 GetAirControlMod()
    {
        //Construct base vectors relative to forward direction
        Vector3 currentDir = diveVelocity.normalized;
        Vector3 rightVector = Vector3.Cross(Vector3.up, currentDir).normalized;
        Vector3 upVector = Vector3.Cross(currentDir, rightVector);

        //create air mod direction vector
        Vector3 modDir = Vector3.zero;
        modDir += allowHorizontalControl ? (rightVector * movementInput.x) : Vector3.zero;
        modDir += allowVerticalControl ? (upVector * movementInput.y) : Vector3.zero;
        return modDir * airControlFactor;
    }

    #region Input

    private void SubscribeInput()
    {
        if(inputObject != null)
        {
            inputObject.Player.Dive.canceled += Input_OnDive;
            inputObject.Player.Movement.performed += Input_OnMovement;
        }
    }
    
    private void UnsubscribeInput()
    {
        if(inputObject != null)
        {
            inputObject.Player.Dive.canceled -= Input_OnDive;
            inputObject.Player.Movement.performed -= Input_OnMovement;
        }
    }

    private void Input_OnDive(InputAction.CallbackContext context)
    {
        bool buttonDown = context.ReadValue<float>() > 0.5;
        _animator.SetBool("Dive", buttonDown);
        if (buttonDown)
            _animator.SetTrigger("Dive_Started");

        moveStateHandler.SwitchToState<PlayerFlightController>(diveVelocity.normalized, diveVelocity.magnitude);
    }

    private void Input_OnMovement(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();

    }
    
    #endregion
}
