Shader "Custom/Test/ShowNoiseBlend"
{
    Properties
    {
        _BaseMap ("Base Map", 2D) = "white" {} //R = MainLight Intensity, G = Ambient Intensity, B = alpha, A = depth;
		_NoiseTex ("Noise Tex", 2D) = "white" {} //R + G channels = low frequency noise, B + A channels = high frequency noise
    }
    SubShader
    {

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"
			#include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _BaseMap;
			sampler2D _NoiseTex;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 cloudTex = tex2D(_BaseMap, i.uv);
				fixed4 noise = tex2D(_NoiseTex, i.uv);

				float cloudAlpha = cloudTex.b;
				float distance = cloudTex.a;
				float inverseDistance = 1 - distance;

				//Make two blends of lowFreq -> highFreq noise based on distance (far = highFreq, close = lowFreq)
				float blendOne = (noise.r * inverseDistance) + (noise.b * distance);
				float blendTwo = (noise.g * inverseDistance) + (noise.a * distance);

				fixed4 finalColor = fixed4(1.0, 1.0, 1.0, 1.0);
				finalColor.r = blendOne * cloudAlpha;
				finalColor.g = blendTwo * cloudAlpha;
				finalColor.b = 0;
				finalColor.a = 1;

				return finalColor;
            }
            ENDCG
        }
    }
}
