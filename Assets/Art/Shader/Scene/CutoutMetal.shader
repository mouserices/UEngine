Shader "YanJia/Scene/CutoutMetal"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color Tint", Color) = (1, 1, 1, 1)
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
		_MetalTex("Metal Tex", 2D) = "black" {}
		_EnvTex ("Cube env tex", CUBE) = "black" {}
		_Spread("Spread", Range (0,1.0)) = 0.5
		_BloomPower ("Bloom Power", Range(0,1)) = 0.1
	}
	SubShader
	{
		Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}

		Pass
		{
			CGPROGRAM
			#pragma multi_compile _ ENABLE_DIFFUSE
			#pragma multi_compile_fwdbase
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			struct a2v
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			#ifdef LIGHTMAP_ON
				float2 uvLM : TEXCOORD1;
			#endif
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float3 worldNormal : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
				float3 reflect : TEXCOORD4;
			#ifdef LIGHTMAP_ON
				float2 uvLM : TEXCOORD5;
			#endif
			};

			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _Cutoff;
			sampler2D _MetalTex;
			samplerCUBE _EnvTex;
			float _Spread;
			fixed _BloomPower;
			
			v2f vert (a2v v)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				UNITY_TRANSFER_FOG(o,o.pos);
				fixed3 viewDir = UnityWorldSpaceViewDir(o.worldPos);
				o.reflect = reflect(-viewDir,o.worldNormal);
				#ifdef LIGHTMAP_ON
					o.uvLM = v.uvLM.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 albedo =  tex2D(_MainTex, i.uv);
				fixed4 col = albedo;
				fixed metallic = tex2D(_MetalTex, i.uv.xy).r;

				#ifdef LIGHTMAP_ON
					col.rgb *=  albedo.rgb * DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uvLM.xy));
				#else
					col *= _Color;
				#endif

				col.rgb += texCUBE(_EnvTex,i.reflect) * _Spread * metallic;

				clip(col.a - _Cutoff);
				col.a = (albedo.a - _Cutoff) * _BloomPower;
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}


		Pass
        {
             Name "ShadowCaster"
             Tags { "LightMode" = "ShadowCaster" }

             ZTest LEqual
             ZWrite On

             CGPROGRAM

             #pragma skip_variants SHADOWS_SOFT
             #pragma multi_compile_shadowcaster
             #pragma vertex vert   
             #pragma fragment frag

			 sampler2D _MainTex;
			 float4 _MainTex_ST;
			 float _Cutoff;
            
			 #include "UnityCG.cginc"
			 struct v2f
			 {
			     V2F_SHADOW_CASTER;
			     float2 uv : TEXCOORD1;
			 };

			 v2f vert(appdata_base v)
			 {
			     v2f o;
			 	 UNITY_INITIALIZE_OUTPUT(v2f, o);
				 o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
			     TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
			     return o;
			 }

			 fixed4 frag(v2f i) : SV_Target
			 {
			 	fixed4 albedo =  tex2D(_MainTex, i.uv);
				clip (albedo.a - _Cutoff);
				SHADOW_CASTER_FRAGMENT(i);
			 }
           
             ENDCG
         }
	}
}
