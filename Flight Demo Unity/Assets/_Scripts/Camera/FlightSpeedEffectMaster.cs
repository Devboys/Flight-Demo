using Devboys.SharedObjects.Variables;
using UnityEngine;

public class FlightSpeedEffectMaster : MonoBehaviour
{
    [Header("Input refs")]
    public FloatReference playerSpeed;

    [Header("Output refs")]
    public FloatVariable smoothedZoom;
    public FloatVariable smoothedFOV;

    [Header("Settings")]
    public SO_SpeedFXVars settingVars;

    /// <summary>
    /// normalized "effect level" value
    /// </summary>
    private float smoothedEffectIntensity;


    public void Start()
    {
        smoothedEffectIntensity = GetNormalizedSpeed();
    }

    public void Update()
    {
        /* Big point is that we want a rigid relationship between camera distance("zoom") and FOV 
         * to prevent values having to "catch up" to each other. We use normalized speed as a 
         * basis for both, so to easily handle the fact that FOV and zoom has different min/max 
         * values, we simply calculate both these values from a shared "effect-intensity" variable,
         * which is directly calculated from normalized speed.
        */
        float effectIntensity = GetNormalizedSpeed();
        smoothedEffectIntensity = Mathf.MoveTowards(smoothedEffectIntensity, effectIntensity, settingVars.smoothSpeed * Time.deltaTime);

        //update Zoom vars
        smoothedZoom.CurrentValue = settingVars.minZoom + (smoothedEffectIntensity * (settingVars.maxZoom - settingVars.minZoom));

        //update FOV vars.
        smoothedFOV.CurrentValue = settingVars.ZoomToFOV(smoothedZoom.CurrentValue);
    }

    private float GetNormalizedSpeed()
    {
        float speed = playerSpeed.CurrentValue;

        if (speed < settingVars.minSpeed) return 0;
        if (speed > settingVars.maxSpeed) return 1;
        return (speed - settingVars.minSpeed) / (settingVars.maxSpeed - settingVars.minSpeed);
    }
}