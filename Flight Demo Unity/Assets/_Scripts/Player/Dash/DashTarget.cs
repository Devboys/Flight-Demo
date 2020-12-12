using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devboys.SharedObjects.Variables;
using Devboys.SharedObjects.RuntimeSets;

public class DashTarget : MonoBehaviour
{
    [SerializeField] private DashTargetSet targetSet = null;

    private void OnEnable()
    {
        targetSet.AddItem(this);
    }

    private void OnDisable()
    {
        targetSet.RemoveItem(this);
    }
}
