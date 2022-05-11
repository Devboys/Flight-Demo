using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ProprietarySO/PlayerSettingsObject")]
public class PlayerSettingsObject : ScriptableObject
{
    [Header("Basic speed Settings")]
    [Tooltip("The base flying speed of the player. In units/second.")]
    public float initialFlySpeed = 10;
    [Tooltip("The desired minimum speed of the player. In units/second.")]
    public float minFlySpeed = 2;
    [Tooltip("The desired maximum speed of the player. In units/second.")]
    public float maxFlySpeed = 15;
    [Tooltip("How fast the players current speed is corrected when exceeding defined limits. In units/second^2")]
    public float speedCorrectionDelta = 5;

    [Header("Tilt & rotation Setting")]
    [Tooltip("The speed modifier when the player is fully tilted up or down. In units/second.")]
    public float tiltAcceleration = 5;
    [Tooltip("How long it takes the player to rotate vertically around themselves. In degrees/second.")]
    public float basePitchSpeed = 40;
    [Tooltip("Time required to go from idle to full pitch speed when turning at full tilt. In seconds.")]
    public float pitchSpeedDamping = 0.25f;
    [Tooltip("How long it takes the player to rotate horizontally around themselves. In degrees/second.")]
    public float baseYawSpeed = 40;
    [Tooltip("Time required to go from idle to full yaw speed when turning at full tilt. In seconds.")]
    public float yawSpeedDamping = 0.25f;
    [Tooltip("The maximum rotation of the player from neutral position (0 rotation). In degrees.")]
    public float maxPitchAngle = 50;
    [Tooltip("The minimum rotation of the player from neutral position (0 rotation). In degrees.")]
    public float minPitchAngle = -50;

    [Label("Flap cooldown/speedup delay is handled through animation events.")]
    [Header("Wing flap Settings")]
    [Tooltip("How much the player accelerates from a single wing flap. In units/second.")]
    public float flapSpeedMod = 8;

    [Header("Wing break vars")]
    [Tooltip("How much the player slows down when breaking with a wing. Applies doubly when breaking with both wings. In units/second.")]
    public float breakFlightDeceleration = -4;
    [Tooltip("How much faster the player rotates horizontally when breaking either left or right. In degrees/second.")]
    public float breakYawSpeedMod = -40;

    public void OnValidate()
    {
        ValidateUtils.EnsurePositiveOrZero(ref initialFlySpeed);
        ValidateUtils.EnsurePositiveOrZero(ref minFlySpeed);
        ValidateUtils.EnsurePositiveOrZero(ref maxFlySpeed);
        ValidateUtils.EnsurePositiveOrZero(ref speedCorrectionDelta);

        ValidateUtils.EnsurePositiveOrZero(ref tiltAcceleration);
        ValidateUtils.EnsurePositiveOrZero(ref basePitchSpeed);
        ValidateUtils.EnsurePositiveOrZero(ref baseYawSpeed);

        ValidateUtils.EnsureNegativeOrZero(ref minPitchAngle);
        ValidateUtils.EnsurePositiveOrZero(ref maxPitchAngle);

        ValidateUtils.EnsurePositiveOrZero(ref flapSpeedMod);

        ValidateUtils.EnsureNegativeOrZero(ref breakFlightDeceleration);
        ValidateUtils.EnsurePositiveOrZero(ref breakYawSpeedMod);
    }
}