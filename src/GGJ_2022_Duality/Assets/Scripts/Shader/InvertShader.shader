Shader "Custom/2D/InvertColor"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Threshold("Threshold", Range(0., 1.)) = 0
	}
		SubShader
		{
			Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
				#pragma vertex vert_img
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

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					return o;
				}

				sampler2D _MainTex;
				float _Threshold;

				fixed4 frag(v2f_img i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.uv);
					col.rgb = abs(_Threshold - col.rgb);
					return col;
				}
				ENDCG
			}
		}
}