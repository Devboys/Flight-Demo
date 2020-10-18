using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devboys.SharedObjects.Variables;
using Devboys.SharedObjects.Events;

public class PlayerHealthController : MonoBehaviour
{
    [Tooltip("The players current health. Exposed as shared variable so other systems can access it.")]
    public FloatVariable playerHealth;

    [Tooltip("The event that gets called when the player dies")]
    public GameEvent playerDeathEvent;

    public void Update()
    {
    }

    public void ModHealth(float value)
    {
        playerHealth.CurrentValue -= value;

        if (playerHealth.CurrentValue <= 0) Die();
    }

    public void Die()
    {
        playerHealth.ResetToDefault();
        playerDeathEvent.Raise();
    }


}
