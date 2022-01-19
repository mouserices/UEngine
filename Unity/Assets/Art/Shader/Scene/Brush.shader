Shader "YanJia/Scene/Brush"
{
	Properties
	{
		_Color ("Color", Color) = (1, 1, 1)
		_MainTex ("Albedo", 2D) = "white" {}
		//_CutOff("CutOff", Range(0,0.1)) = 0.06
		_BloomPower ("Bloom Power", Range(0,1)) = 0.1
		_LightmapScale("Lightmap Scale", Range(0.1, 3)) = 1.3
		_WindSpeed("Wind Speed", Range(0, 1)) = 0
		_WindPower("Wind Power", Range(0 , 1)) = 0
		[Toggle(ENABLE_CUSTOM_FOG)] _EnableCustomFog ("Enable Custom fog", Float) = 0
	}

	SubShader
	{		
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Brush"}

		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off 
		ZWrite Off

		Pass
		{ 
			Tags { "LightMode"="ForwardBase" }

			CGPROGRAM
			
			#pragma multi_compile_fwdbase
			#pragma multi_compile_fog
			#pragma multi_compile _ ENABLE_CUSTOM_FOG
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "../CustomShader.cginc"

			
			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _BloomPower;
			fixed _LightmapScale;
			fixed _WindSpeed;
			fixed _WindPower;
			//fixed _CutOff;
			
			struct a2v
			{
				float4 vertex : POSITION;
				fixed4 vertexColor : COLOR;
				float4 uv : TEXCOORD0;
			#ifdef LIGHTMAP_ON
				float2 uvLM : TEXCOORD1;
			#endif
			};
			
			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				fixed3 vertexColor : COLOR;
				UNITY_FOG_COORDS(1)
			#ifdef LIGHTMAP_ON
				float2 uvLM : TEXCOORD2;
			#endif
				float3 worldPos : TEXCOORD3;
			};
			
			v2f vert(a2v v) 
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				v.vertex.x += sin(_Time.z * _WindSpeed) * o.uv.y * _WindPower; //添加随风摇动

				
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.pos = UnityObjectToClipPos(v.vertex);

				o.vertexColor = v.vertexColor;//支持特效系统控制颜色
				UNITY_TRANSFER_FOG(o, o.pos);
				#ifdef LIGHTMAP_ON
					o.uvLM = v.uvLM.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb *= _Color;
				//col.a *= 0.1;
				
				//clip(col.a - _CutOff);

				#ifdef LIGHTMAP_ON
					col.rgb  *=  DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uvLM.xy) / _LightmapScale);
				#else
					col.rgb *= i.vertexColor * _Color;
				#endif

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
			Name "Meta"
			Tags { "LightMode" = "Meta" }

			Cull Back

			CGPROGRAM

			#pragma vertex vert_meta
			#pragma fragment frag_meta

			#include "Lighting.cginc"
			#include "UnityMetaPass.cginc"

			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			struct v2f
			{
				float4 pos:SV_POSITION;
				float2 uv:TEXCOORD1;
				float3 worldPos:TEXCOORD0;
			};

			v2f vert_meta(appdata_full v)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f,o);

				o.pos = UnityMetaVertexPosition(v.vertex,v.texcoord1.xy,v.texcoord2.xy,unity_LightmapST,unity_DynamicLightmapST);
				o.uv = v.texcoord.xy;
				return o;
			}

			fixed4 frag_meta(v2f IN) :SV_Target
			{
				fixed4 albedo = tex2D(_MainTex,IN.uv) * _Color;
				UnityMetaInput metaIN;
				UNITY_INITIALIZE_OUTPUT(UnityMetaInput,metaIN);
				metaIN.Albedo = albedo.rgb;
				metaIN.Emission = 0;
				return UnityMetaFragment(metaIN);
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
			 //float _Cutoff;
            
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
			 	//fixed4 albedo =  tex2D(_MainTex, i.uv);
				//clip (-_Cutoff);
				SHADOW_CASTER_FRAGMENT(i);
			 }
           
             ENDCG
        }
	}
}

