using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devboys.SharedObjects.RuntimeSets;

public class SpeedTrialObjectiveHandler : MonoBehaviour
{
    [SerializeField] private TransformRuntimeSet pickupSet;

    private void OnEnable()
    {
        pickupSet.onListChanged += HandleListUpdate;
    }

    private void OnDisable()
    {
        pickupSet.onListChanged -= HandleListUpdate;
    }

    private void HandleListUpdate(int setLength)
    {
        if (setLength == 0)
        {
            Debug.Log("you win!");
        }
    }
}