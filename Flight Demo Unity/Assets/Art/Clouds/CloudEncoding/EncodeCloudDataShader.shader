Shader "Custom/EncodeCloudDataShader"
{

	//This shader is intended for use in the cloud-rendering process. It encodes a number of values into the output image:
	//R - Sunlight value (0-1)
	//G - Skylight value (0-1)
	//B - pixel alpha (0-1). This will always be 1 at this stage, but will be processed later.
	//A - normalized pixel-depth (0-1)
    SubShader
    {
        Tags { "RenderType" = "Opaque" "LightMode"  = "UniversalForward" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
				float depth : DEPTH;
				float sunlight : LIGHT_MAIN;
				float ambient : LIGHT_AMBIENT;
            };

            v2f vert (appdata v)
            {
                v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				//get world normal of vertex
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);

				//calculate sunlight intensity using lambert diffuse (standard diffuse)
				o.sunlight = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
				
				//calculate ambient light intensity
				half3 ambientLight = ShadeSH9(half4(worldNormal, 1));
				//magic numbers are standard rgb->grayscale
				float ambientIntensity = 0.2126 * ambientLight.x + 0.7152 * ambientLight.y + 0.0722 * ambientLight.z;
				o.ambient = max(0, ambientIntensity);

				//calculate z-depth -----
				o.depth = -UnityObjectToViewPos(v.vertex).z * _ProjectionParams.w;

                return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				return fixed4(i.sunlight, i.ambient, 1, i.depth); //combine vertex calculations into texture color.
            }
            ENDCG
        }
    }
}
