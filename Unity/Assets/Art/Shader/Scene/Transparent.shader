Shader "YanJia/Scene/Transparent"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color Tint", Color) = (1, 1, 1, 1)
		_Alpha("Alpha", Range(0,1)) = 1
		_BloomPower ("Bloom Power", Range(0,1)) = 0.1
		[Toggle(ENABLE_FOG)] _EnableFog ("Enable fog", Float) = 0
		[Toggle(ENABLE_CUSTOM_FOG)] _EnableCustomFog ("Enable Custom fog", Float) = 0
		[Toggle(ENABLE_DIFFUSE)]_EnableDiffuse("Enable diffuse", Float) = 0
		[Space(20)]
		_WindSpeed("Wind Speed", Range(0, 2)) = 0
		_WindPower("Wind Power", Range(0, 2)) = 0

		[Space(20)]
		[Toggle(ENABLE_MATCAP)]_EnableMatCap("Enable MatCap", Float) = 0
		_EnvTex ("Cube env tex", CUBE) = "black" {}
		_Spread("Spread", Range (0.01,1.0)) = 0.5

		[Space(20)]
		[Toggle(ENABLE_FADE_HEIGHT)]_EnableFadeHeight("Enable Fade Height", Float) = 0
		_FadeHeightStart("Fade Height Start", float) = 1
		_FadeHeightEnd("Fade Height End", float) = 2

		[Space(20)]
		_Brightness("Brightness", Range(0,2)) = 1
		_Contrast("Contrast", Range(0, 2)) = 1
		_Saturation("Saturation", Range(0,2)) = 1
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent"}


		ZWrite Off
		Pass
		{
			Tags{"LightMode"="ForwardBase"}
			ColorMask RGB
        	Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma multi_compile _ ENABLE_FOG
			#pragma multi_compile _ ENABLE_DIFFUSE
			#pragma multi_compile _ ENABLE_CUSTOM_FOG
			#pragma multi_compile _ ENABLE_MATCAP
			#pragma multi_compile _ ENABLE_FADE_HEIGHT
			#pragma multi_compile_fwdbase
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			#include "../CustomShader.cginc"

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
				float4 viewSpacePos : TEXCOORD3;
			#ifdef LIGHTMAP_ON
				float2 uvLM : TEXCOORD4;
			#endif
			#ifdef ENABLE_FOG
				UNITY_FOG_COORDS(5)
			#endif
			#ifdef ENABLE_MATCAP
				float3 reflect : TEXCOORD6;
			#endif
			};

			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _Alpha;
			half _WindSpeed;
			half _WindPower;
			samplerCUBE _EnvTex;
			half _Spread;
			float _FadeHeightStart;
			float _FadeHeightEnd;

			half _Brightness;
			half _Contrast;
			half _Saturation;
			
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
				#ifdef ENABLE_MATCAP
					fixed3 viewDir = UnityWorldSpaceViewDir(o.worldPos);
					o.reflect = reflect(-viewDir,o.worldNormal);
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

				//计算金属反光
				#ifdef ENABLE_MATCAP
				col.xyz += texCUBE(_EnvTex,i.reflect) * _Spread;
				#endif

				col.rgb *= _Brightness;
				half gray = 0.2125 * col.r + 0.7154 * col.g + 0.0721 * col.b;
				half3 grayColor = fixed3(gray, gray, gray);
				col.rgb = lerp(grayColor, col.rgb, _Saturation);
				half3 avgColor = half3(0.5, 0.5, 0.5);
				col.rgb = lerp(avgColor, col.rgb, _Contrast);

				#ifdef ENABLE_FOG
					UNITY_APPLY_FOG(i.fogCoord, col);
				#elif ENABLE_CUSTOM_FOG
					col = GetCustomFog(i.worldPos, col);
				#endif

				#ifdef ENABLE_FADE_HEIGHT
					col.a = lerp(1, 0, (i.worldPos.y - _FadeHeightStart) /  (_FadeHeightEnd - _FadeHeightStart));
					//half temp = (i.worldPos.y - _FadeHeightStart) /  (_FadeHeightEnd - _FadeHeightStart);
					//col.rgb = half3(temp,temp,temp);
				#endif
				return col;
			}
			ENDCG
		}

		Pass
		{
			COLORMASK A
			BlendOp max

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
