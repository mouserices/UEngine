Shader "YanJia/Scene/WaterReflect"
{
	Properties
	{
		[Space(20)]
		_BumpTex ("Bump Texture", 2D) = "white"{}
		_BumpStrength ("Bump strength", Range(0.0, 10.0)) = 1.0
		_BumpDirection ("Bump direction(2 wave)", Vector)=(1,1,1,-1)
		_BumpTiling ("Bump tiling", Vector)=(0.0625,0.0625,0.0625,0.0625)

		[Space(20)]
		_RefractTex ("Refract Texture", 2D) = "white" {}
		_RefractColor("Refract Color", Color) = (1,1,1,1)
		_RefractOffSet("Refract Offset", Range(0,1)) = 0.025

		[Space(20)]
		_AlphaTex ("Alpha Map Texture", 2D) = "white"{}
		_Alpha ("Aplha", Range(0, 1)) = 1

		[Space(20)]
		_Color("Main Color", Color) = (1,1,1)
		_SkyColor("Sky Color", Color) = (1,1,1)
		_Specular("Specular Color", Color) = (1,1,1)
		_SpecularPower("_SpecularPower", Float) = 1
		_Gloss("_Gloss", Float) = 1
		_Fresnel("fresnel", Float) = 0.02

		[Space(20)]
		_ReflectColor("Reflect Color", Color) = (1,1,1,1)
		_ReflectOffSet("Reflect Offset", Float) = 0.05

		[Space(20)]
		_Ref("Don't Set Ref Texture", 2D) = "white" {}
		[Toggle(ENABLE_REF)] _EnableRef ("Enable Reflect Runtime", Float) = 1

		[Space(20)]
		_BloomPower ("Bloom Power", Range(0,1)) = 0.1

		[Space(20)]
		[Toggle(ENABLE_FOG)] _EnableFog ("Enable fog", Float) = 0
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			offset 1,1
			CGPROGRAM
			#pragma multi_compile _ ENABLE_FOG
			#pragma multi_compile _ ENABLE_REF
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			#pragma multi_compile_fog
			#include "UnityCG.cginc"		

			struct a2v
			{
				float4 vertex : POSITION;
				float2 uv:TEXCOORD0;
			};

			struct v2f
			{
				float4 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
				float4 bumpCoords:TEXCOORD1;
				float3 viewVector:TEXCOORD2;
				float4 screenUV : TEXCOORD3;
			#ifdef ENABLE_FOG	
				UNITY_FOG_COORDS(4)
			#endif	
			};

			fixed3 _Color;
			fixed4 _RefractColor;
			sampler2D _Ref;
			sampler2D _BumpTex;
			sampler2D _AlphaTex;
			sampler2D _RefractTex;
			float4 _RefractTex_ST;
			fixed _Alpha;
			half _BumpStrength;
			half4 _BumpDirection;
			half4 _BumpTiling;
			fixed3 _SkyColor;
			fixed3 _Specular;
			half _SpecularPower;
			half _Gloss;
			half _Fresnel;
			half _RefractOffSet;
			fixed4 _ReflectColor;
			half _ReflectOffSet;
			half _BloomPower;
			
			half3 PerPixelNormal(sampler2D bumpMap, half4 coords, half bumpStrength) 
			{
				float2 bump = (UnpackNormal(tex2D(bumpMap, coords.xy)) + UnpackNormal(tex2D(bumpMap, coords.zw))) * 0.5;
				float3 worldNormal = float3(0,0,0);
				worldNormal.xz = bump.xy * bumpStrength;
				worldNormal.y = 1;
				return worldNormal;
			}
			
			inline float FastFresnel(half3 I, half3 N, half R0)
			{
				float icosIN = saturate(1-dot(I, N));
				float i2 = icosIN*icosIN;
				float i4 = i2*i2;
				return R0 + (1-R0)*(i4*icosIN);
			}

			v2f vert (a2v v)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);

				o.pos = UnityObjectToClipPos(v.vertex);
				half3 worldPos = mul(unity_ObjectToWorld, v.vertex);

				o.uv.xy = v.uv;
				o.uv.zw = TRANSFORM_TEX(v.uv, _RefractTex);
				o.screenUV = ComputeScreenPos(o.pos);
				o.bumpCoords.xyzw = (worldPos.xzxz + _Time.yyyy * _BumpDirection.xyzw) * _BumpTiling.xyzw;
				o.viewVector = (worldPos - _WorldSpaceCameraPos.xyz);

			#ifdef ENABLE_FOG
				UNITY_TRANSFER_FOG(o,o.pos);
			#endif
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = fixed4(0,0,0,1);
				
				half3 worldNormal = normalize(PerPixelNormal(_BumpTex, i.bumpCoords, _BumpStrength));
				half3 viewVector = normalize(i.viewVector);
				fixed3 worldLightDir = fixed3(-0.430, 0.766, -0.478);
				half3 halfVector = normalize(normalize(worldLightDir) - viewVector);
				half2 screenUV = i.screenUV.xy/i.screenUV.w;	

				half2 offsets = worldNormal.xz * viewVector.y;
				//fixed3 refractColor = tex2D(_RefractTex, i.uv.zw + offsets * _RefractOffSet).rgb * _Color;

				#if (ENABLE_REF)
					fixed4 reflectColor = tex2D(_Ref, screenUV + offsets * _ReflectOffSet) * _ReflectColor;
				#else
					fixed4 reflectColor = tex2D(_RefractTex, screenUV + offsets * _ReflectOffSet) * _RefractColor;
				#endif

				half3 reflUV = reflect(viewVector, worldNormal);
				//reflectColor.rgb = lerp(_SkyColor, reflectColor, reflectColor.a);
				half fresnel = FastFresnel(-viewVector, worldNormal, _Fresnel);

				col.rgb = reflectColor;
				col.rgb += _Specular.rgb * _SpecularPower * pow(max(0, dot(worldNormal, halfVector)), _Gloss);
				
				fixed3 alpha = tex2D(_AlphaTex, i.uv.xy);
				col.a = alpha * _Alpha;

			#ifdef ENABLE_FOG
				UNITY_APPLY_FOG(i.fogCoord, col);
			#endif

				return col;
			}
			ENDCG
		}

		Pass
		{
			COLORMASK A
			Blend One Zero

			CGPROGRAM
			#include "UnityCG.cginc"		
			#pragma vertex vert
			#pragma fragment fragAlpha

			half _BloomPower;

			float4 vert (appdata_base v) : SV_POSITION
			{
				return UnityObjectToClipPos(v.vertex);
			}

			fixed4 fragAlpha () : SV_Target
			{
				return fixed4(0,0,0,_BloomPower);
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
}
