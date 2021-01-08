using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWireSphereGizmo : MonoBehaviour
{
    public bool onSelectedOnly;
    public Color gizmoColor = Color.white;

    public bool overrideScale;
    public float radiusOverride;

    private float CurrentRadius => (overrideScale ? radiusOverride : 0.5f);

    private void OnDrawGizmos()
    {
        if (onSelectedOnly) return;
        GizmoDraw();
    }

    private void OnDrawGizmosSelected()
    {
        if (!onSelectedOnly) return;
        GizmoDraw();
    }


    private void GizmoDraw()
    {
        Gizmos.color = gizmoColor;

        Vector3 pos = transform.position;
        if (!overrideScale)
        {
            //if we're using local scale, the center of the object is (0, 0, 0)
            Gizmos.matrix = transform.localToWorldMatrix;
            pos = new Vector3(0, 0, 0);
        }

        Gizmos.DrawWireSphere(pos, CurrentRadius);
    }
}
