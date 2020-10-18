﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devboys.SharedObjects.Variables;

public class PlayerFlightController : MonoBehaviour
{
    [Header("Basic speed vars")]
    [Tooltip("The base flying speed of the player. In units/second.")]
    [SerializeField] private float initialFlySpeed = 10;
    [Tooltip("The minimum speed of the player. In units/second.")]
    [SerializeField] private float minFlySpeed = 2;
    [Tooltip("The maximum speed of the player. In units/second.")]
    [SerializeField] private float maxFlySpeed = 15;
    [Tooltip("The shared object to hold the players current speed (used by other systems).")]
    [SerializeField] private FloatVariable currentSpeed;

    [Header("Tilt & rotation vars")]
    [Tooltip("The speed modifier when the player is fully tilted up or down. In units/second.")]
    [SerializeField] private float tiltAcceleration = 5;
    [Tooltip("How long it takes the player to rotate vertically around themselves. In degrees/second.")]
    [SerializeField] private float verticalRotationSpeed = 40;
    [Tooltip("How long it takes the player to rotate horizontally around themselves. In degrees/second.")]
    [SerializeField] private float horizontalRotationSpeed = 40;
    [Tooltip("The maximum rotation of the player from neutral position (0 rotation). In degrees.")]
    [SerializeField] private float maxVerticalAngle = 30;
    [Tooltip("The minimum rotation of the player from neutral position (0 rotation). In degrees.")]
    [SerializeField] private float minVerticalAngle = -30;

    [Label("Flap cooldown/speedup delay is handled through animation events.")]
    [Header("Wing flap vars")]
    [Tooltip("How much the player accelerates from a single wing flap.")]
    [SerializeField] private float flapSpeedMod = 2;

#if UNITY_EDITOR
    [Header("Gizmos")]
    public bool drawGizmos = true;
#endif


    //cached local components
    private CharacterController _controller;
    private Animator _animator;

    //input vars
    private PlayerInputActions inputObject;
    private Vector2 movementInputVector;

    //fly vectors
    private Vector3 flyDirection;
    [SerializeField] private VectorVariable flyDirectionVar;
    //base vectors tied to current flyDirection projected onto the xz-plane. used to make rotations unaffected by flydirection
    private Vector3 baseForward; 
    private Vector3 baseSideways;

    //flap vars
    private bool isFlapping;

    #region - Unity Callbacks - 
    private void Awake()
    {
        //cache components
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        inputObject = new PlayerInputActions();

        SubscribeControls();
        currentSpeed.CurrentValue = initialFlySpeed;
    }

    private void Start()
    {
        flyDirection = Vector3.forward;
    }

    private void FixedUpdate()
    {
        //update base vectors;
        baseForward = Vector3.ProjectOnPlane(flyDirection, Vector3.up).normalized;
        baseSideways = Vector3.Cross(baseForward, Vector3.up);

        if (Mathf.Abs(movementInputVector.y) > 0.01f)
        {
            float rot = movementInputVector.y * verticalRotationSpeed * Time.fixedDeltaTime;
            Vector3 desiredDirection = Quaternion.AngleAxis(rot, baseSideways) * flyDirection;

            float angle = Vector3.SignedAngle(baseForward, desiredDirection, baseSideways);

            if (Mathf.Sign(movementInputVector.y) == 1)
            {
                if (angle < maxVerticalAngle)
                {
                    flyDirection = desiredDirection;
                }
                else
                {
                    flyDirection = GetRotatedBaseVector(false);
                }
            }
            else if (Mathf.Sign(movementInputVector.y) == -1)
            {
                if(angle > minVerticalAngle)
                {
                    flyDirection = desiredDirection;
                }
                else
                {
                    flyDirection = GetRotatedBaseVector(true);
                }
            }
        }

        if(Mathf.Abs(movementInputVector.x) > 0.01f)
        {
            flyDirection = Quaternion.AngleAxis(horizontalRotationSpeed * movementInputVector.x * Time.fixedDeltaTime, Vector3.up) * flyDirection;
        }

        float verticalAngle = Vector3.SignedAngle(baseForward, flyDirection, baseSideways);
        if (verticalAngle != 0) currentSpeed.CurrentValue += - (verticalAngle / maxVerticalAngle) * tiltAcceleration * Time.fixedDeltaTime;
        currentSpeed.CurrentValue = Mathf.Clamp(currentSpeed.CurrentValue, minFlySpeed, maxFlySpeed);

        this.transform.LookAt(transform.position + flyDirection, Vector3.up);
        flyDirectionVar.CurrentValue = flyDirection;

        _controller.Move(flyDirection * currentSpeed.CurrentValue * Time.fixedDeltaTime);
    }

    private void OnEnable()
    {
        inputObject.Enable();
    }

    private void OnDisable()
    {
        inputObject.Disable();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (drawGizmos && Application.isPlaying)
        {
            //draw forward vector
            GizmoUtils.DrawVector(3, 0.2f, transform.position, flyDirection, Color.cyan);

            //draw unrotated forward vector
            GizmoUtils.DrawVector(3, 0.2f, transform.position, baseForward, Color.yellow);
            GizmoUtils.DrawVector(3, 0.2f, transform.position, baseSideways, Color.black);

            //draw max/min rotations as vectors
            Vector3 maxVector = Quaternion.AngleAxis(maxVerticalAngle, baseSideways) * baseForward;
            GizmoUtils.DrawVector(3, 0.2f, transform.position, maxVector, Color.blue);
            Vector3 minVector = Quaternion.AngleAxis(minVerticalAngle, baseSideways) * baseForward;
            GizmoUtils.DrawVector(3, 0.2f, transform.position, minVector, Color.red);
        }
        else if (drawGizmos && Application.isEditor)
        {
            //draw base vectors (these are just transform.forward and transform.right in edit mode)
            GizmoUtils.DrawVector(3, 0.2f, transform.position, transform.forward, Color.yellow);
            GizmoUtils.DrawVector(3, 0.2f, transform.position, transform.right, Color.black);

            //draw max/min rotations as vectors
            Vector3 maxVector = Quaternion.AngleAxis(maxVerticalAngle, transform.right) * transform.forward;
            
            GizmoUtils.DrawVector(3, 0.2f, transform.position, maxVector, Color.blue);
            Vector3 minVector = Quaternion.AngleAxis(minVerticalAngle, transform.right) * transform.forward;
            GizmoUtils.DrawVector(3, 0.2f, transform.position, minVector, Color.red);
        }
    }
#endif

#endregion

    private void SubscribeControls()
    {
        inputObject.Movement.Jump.performed += x => Flap();
        inputObject.Movement.Movement.performed += x =>
        {
            movementInputVector = x.ReadValue<Vector2>();
        };
    }

    void Flap()
    {
        if (!isFlapping)
        {
            isFlapping = true;
            _animator.SetTrigger("Flap");
        }
    }
    //used by animation events
    void FlapSpeedMod()
    {
        currentSpeed.CurrentValue += flapSpeedMod;
    }

    //used by animation events
    void EndFlap()
    {
        isFlapping = false;
    }

    /// <summary>
    /// Returns a vector rotated exactly to the degrees indicated by either minVerticalRotation or maxVerticalRotation. 
    /// </summary>
    private Vector3 GetRotatedBaseVector(bool returnMin)
    {
        Vector3 result = Quaternion.AngleAxis((returnMin ? minVerticalAngle : maxVerticalAngle), baseSideways) * baseForward;
        return result;
    }
}
