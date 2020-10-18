using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathUIScript : MonoBehaviour
{
    [Tooltip("The UI object to enable/disable when player dies")]
    public RectTransform deathUIObject;

    [Tooltip("the duration that the death UI is activate for.")]
    public float activeDuration;

    /// <summary>
    /// Activates the deathUIObject;
    /// </summary>
    public void ActivateDeathUI()
    {
        deathUIObject.gameObject.SetActive(true);
        StartCoroutine(waitAndDeactivate());
    }

    private IEnumerator waitAndDeactivate()
    {

        yield return new WaitForSeconds(activeDuration);

        deathUIObject.gameObject.SetActive(false);

    }

}
