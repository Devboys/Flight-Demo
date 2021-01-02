Shader "Custom/CloudDecode"
{
    Properties
    {
        _BaseMap ("Base Map", 2D) = "white" {} //R = MainLight Intensity, G = Ambient Intensity, B = alpha, A = depth;
		_NoiseTex ("Noise Tex", 2D) = "white" {} //R + G channels = low frequency noise, B + A channels = high frequency noise
		_AmbientCol("Ambient Light Color", Color) = (1,1,1,1)
		_DistortionSpread("Distortion Spread", float) = 1
    }
    SubShader
    {
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha

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
			float _DistortionSpread;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 cloudData = tex2D(_BaseMap, i.uv);
				fixed4 noise = tex2D(_NoiseTex, i.uv);

				//decode cloud values
				float distance = cloudData.a;
				float inverseDistance = 1 - distance;

				//Make two blends of lowFreq -> highFreq noise based on distance (far = highFreq, close = lowFreq)
				float blendOne = (noise.r * inverseDistance) + (noise.b * distance); //y
				float blendTwo = (noise.g * inverseDistance) + (noise.a * distance); //x

				//the blends are used as offsets to the input-texture's UVs to give a distorted outline effect on the alpha channel.
				float2 distortedUV = i.uv;
				/*float distanceGate = (cloudAlpha >= distance) ? cloudAlpha : 0;*/
				distortedUV.x += (-_DistortionSpread + (2 * _DistortionSpread * blendTwo)) * inverseDistance;
				distortedUV.y += (-_DistortionSpread + (2 * _DistortionSpread * blendOne)) * inverseDistance;

				fixed4 distortedCloudData = tex2D(_BaseMap, distortedUV);

				float mainlightIntensity = distortedCloudData.r;
				float ambientIntensity = distortedCloudData.g;
				float cloudAlpha = distortedCloudData.b;
				
				fixed4 finalColor = (0, 0, 0, 0);
				//apply main light
				finalColor.rgb += _LightColor0.rgb * mainlightIntensity;
				//apply ambient light;
				finalColor.rgb += _AmbientCol.rgb * ambientIntensity;
				finalColor.a = cloudAlpha;

				//float gatedAlpha = (cloudAlpha >= distance) ? cloudAlpha : 0; //we're not branching since this provides the same value type.
				/*fixed4 finalColor = finalCloudData;*/
				////apply sunlight
				//finalColor.rgb += _LightColor0 * mainlightIntensity;
				////apply ambient light;
				//finalColor.rgb += _AmbientCol.rgb * ambientIntensity;
				/*finalColor.a = max(finalColor.b, distance);*/

				return finalColor; 
				/*return fixed4(cloudAlpha, cloudAlpha, cloudAlpha, 1);*/
				/*return fixed4(inverseDistance, inverseDistance, inverseDistance, 1);*/
				/*return fixed4(distance, distance, distance, 1);*/
            }
            ENDCG
        }
    }
}
