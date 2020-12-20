Shader "Custom/Test/IsolateTextureRGBA"
{
    Properties
    {
        _BaseMap ("Base Texture", 2D) = "white" {}

		[Header(ChannelToggle(Leave only one enabled))]
		[Toggle] _DrawR("Draw R", float) = 0
		[Toggle] _DrawG("Draw G", float) = 0
		[Toggle] _DrawB("Draw B", float) = 0
		[Toggle] _DrawA("Draw A", float) = 0


    }
    SubShader
    {

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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
			float _DrawR;
			float _DrawG;
			float _DrawB;
			float _DrawA;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_BaseMap, i.uv);
				float r = (col.r * _DrawR) + (col.g * _DrawG) + (col.b * _DrawB) + (col.a * _DrawA);
				float g = (col.r * _DrawR) + (col.g * _DrawG) + (col.b * _DrawB) + (col.a * _DrawA);
				float b = (col.r * _DrawR) + (col.g * _DrawG) + (col.b * _DrawB) + (col.a * _DrawA);
				float a = 255;
				return fixed4(r, g, b, a);
            }
            ENDCG
        }
    }
}
