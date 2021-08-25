Shader "YanJia/Xray"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_RimColor("RimColor", Color) = (0, 1, 0, 1)
		_RimIntensity("Intensity", Range(-5, 5)) = 1
		[Toggle(ENABLE_TEXTURE)]_EnableTexure("FakeLight", Float) = 0
		_Alpha("Alpha", Range(0,1)) = 0.3
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Opaque" }

		Pass
		{
			ColorMask 0
		}

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite off
			Lighting off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#pragma multi_compile _ ENABLE_TEXTURE

			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : Normal;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				fixed4 color : COLOR;
				float2 uv : TEXCOORD0;
			};

			fixed4 _RimColor;
			float _RimIntensity;
			fixed _Alpha;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(a2v v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				fixed3 viewDir = normalize(ObjSpaceViewDir(v.vertex));//计算出顶点到相机的向量
				float val = 1 - saturate(dot(v.normal, viewDir));//计算点乘值
				o.color = lerp( fixed4(1,1,1,0), _RimColor,  val * (1 + _RimIntensity));//计算强度
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = 1;
#ifdef ENABLE_TEXTURE
				col.rgb = tex2D(_MainTex, i.uv).rgb;
#else
				col.rgb = i.color.rgb;
#endif
				col.a *= _Alpha;
				return col;
			}
			ENDCG
		}
	}
}