Shader "Custom/CloudDecode"
{
    Properties
    {
        _BaseMap ("Base Map", 2D) = "white" {}
		_NoiseTex ("Noise Tex", 2D) = "white" {} //R + G channels = low frequency noise, B + A channels = high frequency noise
		_AmbientCol("Ambient Light Color", Color) = (1,1,1,1)
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
			fixed4 _AmbientCol;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_BaseMap, i.uv);
				fixed4 noise = tex2D(_NoiseTex, i.uv);

				float cloudAlpha = col.b;
				float distance = col.a;
				float inverseDistance = 1 - distance;

				//names are not indicative - error named

				float blendOne = (noise.r * inverseDistance) + (noise.b * distance);
				float blendTwo = (noise.g * inverseDistance) + (noise.a * distance);

				//blend between the two noise channels
				/*col.r = blendOne;
				col.g = blendTwo;
				col.b = 0;
				col.a = 1;*/

				fixed4 finalColor = fixed4(0, 0, 0, 0);
				//apply sunlight
				finalColor.rgb += _LightColor0 * col.r;
				//apply ambient light;
				finalColor.rgb += _AmbientCol.rgb * col.g;
				finalColor.a = col.a * noise.r;

				return finalColor;
            }
            ENDCG
        }
    }
}
