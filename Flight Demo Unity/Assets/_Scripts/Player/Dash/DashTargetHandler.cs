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

    [Header("Output")]
    [SerializeField] private TransformVariable currentDashTargetVar = null;
    [SerializeField] private BoolVariable dashTargetValid = null;

    [Header("Settings")]
    [Tooltip("The maxmimum distance in world space a dash target can have to the player to be considered a target")]
    [SerializeField] private float maxWorldDistance = 5f;
    [SerializeField] private float minWorldDistance = 1f;
    [Tooltip("The maxmimum distance in viewport space a dash target can have from the center of the screen to be considered a target")]
    [SerializeField] private float maxViewportDistance = 2f;

    private List<DashTarget> dashTargets => dashTargetSet.items;
    private Camera mainCam;

    #region - Unity Callbacks -
    public void Awake()
    {
        mainCam = Camera.main;
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

        foreach (DashTarget consideredTarget in dashTargets)
        {
            if (IsInCenterView(consideredTarget.transform.position) && IsInFrontOfPlayer(consideredTarget.transform.position))
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

    private bool IsInCenterView(Vector3 pos)
    {
        Vector3 viewportPos = mainCam.WorldToViewportPoint(pos);

        //check if within camera bounds
        if (viewportPos.x > 0f && viewportPos.x < 1f && viewportPos.y > 0f && viewportPos.y < 1f && viewportPos.z > 0f)
        {
            //check if within center radius
            if (Vector2.Distance(viewportPos, new Vector2(0.5f, 0.5f)) < maxViewportDistance)
                return true;
        }

        return false;
    }

    private bool IsInFrontOfPlayer(Vector3 pos)
    {
        float camDistanceFromPlayer = Vector3.Distance(mainCam.transform.position, playerPositionVar.CurrentValue);
        float targetDistance = Vector3.Distance(mainCam.transform.position, pos);

        if (camDistanceFromPlayer < targetDistance) return true;
        else return false;

    }
}
