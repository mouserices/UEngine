Shader "YanJia/Scene/DynamicCloud"
{
	Properties
	{
		_BaseColor ("BaseColor", Color) = (1,1,1,0.5)
        _LightingIndirect("Indirect Lighting", Float) = 1
        _Normalized ("Normalized", Float ) = 1				//1 = true, 2 = false
        _Shading ("Shading Color", Color) = (0, 0, 0, 0.5)
        _DepthIntensity ("Depth Intensity", Range(-1, 1) ) = 0.5
        _PerlinNormalMap ("PerlinNormalMap", 2D) = "white" {}
		_PerlinNormalMapCutoff("NormalMap Cutoff", Range(-1,1)) = 0
		_PerlinNormalMapScale("NormalMap Scale", Range(0, 3)) = 1
        _Tiling ("Tiling", Range(0.1, 10) ) = 1
        _Density ("Density", Range(-1, 1) ) = -0.25
        _Alpha ("Alpha", Range(0, 1) ) = 5
        _TimeMult ("Speed", Float ) = 0.1
        _TimeMultSecondLayer ("SpeedSecondLayer", Float ) = 4
        _WindDirection ("WindDirection", Vector) = (1,0,0,0)
        _CloudNormalsDirection ("_CloudNormalsDirection", Vector) = (1, 1, -1, 0)
		_MaskTex("Mask Texture", 2D) = "white" {}
		//_Distance("Distance", Range(0,2)) = 2 
	}
	SubShader
	{
		Tags {"Queue" = "Transparent-1" "RenderType" = "Transparent"}

		Pass
		{
			Tags { "LightMode" = "ForwardBase" }
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite On

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
            #include "Lighting.cginc"

			struct a2v
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
                float4 vertexColor : COLOR;
                float3 normal : NORMAL;
				half4 tangent : TANGENT;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertexColor: COLOR;
				UNITY_FOG_COORDS(1)
				float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
				float3 tangentDir : TEXCOORD4;
				float3 binormalDir : TEXCOORD5;
				float4 pos : SV_POSITION;
			};

			fixed _TimeMult;
            fixed _TimeMultSecondLayer;
            fixed _Tiling;
            fixed _Normalized;
            fixed _LightingIndirect;
            fixed _Density;
            fixed4 _BaseColor;
            fixed _Alpha;
            fixed _DepthIntensity;
            sampler2D _PerlinNormalMap;
			float4 _PerlinNormalMap_ST;
			fixed _PerlinNormalMapCutoff;
			fixed _PerlinNormalMapScale;
            fixed4 _WindDirection;
			fixed4 _Shading;
			fixed4 _CloudNormalsDirection;
			fixed3 _NormalOffset;
			sampler2D _MaskTex;
			float4 _MaskTex_ST;
			fixed _Distance;
			
			v2f vert (a2v v)
			{
				v2f o;
				o.uv = v.uv;
				o.vertexColor = v.vertexColor;
				o.pos = UnityObjectToClipPos(v.vertex);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.normalDir = mul(half4(v.normal,0), unity_WorldToObject).xyz;
				o.tangentDir = normalize( mul( unity_ObjectToWorld, half4( v.tangent.xyz, 0.0 ) ).xyz );
				o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = 1;
				fixed2 baseAnimation = (_Time.g) * 0.001 * _WindDirection.rb + _PerlinNormalMap_ST.ba;
				fixed2 worldUV = i.uv / (_PerlinNormalMap_ST.rg * _Tiling);
				fixed2 newUV = worldUV + (baseAnimation * _TimeMult);
                fixed2 newUV2 = worldUV + (baseAnimation * _TimeMultSecondLayer) + fixed2(0.0, 0.5);
                fixed4 cloudTexture = tex2D(_PerlinNormalMap, newUV);
                fixed4 cloudTexture2 = tex2D(_PerlinNormalMap, newUV2);
				cloudTexture2.a = saturate(cloudTexture2.a - _PerlinNormalMapCutoff) * _PerlinNormalMapScale;
				//cloudTexture.a = saturate(cloudTexture.a - _PerlinNormalMapCutoff) * _PerlinNormalMapScale;
                fixed baseMorph = ((saturate(cloudTexture.a + _Density) * i.vertexColor.a) - cloudTexture2.a);
                fixed3 baseMorphNormals = ((cloudTexture.rgb *2 - 1) * i.vertexColor.a) - (cloudTexture2.rgb * 2-1);
				fixed cloudMorph = baseMorph * _Alpha;
				cloudMorph = saturate(cloudMorph);
				fixed fakeDepth = saturate(_DepthIntensity  + (i.vertexColor.b * _CloudNormalsDirection.g + 1 ) / 2 );
				fixed3 LightDir = UnityWorldSpaceLightDir(i.posWorld);
				half3x3 tangentTransform = half3x3( i.tangentDir, i.binormalDir, i.normalDir);
				half3 normalLocal = baseMorphNormals * _CloudNormalsDirection.rgb;
				half3 normalDirection =  normalize(mul(normalLocal, tangentTransform));
				fixed NdotL = dot(LightDir, normalDirection);
				NdotL = ( 1 + ( 1- _Normalized ) *0.5) + ( _Normalized  *0.5)*NdotL;
				NdotL = max(0, NdotL);
				fixed shadingRamp = (1.0 - _Shading.a * ( 1.0 - NdotL )) * fakeDepth;
				fixed BaseLightIntensity = max(max(_LightColor0.x, _LightColor0.y), _LightColor0.z);
				fixed diffuse = NdotL;
				fixed3 finalColor = diffuse * _LightingIndirect + lerp( _Shading.rgb, _BaseColor.rgb, shadingRamp) * lerp(_LightColor0.rgb, BaseLightIntensity, _BaseColor.a);
				fixed4 mask = tex2D(_MaskTex, TRANSFORM_TEX(i.uv, _MaskTex));
				//cloudMorph *= _Distance - sqrt((i.uv.x - 0.5) * (i.uv.x - 0.5) + (i.uv.y - 0.5) * (i.uv.y - 0.5)) > 0 ? 1 : 0;
				//cloudMorph *= _Distance - sqrt((i.uv.x - 0.5) * (i.uv.x - 0.5) + (i.uv.y - 0.5) * (i.uv.y - 0.5)) > 0.2 ? 0 : 0;
				col = fixed4(finalColor, cloudMorph * mask.r);
				//col = fixed4(shadingRamp,shadingRamp,shadingRamp,1);
				//UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
