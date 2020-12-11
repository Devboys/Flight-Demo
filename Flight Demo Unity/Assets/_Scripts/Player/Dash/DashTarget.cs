using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devboys.SharedObjects.Variables;
using Devboys.SharedObjects.RuntimeSets;

public class DashTarget : MonoBehaviour
{
    [SerializeField] private DashTargetSet targetSet = null;

    // Start is called before the first frame update
    void Start()
    {
        targetSet.AddItem(this);
    }
}
