Shader "YanJia/Scene/Building"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_LightMapTex ("Lightmap Texture", 2D) = "white" {}
		_BloomPower ("Bloom Power", Range(0,1)) = 0.5
		[Toggle(ENABLE_CUSTOM_FOG)] _EnableCustomFog ("Enable Custom fog", Float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma multi_compile _ ENABLE_CUSTOM_FOG
			
			#include "UnityCG.cginc"
			#include "HLSLSupport.cginc"
			#include "../CustomShader.cginc"

			struct a2v
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float3 worldPos : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _LightMapTex;
			float4 _LightMapTex_ST;
			half _BloomPower;
		

			float _GLOBALBRIGHTNESS;
			float _GLOBALCONTRASR;
			
			inline float3 DecodeLightmap2( float4 color )
			{
			//#if defined (SHADER_API_MOBILE)
				return color * 2.0;
			//#else
				//return pow((_GLOBALBRIGHTNESS+1)* color.rgb, 1 + _GLOBALCONTRASR);
			//#endif
			}

			v2f vert (a2v v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv.zw = TRANSFORM_TEX(v.uv1, _LightMapTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 albedo =  tex2D(_MainTex, i.uv.xy);
				fixed3 lightmap = DecodeLightmap2(tex2D(_LightMapTex, i.uv.zw));
				fixed4 col = albedo * fixed4(lightmap,_BloomPower);
			#ifdef ENABLE_CUSTOM_FOG
				col = GetCustomFog(i.worldPos, col);
			#else
				UNITY_APPLY_FOG(i.fogCoord, col);
			#endif
				return col;
			}
			ENDCG
		}
	}
}
