using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devboys.SharedObjects.Variables;
using Devboys.SharedObjects.RuntimeSets;

public class DashTargetHandler : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private DashTargetSet dashTargetSet = null;
    [SerializeField] private VectorReference playerPositionVar = null;
    [SerializeField] private VectorReference playerDirRef = null;

    [Header("Output")]
    [SerializeField] private TransformVariable currentDashTargetVar = null;
    [SerializeField] private BoolVariable dashTargetValid = null;

    [Header("Settings")]
    [Tooltip("The maxmimum distance in world space a dash target can have to the player to be considered a target")]
    [SerializeField] private float maxWorldDistance = 5f;
    [SerializeField] private float minWorldDistance = 1f;
    [Tooltip("The maxmimum distance in viewport space a dash target can have from the center of the screen to be considered a target")]
    [SerializeField] private float maxViewportDistance = 2f;
    [SerializeField] private LayerMask dashObstructMask;
    
    [SerializeField] private float dotLimit;

    private List<DashTarget> dashTargets => dashTargetSet.items;
    private Camera mainCam;

    private float maxDistSqr;
    private float minDistSqr;

    #region - Unity Callbacks -
    public void Awake()
    {
        mainCam = Camera.main;

        maxDistSqr = Mathf.Pow(maxWorldDistance, 2);
        minDistSqr = Mathf.Pow(minWorldDistance, 2);
    }

    public void Update()
    {
        DashTarget closestTarget = GetClosestValidTarget(playerPositionVar.CurrentValue);
        if (closestTarget != null)
        {
            currentDashTargetVar.CurrentValue = closestTarget.transform;
            dashTargetValid.CurrentValue = true;
        }
        else
        {
            dashTargetValid.CurrentValue = false;
        }
    }

    private void OnValidate()
    {
        dotLimit = Mathf.Clamp(dotLimit, -1, 1);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(playerPositionVar.CurrentValue, maxWorldDistance);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(playerPositionVar.CurrentValue, minWorldDistance);
    }
    #endregion

    private DashTarget GetClosestValidTarget(Vector3 playerPos)
    {
        float shortestDistance = 9999;
        DashTarget closestTarget = null;

        //Brute-force search closest distance. TODO: Should be optimized.
        foreach (DashTarget consideredTarget in dashTargets)
        {
            if (!consideredTarget.gameObject.activeSelf) continue;
            Vector3 targetPos = consideredTarget.transform.position;
            if (IsTargetValid(targetPos))
            {
                float distance = Vector3.Distance(playerPos, consideredTarget.transform.position);
                if (distance < maxWorldDistance && distance > minWorldDistance && distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestTarget = consideredTarget;
                }
            }
        }
        return closestTarget;
    }

    private bool IsTargetValid(Vector3 targetPos)
    {
        bool result = true;
        Vector3 targetDirVec = targetPos - playerPositionVar.CurrentValue;
        
        //is target within distance limits?
        float distToTarget = targetDirVec.sqrMagnitude;
        if ( distToTarget > maxDistSqr || distToTarget < minDistSqr)
        {
            result = false;
        }

        //is target in front of player?
        float dot = Vector3.Dot(targetDirVec.normalized, playerDirRef.CurrentValue.normalized);
        if(dot < dotLimit)
        {
            result = false;
        }

        //is target on screen?
        Vector2 viewportPos = mainCam.WorldToViewportPoint(targetPos);
        if(viewportPos.x < 0f || viewportPos.x > 1 || viewportPos.y < 0f || viewportPos.y > 1)
        {
            result = false;
        }

        //is path to object unobstructed?
        if (Physics.Linecast(playerPositionVar.CurrentValue, targetPos, dashObstructMask))
        {
            result = false;
        }

        return result;
    }
}
