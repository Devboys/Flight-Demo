using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetReplacementShader : MonoBehaviour
{
    public Shader replacementShader;

    private Camera _cam;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        if(replacementShader != null)
        {
            Debug.Log("applied shader");
            GetComponent<Camera>().SetReplacementShader(replacementShader, "RenderType"); //set global replacement shader for this camera.
        }
    }

    private void OnDisable()
    {
        Debug.Log("disabled shader");
        _cam.ResetReplacementShader();
    }
}
