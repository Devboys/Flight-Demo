using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleToFrustum : MonoBehaviour
{
    public Camera targetCamera = null;

    public float distanceFromCamera = 1;

    private void OnValidate()
    {
        if (targetCamera != null)
        {
            float pos = targetCamera.nearClipPlane + distanceFromCamera;
            transform.position = targetCamera.transform.position + targetCamera.transform.forward * pos;

            float h = Mathf.Tan(targetCamera.fieldOfView * Mathf.Deg2Rad * 0.5f) * pos * 2f;
            transform.localScale = new Vector3(h * targetCamera.aspect, h, 0f);
        }
    }
}
