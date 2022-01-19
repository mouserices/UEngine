Shader "YanJia/Scene/TransparentZwrite"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color Tint", Color) = (1, 1, 1, 1)
		_Alpha("Alpha", Range(0,1)) = 1
		_BloomPower ("Bloom Power", Range(0,1)) = 0.1
		[Toggle(ENABLE_FOG)] _EnableFog ("Enable fog", Float) = 0
		[Toggle(ENABLE_DIFFUSE)]_EnableDiffuse("Enable diffuse", Float) = 0
		[Toggle(ENABLE_RIM)]_EnableRim("Enable Rim", Float) = 0
		_RimPower ("Rim Power", Range(1, 20)) = 30
        _RimColor ("Rim Color", Color) = (1,1,1)
		_RimIntensity ("Rim Intensity", Range(0,3)) = 0.2
		[Space(20)]
		_WindSpeed("Wind Speed", Range(0, 2)) = 0
		_WindPower("Wind Power", Range(0, 2)) = 0
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent"}

		Pass
		{
			ColorMask 0
		}
		
		Pass
		{
			ZWrite Off
			ColorMask RGB
        	Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma multi_compile _ ENABLE_FOG
			#pragma multi_compile _ ENABLE_DIFFUSE
			#pragma multi_compile _ ENABLE_RIM
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
				float3 worldNormal : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
			#ifdef LIGHTMAP_ON
				float2 uvLM : TEXCOORD3;
			#endif
			#ifdef ENABLE_FOG
				UNITY_FOG_COORDS(4)
			#endif
			};

			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _Alpha;
			half _WindSpeed;
			half _WindPower;
			half _RimPower;
			fixed3 _RimColor;
			fixed _RimIntensity;
			
			v2f vert (a2v v)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				v.vertex.x += sin(_Time.z * _WindSpeed) * v.uv.y * _WindPower;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#ifdef ENABLE_FOG
				UNITY_TRANSFER_FOG(o,o.pos);
				#endif
				#ifdef LIGHTMAP_ON
					o.uvLM = v.uvLM.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 albedo =  tex2D(_MainTex, i.uv);
				fixed4 col = albedo;
				#ifdef LIGHTMAP_ON
					col.rgb  *=  DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uvLM.xy));
				#else
					col *= _Color;
				#endif
				col.a *= _Alpha;

				#ifdef ENABLE_DIFFUSE
					fixed3 worldLightDir =  normalize(UnityWorldSpaceLightDir(i.worldPos));
					fixed3 LightColor = _LightColor0.rgb;
					fixed3 worldNormal = normalize(i.worldNormal);
					
					//计算漫反射
					albedo *= _Color;
					fixed3 diffuse = LightColor * albedo.rgb * max(0, dot(worldNormal, worldLightDir));
					#if LIGHTMAP_ON
						col.rgb += diffuse;
					#else
						col.rgb = diffuse;
					#endif

					#ifndef LIGHTMAP_ON
						//计算环境光光照衰减
						fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo.rgb;
						col.rgb = col.rgb + ambient;
					#endif
				#endif

				#ifdef ENABLE_RIM
					fixed3 rim = 0;
					fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
					fixed f =  1.0 - saturate(dot(normalize(viewDir), normalize(i.worldNormal)));
					rim = _RimColor.rgb * pow (f, _RimPower) * _RimIntensity;
					col.rgb += rim;
				#endif

				#ifdef ENABLE_FOG
					UNITY_APPLY_FOG(i.fogCoord, col);
				#endif
				return col;
			}
			ENDCG
		}

		Pass
		{
			ZWrite Off
			COLORMASK A
			Blend One OneMinusSrcAlpha

			CGPROGRAM
			#include "UnityCG.cginc"		
			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
			float4 _MainTex_ST;
			half _BloomPower;
			fixed _Alpha;
			half _WindSpeed;
			half _WindPower;

			struct a2v
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert (a2v v)
			{
				v2f o;
				v.vertex.x += sin(_Time.z * _WindSpeed) * v.uv.y * _WindPower;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed alpha = tex2D(_MainTex, i.uv).a;
				return fixed4(0,0,0,_BloomPower * alpha * _Alpha);
			}

			ENDCG
		}
	}
}
