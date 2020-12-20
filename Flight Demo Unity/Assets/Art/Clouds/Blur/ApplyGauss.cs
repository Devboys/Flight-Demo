using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyGauss : MonoBehaviour
{
    public bool Active;

    public ComputeShader shader;
    public ComputeShader boxBlurShader;

    public RenderTexture inputTex;

    private RenderTexture outputTex;

    private RenderTexture outputTexBox;

    public float spread = 0.1f;
    [Range(3, 9)]
    public int kernelSize = 3;
    [Range(3, 9)]
    public int boxKernelSize = 3;

    Vector2Int textureSize;

    Renderer _rend;
    // Start is called before the first frame update
    void Start()
    {
        textureSize = new Vector2Int(inputTex.width, inputTex.height);

        outputTex = new RenderTexture(inputTex);
        outputTex.enableRandomWrite = true;
        outputTex.Create();

        outputTexBox = new RenderTexture(inputTex);
        outputTexBox.enableRandomWrite = true;
        outputTexBox.Create();


        _rend = GetComponent<Renderer>();
        _rend.enabled = true;

    }

    void Update()
    {
        if (Active)
        {
            //apply gauss
            int kernelHandle = shader.FindKernel("CSMain");
            shader.SetInt("_KernelSize", kernelSize);
            shader.SetFloat("_SpreadMul", spread);
            shader.SetTexture(kernelHandle, "_InputTex", inputTex);
            shader.SetTexture(kernelHandle, "_ResultTex", outputTex);
            shader.Dispatch(kernelHandle, textureSize.x / 16, textureSize.y / 9, 1); //expects 16:9 aspect

            //apply box
            int boxKernelHandle = boxBlurShader.FindKernel("CSMain");
            boxBlurShader.SetInt("_KernelSize", boxKernelSize);
            boxBlurShader.SetTexture(boxKernelHandle, "_InputTex", outputTex);
            boxBlurShader.SetTexture(boxKernelHandle, "_ResultTex", outputTexBox);
            boxBlurShader.Dispatch(boxKernelHandle, textureSize.x / 16, textureSize.y / 9, 1); //expects 16:9 aspect


            _rend.material.SetTexture("_BaseMap", outputTexBox);

        }
        else
        {
            _rend.material.SetTexture("_BaseMap", inputTex);
        }
    }
}
