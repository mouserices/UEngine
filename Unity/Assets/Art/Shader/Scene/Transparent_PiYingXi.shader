Shader "YanJia/Scene/Transparent_PiYingXi"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color Tint", Color) = (1, 1, 1, 1)
		_Alpha("_Alpha", Range(0,1)) = 1
		_Edge ("Edge", Range(0, 0.5)) = 0.003
	}

	SubShader
	{		
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}

		// Pass
		// {
		// 	Fog {Mode Off}
		// 	ZWrite On
		// 	ColorMask 0
		// }

		Pass
		{ 
			//Tags { "LightMode"="ForwardBase" }

			Blend SrcAlpha OneMinusSrcAlpha
			//Lighting Off 
			//ZWrite Off
			Fog {Mode Off}

			CGPROGRAM
			
			#pragma multi_compile_fwdbase
			//#pragma multi_compile_fog
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			 #include "UnityLightingCommon.cginc"
			//#include "CurvedWorld_Base.cginc"
			
			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _Alpha;
			float _Edge;
			
			struct a2v
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
			};
			
			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normalDir : TEXCOORD2;
				//UNITY_FOG_COORDS(1)
			};
			
			v2f vert(a2v v) 
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				 o.normalDir = UnityObjectToWorldNormal(v.normal);
				//UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				if(col.r <= 0.5)
					//return fixed4(1,1,1,1);
                	clip(col.a + col.r - _Edge - 0.5);
				else
					clip(col.a - _Edge);
					//return fixed4(0,0,0,1);

				col.a *= _Color * _Alpha;
				//i.normalDir = normalize(i.normalDir);
				//float3 normalDirection = i.normalDir;
////// Lighting:
                //float attenuation = LIGHT_ATTENUATION(i);
                //float3 attenColor = attenuation * _LightColor0.xyz;
				//float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
/////// Diffuse
                //float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                //float3 directDiffuse = max( 0.0, NdotL);
                //float3 indirectDiffuse = float3(0,0,0);
                //indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                //float3 diffuseColor = (col.rgb*2.0);
                //float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
                //float3 diffuse = diffuseColor;
/// Final Color:
                //float3 finalColor = diffuse;
				//col.rgb = finalColor;

				//UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0,0,0,0));
				return col;
			}
			ENDCG
		}
	}
	FallBack off
}

