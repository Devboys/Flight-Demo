using UnityEngine;
using Devboys.SharedObjects.Variables;
using UnityEngine.InputSystem;
using System;

[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerDashController))]
public class PlayerFlightController : PlayerMovementStateBase
{
    [Header("Static Settings")]
    [SerializeField] private PlayerSettingsObject settings;

    [Label("Variables that this controller must check before switching to a dash state")]
    [Header("Dash transition vars")]
    [Tooltip("Shared object that indicates whether we have a valid dash target in sight. We cannot dash if we do not have a valid dash target to dash to.")]
    [SerializeField] private BoolReference validDashTarget = null;

    [Label("These shared object vars are used in other systems as read-only vars without a direct reference to the player object.")]
    [Header("Output vars")]
    [Tooltip("The shared object to hold the players current speed (used by other systems).")]
    [SerializeField] private FloatVariable currentSpeed = null;
    [SerializeField] private VectorVariable currentPositionVar = null;
    [SerializeField] private VectorVariable flyDirectionVar = null;

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private bool lockPosition;
    [SerializeField] private bool drawDebug;
#endif

    //cached local components
    private CharacterController _controller;
    private Animator _animator;

    //input vars
    private PlayerInputActions inputObject;
    private Vector2 movementInputVector;

    //fly vectors
    private Vector3 flyDirection;
    private float currentYawSpeed;
    private float currentYawSpeedVelocity;
    private float currentPitchSpeed;
    private float currentPitchSpeedVelocity;

    //base vectors tied to current flyDirection projected onto the xz-plane. used to make rotations unaffected by flydirection
    private Vector3 baseForward; 
    private Vector3 baseSideways;

    //flap vars
    private bool isFlapping;
    private Action onFlapStart;
    private Action onFlapEnd;

    private bool isDiving;



#region - Unity Callbacks - 
    private void Awake()
    {
        //cache components
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        currentYawSpeed = 0;

        currentSpeed.CurrentValue = settings.initialFlySpeed;
        inputObject = SharedPlayerInput.GetSceneInstance().GetPlayerInput();
    }


    //All movement is handled in ActiveUpdate. Flight control works by rotating the current forward vector (base
    public override void StateUpdate()
    {
        UpdateBaseVectors();

        DoFlightMove();
        
        DoWingBreak();

        DoFlightGravityAccelerate();

        ClampFlightSpeed();

#if UNITY_EDITOR
        if (lockPosition)
        {
            currentSpeed.CurrentValue = 0;
        }
#endif
        //rotate transform to face defined fly direction.
        this.transform.LookAt(transform.position + flyDirection, Vector3.up);
        _controller.Move(flyDirection * currentSpeed.CurrentValue * Time.deltaTime);

        //update shared vars
        flyDirectionVar.CurrentValue = flyDirection;
        currentPositionVar.CurrentValue = transform.position;

    }

    public override void HandleStateActivate(Vector3 prevStateDirection, float prevStateSpeed)
    {
        //flydirection is set on state activate to things like maintaing fly-direction when we end a dash.
        flyDirection = transform.forward;
        movementInputVector = inputObject.Player.Movement.ReadValue<Vector2>();
        currentPitchSpeed = 0;
        currentYawSpeed = 0;

        SubscribeControls();
        SubscribeAnimationEvents();
    }

