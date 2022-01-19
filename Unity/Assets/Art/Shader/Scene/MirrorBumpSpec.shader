Shader "YanJia/Scene/MirrorsBumpedSpecular" 
{
	Properties 
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
		_Gloss("_Gloss", Float) = 1
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_BumpScaleX ("Bump X",Range(0,1))= 0
		_BumpScaleY ("Bump Y",Range(0,1))= 0
		_Ref ("For Mirror reflection,don't set it!", 2D) = "white" {}
		_RefColor("Reflection Color",Color) = (1,1,1,1)
		_RefRate ("Reflective Rate", Range (0, 1)) = 1
		//_Roughness("Roughess", Range(0,7)) = 0
		_BloomPower("Bloom Power", Range(0,1)) = 0.1
		[Toggle(ENABLE_UI)]_EnableUI("Enable UI", Float) = 0
		[Toggle(ENABLE_CUSTOM_FOG)] _EnableCustomFog ("Enable Custom fog", Float) = 0
	}

	SubShader 
	{ 
		pass
		{
			Tags { "RenderType"="Opaque" }
			
			CGPROGRAM

			#pragma multi_compile_fwdbase
			#pragma multi_compile _ ENABLE_UI 
			#pragma multi_compile _ ENABLE_REF
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#include "../CustomShader.cginc"
			#pragma multi_compile _ ENABLE_CUSTOM_FOG

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BumpMap;
			float4 _BumpMap_ST;
			fixed4 _Color;
			fixed _Gloss;
			fixed _RefRate;
			fixed _BumpScaleX;
			fixed _BumpScaleY;
			fixed4 _RefColor;
			sampler2D _Ref;
			//half _Roughness;
			fixed _BloomPower;

			struct a2v
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float2 uvLM : TEXCOORD1;
			};
						
			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 reflect : TEXCOORD3;
				UNITY_SHADOW_COORDS(4)
				UNITY_FOG_COORDS(5)
				float4 screenUV : TEXCOORD6;
				float4 uvLM : TEXCOORD7;
			};
						
			v2f vert(a2v v) 
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				fixed3 viewDir = UnityWorldSpaceViewDir(o.worldPos);
				o.reflect = reflect(-viewDir,o.worldNormal);
				o.screenUV = ComputeScreenPos(o.pos);
				o.uvLM.wz = TRANSFORM_TEX(v.uv, _BumpMap);

				TRANSFER_SHADOW(o);
				UNITY_TRANSFER_FOG(o, o.pos);

			#ifdef LIGHTMAP_ON
				o.uvLM.xy = v.uvLM.xy * unity_LightmapST.xy + unity_LightmapST.zw;
			#endif
				return o;
			}
						
			fixed4 frag(v2f i) : SV_Target
			{		
				fixed4 albedo = tex2D(_MainTex, i.uv.xy);
				fixed4 col = 1;
				half2 screenUV = 0;
				
		#ifdef ENABLE_UI
			col.rgb = albedo;
			#ifdef ENABLE_REF
			screenUV = i.screenUV.xy / i.screenUV.w;
			fixed3 ref = tex2D(_Ref, screenUV).rgb;
			col.rgb += ref * ref * 0.6;
			#endif
		#else
			fixed3 worldNormal = normalize(i.worldNormal);
			#if LIGHTMAP_ON
				col.rgb = albedo.rgb * DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uvLM.xy));
			#else

				fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
				fixed3 LightColor = _LightColor0.rgb;

				albedo *= _Color;
				fixed3 diffuse = LightColor * albedo.rgb * max(0, dot(worldNormal, worldLightDir));
				col.rgb = diffuse;

				fixed3 viewDir;
				viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
				fixed3 halfDir = normalize(worldLightDir + viewDir);
				fixed3 specular = LightColor * pow(max(0, dot(worldNormal, halfDir)), _Gloss);
				col.rgb += specular;

				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo.rgb;
				UNITY_LIGHT_ATTENUATION(atten, i, i.worldPos);
				col.rgb = col.rgb * atten + ambient;

			#endif

				fixed2 BumpNormaTex = tex2D(_BumpMap, i.uvLM.wz).rg;
				half2 BukmpNormal = (BumpNormaTex * 2.0 - 1.0) *  half2(_BumpScaleX, _BumpScaleY);
				#ifdef ENABLE_REF
					screenUV = i.screenUV.xy / i.screenUV.w + BukmpNormal;
					fixed3 ref = tex2D(_Ref, screenUV).rgb;
					col.rgb += ref * ref * _RefRate;
				#endif
		#endif



				//half floorRoughness = floor(_Roughness);
				//half ceilRoughness = ceil(_Roughness);
				//fixed4 floorRef = tex2Dlod(_Ref, float4(screenUV, 0, floorRoughness));
				//fixed4 ceilRef = tex2Dlod(_Ref, float4(screenUV, 0, ceilRoughness));
				//fixed4 ref = lerp(floorRef, ceilRef, _Roughness - floorRoughness);
				//col.rgb += ref.rgb * ref.rgb * _RefRate;

				col.a = albedo.a * _BloomPower;
			#ifdef ENABLE_CUSTOM_FOG
				col = GetCustomFog(i.worldPos, col);
			#else
				UNITY_APPLY_FOG(i.fogCoord, col);
			#endif
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
            
			#define OBJECT_PASS_SHADOWCASTER
			#include "../CustomShader.cginc"
           
            ENDCG
        }
	}
	Fallback Off
}
