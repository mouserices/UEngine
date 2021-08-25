
Shader "Hidden/MyShadow"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "Black" {}
		_FadeOutDistance("_FadeOutDistance", Range(0, 10)) = 4
		_FadeOutScale("_FadeOutScale", Range(0,1)) = 0.4
	}
	SubShader
	{
		Tags 
		{ 
			"RenderType"="Transparent"
			"Queue" = "Transparent -10"
		}
		

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			float4x4 _WorldToCameraMatrix;
			float4x4 _ProjectionMatrix;
			half _FadeOutDistance;
			fixed _FadeOutScale;

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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				float4 worldCoord = mul(unity_ObjectToWorld, v.vertex);
				float4 cameraCoord = mul(_WorldToCameraMatrix, worldCoord);
				float4 projectionCoord = mul(_ProjectionMatrix, cameraCoord);
				o.uv = projectionCoord / projectionCoord.w;
				o.uv = 0.5f*o.uv + float2(0.5f, 0.5f);

				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				half dis = (i.uv.x - 0.5f)*(i.uv.x - 0.5f) + (i.uv.y - 0.5f)*(i.uv.y - 0.5f);
				fixed4 col = tex2D(_MainTex, i.uv);
				//col.rgb = 0;
				col.a = (col.r - dis * _FadeOutDistance) * _FadeOutScale;
				col.rgb = 0;
				if (i.uv.y < 0.0f || i.uv.y > 1.0f || i.uv.x < 0.0f || i.uv.x > 1.0f)
				{
					discard;
				}
				return col;
			}
			ENDCG
		}
	}
}
