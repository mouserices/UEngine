Shader "YanJia/Scene/CutoutLeaf"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color Tint", Color) = (1, 1, 1, 1)
		//_ShadowColor ("Shadow Color", Color) = (1 ,1 ,1 ,1)
		_MaskTex ("Mask Tex", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
		_BloomPower ("Bloom Power", Range(0,1)) = 0.1
		_LightmapScale("Lightmap Scale", Range(0.1, 3)) = 1
		_RampThreshold ("Ramp Threshold", Range(0,1)) = 0.5
		_RampSmooth ("Ramp Smoothing", Range(0.55,2.5)) = 0.1
		[Space(20)]
		_WindSpeed("Wind Speed", Range(0, 2)) = 0
		_WindPower("Wind Power", Range(0 , 2)) = 0
		[Space(20)]
		_EdgeLitRate ("Edge Light Rate", Range(0,2))= 0.3
		_EdgeCutOff("Edge Mask Cutoff", Range(0,1)) = 0
		_InteriorColor ("Interior Color", Color) = (1,1,1)
        _BackSubsurfaceDistortion ("Back Subsurface Distortion", Range(0,1)) = 0.5
		[Space(20)]
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
			#pragma multi_compile_fwdbase
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma multi_compile _ ENABLE_CUSTOM_FOG
			
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
				float3 worldPos : TEXCOORD2;
				float4 viewSpacePos : TEXCOORD3;
			#ifdef LIGHTMAP_ON
				float2 uvLM : TEXCOORD4;
			#else
				float3 worldNormal : TEXCOORD4;
				float3 worldLightDir : TEXCOORD5;
			#endif
				
			};

			fixed4 _Color;
			//fixed4 _ShadowColor;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _MaskTex;
			fixed _Cutoff;
			half _WindSpeed;
			half _WindPower;
			fixed _LightmapScale;
			fixed _BloomPower;

			half _RampThreshold;
			half _RampSmooth;

			fixed3 _InteriorColor;
			fixed _BackSubsurfaceDistortion;
			fixed _EdgeLitRate;
			fixed _EdgeCutOff;
			
			fixed GetLuminance(fixed3 color)
			{
				return dot(color, fixed3(0.299, 0.587, 0.114));
			}

			v2f vert (a2v v)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				v.vertex.x += sin(_Time.z * _WindSpeed) * v.uv.y * _WindPower; 
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				
				UNITY_TRANSFER_FOG(o,o.pos);
				#ifdef LIGHTMAP_ON
					o.uvLM = v.uvLM.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#else
					o.worldNormal = UnityObjectToWorldNormal(v.normal);
					o.worldLightDir = UnityWorldSpaceLightDir(o.worldPos);
				#endif
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 albedo =  tex2D(_MainTex, i.uv);
				fixed4 col = albedo * _Color;
				

				fixed3 light = 0;

				#ifdef LIGHTMAP_ON
					light =  DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uvLM.xy) / _LightmapScale);
				#else
					fixed3 worldLightDir = i.worldLightDir;
					fixed3 worldNormal = i.worldNormal;
					fixed NdotL = max(0, dot(worldNormal, worldLightDir));
					light = _LightColor0.rgb * NdotL  + UNITY_LIGHTMODEL_AMBIENT.xyz * albedo.rgb;
				#endif

				fixed bright = GetLuminance(light);
				fixed ramp = smoothstep(_RampThreshold - _RampSmooth * 0.5, _RampThreshold + _RampSmooth * 0.5, bright);
				light *=  ramp / bright;
				
				/* 树叶透射
				fixed4 mask = tex2D(_MaskTex, i.uv);
				fixed3 backLitDir = normalize(worldNormal * _BackSubsurfaceDistortion + worldLightDir);
				fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
                fixed backSSS = saturate(dot(viewDir, -backLitDir));
                backSSS = saturate(backSSS * backSSS * backSSS);

				fixed3 edgeCol = backSSS * _EdgeLitRate * _InteriorColor;
                edgeCol += (mask.r - _EdgeCutOff) * backSSS * _InteriorColor;
				col.rgb += edgeCol;
				*/

				col.rgb *= light;

				//fixed maskColor = mask.b;
				//col.rgb = fixed3(maskColor,maskColor,maskColor);

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
