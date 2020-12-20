using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyBox : MonoBehaviour
{
    public bool Active;

    public ComputeShader shader;
    public RenderTexture inputTex;

    private RenderTexture outputTex;

    Renderer _rend;
    // Start is called before the first frame update
    void Start()
    {
        outputTex = new RenderTexture(inputTex);
        outputTex.enableRandomWrite = true;
        outputTex.Create();

        _rend = GetComponent<Renderer>();
        _rend.enabled = true;

    }

    void Update()
    {
        if (Active)
        {
            int kernelHandle = shader.FindKernel("CSMain");
            shader.SetInt("_KernelSize", 3);
            shader.SetTexture(kernelHandle, "_InputTex", inputTex);
            shader.SetTexture(kernelHandle, "_ResultTex", outputTex);
            shader.Dispatch(kernelHandle, 256 / 8, 256 / 8, 1);

            _rend.material.SetTexture("_BaseMap", outputTex);
        }
        else
        {
            _rend.material.SetTexture("_BaseMap", inputTex);
        }
    }
}
