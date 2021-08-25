Shader "YanJia/Scene/BuildingTransparent"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_LightMapTex ("Lightmap Texture", 2D) = "white" {}
		//_LightMapIntensity ("Lightmap Intensity", Range(0, 2)) = 1
		_Alpha("Alpha", Range(0,1)) = 1
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent"}

        Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct a2v
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float4 color : Color;
			};

			struct v2f
			{
				float4 uv : TEXCOORD0;
				float4 color : Color;
				UNITY_FOG_COORDS(1)
				float4 pos : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _LightMapTex;
			float4 _LightMapTex_ST;
			fixed _Alpha;
			//half _LightMapIntensity;
			
			v2f vert (a2v v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv.zw = TRANSFORM_TEX(v.uv1, _LightMapTex);
				o.color = v.color;
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 albedo =  tex2D(_MainTex, i.uv.xy);
				fixed4 lightmap =  tex2D(_LightMapTex, i.uv.zw);
				fixed4 col = albedo * lightmap;
				col.a *= i.color.r * _Alpha;
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
