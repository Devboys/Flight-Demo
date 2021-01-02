using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class FlyCameraController : MonoBehaviour
{
    [Header("Input subscribes (Input Actions)")]
    public InputActionReference LookAction = null; //mouse XY in editor;
    public InputActionReference MoveAction = null; //WASD in editor (must be composite)
    public InputActionReference CameraUpAction = null; //E in editor
    public InputActionReference CameraDownAction = null; //Q in editor;
    public InputActionReference SprintAction = null; //Shift in editor;
    public InputActionReference AdjustSpeedAction = null; //mouse scroll in editor (must be a value)
    public InputActionReference ToggleCloseModeAction = null; //does not exist in editor

    [Header("Movement parameters")]
    [Tooltip("Minimum speed that camera can go to")]
    public float minSpeed = 10f;
    [Tooltip("Maximum speed that camera can go to")]
    public float maxSpeed = 100f;
    [Tooltip("Speed when scene starts")]
    public float defaultSpeed = 20f;
    [Tooltip("Speed multiplier when player sprints")]
    public float sprintMultiplier = 2.5f;
    [Tooltip("How sensitive the camera is to look-input when looking around")]
    public float lookSensitivity = 0.25f;
    [Tooltip("How much of an influence the adjust-speed input has on the speed")]
    public float speedAdjustSensitivity = 0.5f;

    [Label("'Close-Mode' makes the controller use other min/max speeds to allow the user to look at smaller objects more easily")]
    [Header("Close-Mode")]
    public float closeModeMinSpeed = 0.1f;
    public float closeModeMaxSpeed = 10f;

    [Header("State values")]
    [ReadOnly] [SerializeField] private float currentSpeed;
    [ReadOnly] [SerializeField] private bool inCloseMode; //Close mode uses other min/max speeds to allow you to look at smaller objects more easily

    private CursorLockMode prevMode;

    private float CurrentMaxSpeed => (inCloseMode ? closeModeMaxSpeed : maxSpeed);
    private float CurrentMinSpeed => (inCloseMode ? closeModeMinSpeed : minSpeed);

    private void OnValidate()
    {
        defaultSpeed = Mathf.Clamp(defaultSpeed, minSpeed, maxSpeed);
    }

    private void OnEnable()
    {
        //record previous cursor state for when we need to revert.
        prevMode = Cursor.lockState;

        //hide & lock cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //enable all actions
        MoveAction.action.Enable();
        LookAction.action.Enable();
        CameraUpAction.action.Enable();
        CameraDownAction.action.Enable();
        SprintAction.action.Enable(); 
        AdjustSpeedAction.action.Enable();
        ToggleCloseModeAction.action.Enable();
}

    private void OnDisable()
    {
        //re-enable cursor & revert lockstate
        Cursor.visible = true;
        Cursor.lockState = prevMode;

        //disable all actions
        LookAction.action.Disable();
        MoveAction.action.Disable();
        CameraUpAction.action.Disable();
        CameraDownAction.action.Disable();
        SprintAction.action.Disable();
        AdjustSpeedAction.action.Disable();
        ToggleCloseModeAction.action.Disable();
    }

    private void Start()
    {
        currentSpeed = defaultSpeed;
    }

    void Update()
    {
        //---- Look input ----
        Vector2 mouseDelta = LookAction.action.ReadValue<Vector2>();
        Vector2 lookRotEuler = new Vector2(-mouseDelta.y * lookSensitivity, mouseDelta.x * lookSensitivity);
        Vector3 eulerRotation = new Vector3(transform.eulerAngles.x + lookRotEuler.x, transform.eulerAngles.y + lookRotEuler.y, 0);
        transform.eulerAngles = eulerRotation;

        //---- Keyboard commands ----
        Vector3 moveVector = GetBaseInput(); //get vector representing input direction;

        //Toggle Close-Mode
        if(ToggleCloseModeAction.action.triggered && ToggleCloseModeAction.action.ReadValue<float>() != 0)
        {
            //normalize current speed by pre-toggle extrema (min/max values).
            float normalizedSpeed = (currentSpeed - CurrentMinSpeed) / (CurrentMaxSpeed - CurrentMinSpeed);

            inCloseMode = !inCloseMode;

            //translate normalized speed to newly changed extrema (denormalize).
            currentSpeed = (CurrentMaxSpeed - CurrentMinSpeed) * normalizedSpeed + CurrentMinSpeed;
        }

        //accelerate if shift pressed.
        if (SprintAction.action.ReadValue<float>() != 0)
        {
            moveVector = moveVector * currentSpeed * sprintMultiplier;
        }
        else
        {
            moveVector = moveVector * currentSpeed;
        }

        //Move camera up when up key is pressed
        if(CameraUpAction.action.ReadValue<float>() != 0)
        {
            moveVector.y = currentSpeed;
        }

        //Move camera down when down key is pressed
        if (CameraDownAction.action.ReadValue<float>() != 0)
        {
            moveVector.y = -currentSpeed;
        }

        //Adjust speed if scroll-wheel is scrolled.
        float scrollInput = AdjustSpeedAction.action.ReadValue<float>();
        if(scrollInput != 0)
        {
            currentSpeed += scrollInput * speedAdjustSensitivity * (CurrentMaxSpeed - CurrentMinSpeed) * 0.1f * Time.deltaTime;

            //clamp speed
            currentSpeed = Mathf.Clamp(currentSpeed, CurrentMinSpeed, CurrentMaxSpeed);
        }

        //---- Final ----
        //apply movement.
        moveVector = moveVector * Time.deltaTime;
        transform.Translate(moveVector);
    }

    private Vector3 GetBaseInput()
    { //returns the basic values, if it's 0 than it's not active.
        Vector2 moveInput = MoveAction.action.ReadValue<Vector2>();

        //Relative movement is in x and z-axis.
        Vector3 relativeMovement = new Vector3(moveInput.x, 0, moveInput.y);

        return relativeMovement;
    }
}