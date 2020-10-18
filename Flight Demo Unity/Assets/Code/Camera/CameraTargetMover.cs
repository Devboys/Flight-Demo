using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devboys.SharedObjects.Variables;

public class CameraTargetMover : MonoBehaviour
{
    [SerializeField] public VectorReference playerDirectionReference;
    public float radius;

    private Vector3 initialPos;

    public void Start()
    {
        initialPos = this.transform.localPosition;
    }

    public void Update()
    {
        //move target immediately to the players direction
        Vector3 playerDir = playerDirectionReference.CurrentValue;
        Vector3 targetPos = initialPos + playerDir * radius;
        transform.localPosition = targetPos;
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        if (Application.isPlaying)
        {
            Gizmos.DrawWireSphere(initialPos, radius);
        }
        else if (Application.isEditor)
        {
            Gizmos.DrawWireSphere(this.transform.position, radius);
        }
    }
}
