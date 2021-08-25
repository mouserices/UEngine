Shader "YanJia/Scene/Liquid"
{
	Properties
	{
		_TopColor ("Top Color", Color) = (1,1,1,1)
		_Color (" Color", Color) = (1,1,1,0.9)
		_MainTex ("Texture", 2D) = "white" {}

		[Space(20)]
		_FillAmount ("Fill Amount", Range(-10,10)) = 0.0
		_FoamColor ("Foam Line Color", Color) = (1,1,1,1)
		_FoamHeight("Foam Height", Range(0,0.5)) = 0.03
		_WobbleSpeed("Foam Wobble Speed", Range(0, 10)) = 1
		_WobblePower("Foam Wobble Power", Range(0,1)) = 0.1

		[Space(20)]
        _RimExp ("Rim Exp", Range(0, 10)) = 10
        _RimColor ("Rim Color", Color) = (1,1,1)
		_RimPower ("Rim Power", Range(0,3)) = 0.2

		[Space(20)]
		_RimAlphaExp ("Alpha Exp", Range(0, 10)) = 10
		_RimAlpha("Rim Alpha", Range(0,1)) = 1
		_Alpha("Alpha", Range(0,1)) = 0.5
		_BloomPower ("Bloom Power", Range(0,1)) = 0.1
	}
	SubShader
	{
		Tags{"RenderType"="Transparent" "Queue"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha 
		ZWrite OFF		//Cull Off
		COLORMASK RGB
		//AlphaToMask On

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			half4 _Color;
			float4 _MainTex_ST;
        	sampler2D _MainTex;
			half _Alpha;
			half _RimExp;
			half3 _RimColor;
			half _RimPower;
			half _RimAlpha;
			half _RimAlphaExp;
			half _FoamHeight;
			half _WobbleSpeed;
			half _WobblePower;
			half _FillAmount;
			half4 _TopColor;
			half4 _FoamColor;

			float4 RotateAroundYInDegrees (float4 vertex, float degrees)
			{
				float alpha = degrees * UNITY_PI / 180;
				float sina, cosa;
				sincos(alpha, sina, cosa);
				float2x2 m = float2x2(cosa, sina, -sina, cosa);
				return float4(vertex.yz , mul(m, vertex.xz)).xzyw ;				
			}

			struct a2v
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;	
			};
	
			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float3 viewDir : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;		
				float fillEdge : TEXCOORD4;
			};
			
			v2f vert (a2v v)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,v.vertex);			
				half3 worldPos = mul (unity_ObjectToWorld, v.vertex.xyz);   
				half3 worldPosX= RotateAroundYInDegrees(float4(worldPos,0),360);
				half3 worldPosZ = float3 (worldPosX.y, worldPosX.z, worldPosX.x);
				half wobbleX = sin(_Time.z * _WobbleSpeed) * _WobblePower;
				half wobbleY = cos(_Time.z * _WobbleSpeed) * _WobblePower;
				half3 worldPosAdjusted = worldPos + (worldPosX  * wobbleX)+ (worldPosZ * wobbleY); 
				o.fillEdge =  worldPosAdjusted.y + _FillAmount;
				o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				return o;
			}
			
			half4 frag (v2f i) : SV_Target
			{
				half4 col = 0;
				half4 mainTex = tex2D(_MainTex, i.uv);
				half3 baseColor = lerp(_Color.rgb, _TopColor.rgb, i.fillEdge) * mainTex.rgb;

				half3 rimColor = 0;
				half3 viewDir = normalize(i.viewDir);
				half3 worldNormal = normalize(i.worldNormal);
				half f =  1.0 - saturate(dot(viewDir, worldNormal));
				half rim =  pow (f, _RimExp);
				rimColor = _RimColor.rgb * rim * _RimPower;

				half fillEdge = step(i.fillEdge, 0.5);
				clip(fillEdge- 0.01);
				float foam = fillEdge - step(i.fillEdge, (0.5 - _FoamHeight));
				col.rgb =  baseColor + foam * _FoamColor.rgb + rimColor;

				half alphaRim = (1 - pow (f, _RimAlphaExp) / 2) * _RimAlpha;
				half alpha = min(lerp(_Color.a, _TopColor.a, i.fillEdge), alphaRim);
				col.a = alpha;
				//col.rgb = fixed3(alphaRim,alphaRim,alphaRim);
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}

		Pass
		{
			Blend One OneMinusSrcAlpha
			COLORMASK A

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			half _BloomPower;

			half4 vert (float4 v : POSITION) : SV_POSITION
			{
				return UnityObjectToClipPos(v);
			}

			half4 frag(): SV_Target
			{
				return half4(0,0,0, _BloomPower);
			}
			
			ENDCG
		}
	}

	Fallback Off
}