    public override void HandleStateDeactivate()
    {
        UnsubscribeControls();
        UnsubscribeAnimationEvents();
        EndFlap();
    }

#if UNITY_EDITOR
    public override void StateDrawGizmos()
    {
        if (!drawDebug) return;
        Vector3 currentPos = transform.position;
        
        if (Application.isPlaying)
        {
            //draw forward vector
            GizmoUtils.DrawVector(3, 0.2f, currentPos, flyDirection, Color.cyan);

            //draw unrotated forward vector
            GizmoUtils.DrawVector(3, 0.2f, currentPos, baseForward, Color.yellow);
            GizmoUtils.DrawVector(3, 0.2f, currentPos, baseSideways, Color.black);

            //draw max/min rotations as vectors
            Vector3 maxVector = Quaternion.AngleAxis(settings.maxPitchAngle, baseSideways) * baseForward;
            GizmoUtils.DrawVector(3, 0.2f, currentPos, maxVector, Color.blue);
            Vector3 minVector = Quaternion.AngleAxis(settings.minPitchAngle, baseSideways) * baseForward;
            GizmoUtils.DrawVector(3, 0.2f, currentPos, minVector, Color.red);
        }
        else if (Application.isEditor )
        {
            //draw base vectors (these are just transform.forward and transform.right in edit mode)
            GizmoUtils.DrawVector(3, 0.2f, currentPos, transform.forward, Color.yellow);
            GizmoUtils.DrawVector(3, 0.2f, currentPos, transform.right, Color.black);

            //draw max/min rotations as vectors
            Vector3 maxVector = Quaternion.AngleAxis(settings.maxPitchAngle, currentPos) * transform.forward;

            GizmoUtils.DrawVector(3, 0.2f, currentPos, maxVector, Color.blue);
            Vector3 minVector = Quaternion.AngleAxis(settings.minPitchAngle, transform.right) * transform.forward;
            GizmoUtils.DrawVector(3, 0.2f, currentPos, minVector, Color.red);
        }
    }
#endif

#endregion

#region - Movement Submethods (used in update loop) - 
    /// <summary>
    /// Updates base vectors (baseForward & baseSideways) to follow current flyDirection. Base vectors are used in other movement methods.
    /// </summary>
    private void UpdateBaseVectors()
    {
        baseForward = Vector3.ProjectOnPlane(flyDirection, Vector3.up).normalized;
        baseSideways = Vector3.Cross(baseForward, Vector3.up);
    }

    private void DoFlightMove()
    {
        //rotate direction vector vertically (around local x-axis) on up/down input.
        float desiredPitchSpeed = 0.0f;
        if (Mathf.Abs(movementInputVector.y) > 0.01f)
        {
            desiredPitchSpeed = movementInputVector.y * settings.basePitchSpeed;
        }

        //pitch-turn velocity damping
        currentPitchSpeed = Mathf.SmoothDamp(currentPitchSpeed, desiredPitchSpeed, ref currentPitchSpeedVelocity, settings.pitchSpeedDamping);
        Vector3 desiredDirection = Quaternion.AngleAxis(currentPitchSpeed * Time.deltaTime, baseSideways) * flyDirection;

        //clamp desired direction to pitch extrema.
        float angle = Vector3.SignedAngle(baseForward, desiredDirection, baseSideways);
        if (angle > settings.maxPitchAngle)
        {
            desiredDirection = Quaternion.AngleAxis(settings.maxPitchAngle, baseSideways) * baseForward;
        }
        else if(angle < settings.minPitchAngle)
        {
            desiredDirection = Quaternion.AngleAxis(settings.minPitchAngle, baseSideways) * baseForward;
        }
        flyDirection = desiredDirection;

        //rotate direction vector sideways (around y-axis) on sideways input.
        float desiredYawSpeed = 0.0f;
        if (Mathf.Abs(movementInputVector.x) > 0.01f)
        {
            desiredYawSpeed = movementInputVector.x * settings.baseYawSpeed;
        }
        //yaw-turn velocity damping
        currentYawSpeed = Mathf.SmoothDamp(currentYawSpeed, desiredYawSpeed, ref currentYawSpeedVelocity, settings.yawSpeedDamping);
        flyDirection = Quaternion.AngleAxis(currentYawSpeed * Time.deltaTime, Vector3.up) * flyDirection;
    }

    private void DoWingBreak()
    {
        //Poll actions
        float breakLeftInput = inputObject.Player.BreakLeft.ReadValue<float>();
        float breakRightInput = inputObject.Player.BreakRight.ReadValue<float>();
        
        //Handle break left
        if (breakLeftInput >= 0.5f)
        {
            //decelerate
            currentSpeed.CurrentValue += settings.breakFlightDeceleration * Time.deltaTime;

            //rotate flight vector
            flyDirection = Quaternion.AngleAxis(-settings.breakYawSpeedMod * Time.deltaTime, Vector3.up) * flyDirection;
        }

        //Handle break right
        if (breakRightInput  >= 0.5f)
        {
            //decelerate
            currentSpeed.CurrentValue += settings.breakFlightDeceleration * Time.deltaTime;

            //rotate flight vector
            flyDirection = Quaternion.AngleAxis(settings.breakYawSpeedMod * Time.deltaTime, Vector3.up) * flyDirection;
        }

        //set animator vars
        _animator.SetFloat("LWingBreak", breakLeftInput);
        _animator.SetFloat("RWingBreak", breakRightInput);
    }

