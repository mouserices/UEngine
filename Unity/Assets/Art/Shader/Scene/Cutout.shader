Shader "YanJia/Scene/Cutout"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color Tint", Color) = (1, 1, 1, 1)
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
		_BloomPower ("Bloom Power", Range(0,1)) = 0.1
		_LightmapScale("Lightmap Scale", Range(0.1, 3)) = 1
		[Toggle(ENABLE_DIFFUSE)]_EnableDiffuse("Enable diffuse", Float) = 0
		[Space(20)]
		_WindSpeed("Wind Speed", Range(0, 2)) = 0
		_WindPower("Wind Power", Range(0 , 2)) = 0
		[Toggle(ENABLE_CUSTOM_FOG)] _EnableCustomFog ("Enable Custom fog", Float) = 0
	}
	SubShader
	{
		Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
		Cull Off

		Pass
		{
			Tags {"LightMode"="ForwardBase"}
			
			CGPROGRAM
			#pragma multi_compile _ ENABLE_DIFFUSE
			#pragma multi_compile_fwdbase
			#pragma multi_compile _ ENABLE_CUSTOM_FOG
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
				UNITY_FOG_COORDS(1)
				float3 worldNormal : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
			#ifdef LIGHTMAP_ON
				float2 uvLM : TEXCOORD4;
			#endif
			};

			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _Cutoff;
			half _WindSpeed;
			half _WindPower;
			fixed _LightmapScale;
			fixed _BloomPower;
			
			v2f vert (a2v v)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				v.vertex.x += sin(_Time.z * _WindSpeed) * v.uv.y * _WindPower; 
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				UNITY_TRANSFER_FOG(o,o.pos);
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
					col.rgb  *=  DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uvLM.xy) / _LightmapScale);
				#else
					col *= _Color;
				#endif

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

				clip(col.a - _Cutoff);
				col.a = (albedo.a - _Cutoff) * _BloomPower;

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