    private void DoFlightGravityAccelerate()
    {
        //accelerate or decelerate when pointing down or up, respectively.
        float verticalAngle = Vector3.SignedAngle(baseForward, flyDirection, baseSideways);
        if (verticalAngle != 0) currentSpeed.CurrentValue += -(verticalAngle / settings.maxPitchAngle) * settings.tiltAcceleration * Time.deltaTime;
    }

    private void ClampFlightSpeed()
    {
        //clamp speed to defined extrema.
        if (currentSpeed.CurrentValue > settings.maxFlySpeed)
        {
            currentSpeed.CurrentValue = Mathf.MoveTowards(currentSpeed.CurrentValue, settings.maxFlySpeed, settings.speedCorrectionDelta * Time.deltaTime);
        }
        else if (currentSpeed.CurrentValue < settings.minFlySpeed)
        {
            //no correction delta on min fly speed
            currentSpeed.CurrentValue = settings.minFlySpeed;
        }

    }
    
#endregion

#region - Input Subscribers & Methods - 
    private void SubscribeControls()
    {
        inputObject.Player.Flap.performed += Input_OnFlap;
        inputObject.Player.Movement.performed += Input_OnMovement;
        inputObject.Player.Dash.performed += Input_OnDash;
        inputObject.Player.Dive.started += Input_OnDive;
        inputObject.Player.Dive.canceled += Input_OnDive;
        // inputObject.Player.BreakLeft.performed += Input_BreakLeft;
        // inputObject.Player.BreakRight.performed += Input_BreakRight;
    }

    private void UnsubscribeControls()
    {
        inputObject.Player.Flap.performed -= Input_OnFlap;
        inputObject.Player.Movement.performed -= Input_OnMovement;
        inputObject.Player.Dash.performed -= Input_OnDash;
        inputObject.Player.Dive.started -= Input_OnDive;
        inputObject.Player.Dive.canceled -= Input_OnDive;
        // inputObject.Player.BreakLeft.performed -= Input_BreakLeft;
        // inputObject.Player.BreakRight.performed -= Input_BreakRight;
    }

    private void Input_OnMovement(InputAction.CallbackContext context)
    {
        movementInputVector = context.ReadValue<Vector2>();
    }

    private void Input_OnDash(InputAction.CallbackContext context)
    {
        if (validDashTarget.CurrentValue == true)
        {
            moveStateHandler.SwitchToState<PlayerDashController>(flyDirection, currentSpeed.CurrentValue);
        }
    }

    private void Input_OnFlap(InputAction.CallbackContext context)
    {
        if (isFlapping) return;
        
        isFlapping = true;
        _animator.SetTrigger("Flap");
    }
    
    private void Input_OnDive(InputAction.CallbackContext context)
    {
        // Super weird solution
        bool buttonDown = context.ReadValue<float>() > 0.5 ;
        _animator.SetBool("Dive", buttonDown);
        if(buttonDown)
            _animator.SetTrigger("Dive_Started");

        moveStateHandler.SwitchToState<PlayerDiveController>(flyDirection, currentSpeed.CurrentValue);
    } 

#endregion

#region - Animation Event Handlers -

    /// <summary>
    /// Used by animation events only
    /// </summary>
    public void Animation_HandleFlapStart()
    {
        onFlapStart?.Invoke();
    }
    /// <summary>
    /// Used by animation events only
    /// </summary>
    public void Animation_HandleFlapEnd()
    {
        onFlapEnd?.Invoke();
    }


    /// <summary>
    /// Subscribes methods to their corresponding animation-event actions.
    /// </summary>
    private void SubscribeAnimationEvents()
    {
        onFlapStart += FlapAccelerate;
        onFlapEnd += EndFlap;
    }

    /// <summary>
    /// Subscribes methods from their corresponding animation-event actions.
    /// </summary>
    private void UnsubscribeAnimationEvents()
    {
        onFlapStart -= FlapAccelerate;
        onFlapEnd -= EndFlap;
    }

    void FlapAccelerate()
    {
        currentSpeed.CurrentValue += settings.flapSpeedMod;
    }

    void EndFlap()
    {
        isFlapping = false;
    }

#endregion
}
