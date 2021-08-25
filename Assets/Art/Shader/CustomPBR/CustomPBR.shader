Shader "YanJia/PBR/CustomPBR"
{
	Properties
	{
		[Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull Mode", Float) = 2
		_Color("Color",color) = (1,1,1,1)
		_MainTex("Albedo",2D) = "white"{}
		_MaterialMap ("Material Map", 2D) = "gray" {} //(R:AO, G:Roughenss, B:Metallic)
		_BumpMap("Normal Map",2D) = "bump"{}
		_RoughnessMapScale ("Roughess Scale", Range(0, 2)) = 1
		_MetallicMapScale ("Metallic Scale",Range(0, 2)) = 1
		_BumpScale("Normal Scale",Range(0, 2)) = 1
		_Brightness("Brightness", Range(0,2)) = 1
		_Contrast("Contrast", Range(0, 2)) = 1
		_Saturation("Saturation", Range(0,2)) = 1
        _LightmapScale("Lightmap Scale", Range(0.1, 5)) = 1
		_BloomPower ("Bloom Power", Range(0,1)) = 0.1
		[Toggle(ENABLE_UE4)]_ISUE4Resource("Is UE4 Resource", Float) = 0
		[Toggle(ENABLE_CUSTOM_FOG)] _EnableCustomFog ("Enable Custom fog", Float) = 0

		[Space(20)]
		[Toggle(ENABLE_MATCAP)]_EnableMatCap("Enable MatCap", Float) = 0
		_EnvTex ("Cube env tex", CUBE) = "black" {}
		_Spread("Spread", Range (0,1.0)) = 0.5
		
		[Toggle(ENABLE_ADD_COLOR)] _EnableAddColor ("Enable Add Color", Float) = 0
		_AddColor ("AddColor", Color) = (0.5,0.5,0.5,1)
		_AddColorIntensity("AddColorIntensity", Range (0,1.0)) = 0
	}

	CGINCLUDE

		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#include "AutoLight.cginc"

		inline half4 VertexGI(float2 uv1 ,float3 worldPos,float3 worldNormal)
		{
			half4 ambientOrLightmapUV = 0;

			#ifdef LIGHTMAP_ON
				ambientOrLightmapUV.xy = uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
			#elif UNITY_SHOULD_SAMPLE_SH
				#ifdef VERTEXLIGHT_ON
					ambientOrLightmapUV.rgb = Shade4PointLights(
						unity_4LightPosX0,unity_4LightPosY0,unity_4LightPosZ0,
						unity_LightColor[0].rgb,unity_LightColor[1].rgb,unity_LightColor[2].rgb,unity_LightColor[3].rgb,
						unity_4LightAtten0,worldPos,worldNormal);
				#endif
				ambientOrLightmapUV.rgb += ShadeSH9(half4(worldNormal,1));
			#endif

			return ambientOrLightmapUV;
		}


		inline half3 ComputeIndirectDiffuse(half4 ambientOrLightmapUV,half occlusion)
		{
			half3 indirectDiffuse = 0;

			#if UNITY_SHOULD_SAMPLE_SH
				indirectDiffuse = ambientOrLightmapUV.rgb;	
			#endif

			#ifdef LIGHTMAP_ON
				indirectDiffuse = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap,ambientOrLightmapUV.xy));
			#endif

			return indirectDiffuse * occlusion;
		}


		inline half3 BoxProjectedDirection(half3 worldRefDir,float3 worldPos,float4 cubemapCenter,float4 boxMin,float4 boxMax)
		{
			half3 rbmax = (boxMax.xyz - worldPos) / worldRefDir;
			half3 rbmin = (boxMin.xyz - worldPos) / worldRefDir;

			half3 rbminmax = (worldRefDir > 0.0f) ? rbmax : rbmin;

			half fa = min(min(rbminmax.x,rbminmax.y),rbminmax.z);

			worldPos -= cubemapCenter.xyz;
			worldRefDir = worldPos + worldRefDir * fa;

			return worldRefDir;
		}


		inline half3 SamplerReflectProbe(UNITY_ARGS_TEXCUBE(tex),half3 refDir,half roughness,half4 hdr)
		{
			roughness = roughness * (1.7 - 0.7 * roughness);
			half mip = roughness * 6;
			half4 rgbm = UNITY_SAMPLE_TEXCUBE_LOD(tex,refDir,mip);
			return DecodeHDR(rgbm,hdr);
		}

		inline half3 ComputeIndirectSpecular(half3 refDir,float3 worldPos,half roughness,half occlusion)
		{
			half3 specular = 0;
			half3 refDir1 = BoxProjectedDirection(refDir,worldPos,unity_SpecCube0_ProbePosition,unity_SpecCube0_BoxMin,unity_SpecCube0_BoxMax);
			half3 ref1 = SamplerReflectProbe(UNITY_PASS_TEXCUBE(unity_SpecCube0),refDir1,roughness,unity_SpecCube0_HDR);
			specular = ref1;
			
			return specular * occlusion;
		}

		inline half ComputeSmithJointGGXVisibilityTerm(half nl,half nv,half roughness)
		{
			half ag = roughness * roughness;
			half lambdaV = nl * (nv * (1 - ag) + ag);
			half lambdaL = nv * (nl * (1 - ag) + ag);
			
			return 0.5f/(lambdaV + lambdaL + 1e-5f);
		}

		inline half ComputeGGXTerm(half nh,half roughness)
		{
			half a = roughness * roughness;
			half a2 = a * a;
			half d = (a2 - 1.0f) * nh * nh + 1.0f;
			return a2 * UNITY_INV_PI / (d * d + 1e-5f);
		}

		inline half3 ComputeFresnelTerm(half3 F0,half cosA)
		{
			return F0 + (1 - F0) * pow(1 - cosA, 5);
		}

		inline half3 ComputeDisneyDiffuseTerm(half nv,half nl,half lh,half roughness,half3 baseColor)
		{
			half Fd90 = 0.5f + 2 * roughness * lh * lh;
			return baseColor * UNITY_INV_PI * (1 + (Fd90 - 1) * pow(1-nl,5)) * (1 + (Fd90 - 1) * pow(1-nv,5));
		}

		inline half3 ComputeFresnelLerp(half3 c0,half3 c1,half cosA)
		{
			half t = pow(1 - cosA,5);
			return lerp(c0,c1,t);
		}

		half3 UnpackUE4Normal(half3 normalColor)
		{
			normalColor.g = 1.0f - normalColor.g;
			normalColor = 2.0f * (normalColor - 0.5f);
			return normalize(normalColor);
		}

		half3 UnpackNormalCustom(half4 normalColor)
		{
			half3 normal = UnpackNormal(normalColor);
			normal.x = -normal.x;
			return normalize(normal);
		}

		half4 LinearToSRGB(half4 color)
		{
			return pow(color, 1 / 2.2);
		}

		half4 SRGBToLinear(fixed4 color)
		{
			return pow(color, 2.2);
		}

	ENDCG

	SubShader
	{
		Tags{"RenderType" = "Opaque"}
		pass
		{
			Tags{"LightMode" = "ForwardBase"}

			Cull [_Cull]

			CGPROGRAM

			#pragma target 3.0

			#pragma multi_compile_fwdbase
			#pragma multi_compile_fog
			#pragma multi_compile _ ENABLE_UE4  
			#pragma multi_compile _ ENABLE_CUSTOM_FOG
			#pragma multi_compile _ ENABLE_MATCAP
			#pragma multi_compile _ ENABLE_ADD_COLOR
			#include "../CustomShader.cginc"

			#pragma vertex vert
			#pragma fragment frag

			half4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _MaterialMap;
			sampler2D _BumpMap;
			half _MetallicMapScale;
			half _RoughnessMapScale;
			half _BloomPower;
			half _BumpScale;
			half _Brightness;
			half _Contrast;
			half _Saturation;
			half _LightmapScale;
			samplerCUBE _EnvTex;
			float _Spread;
			
			float _AddColorIntensity;
            uniform float4 _AddColor;
            
			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent :TANGENT;
				float2 uv : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
			};
			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				half4 ambientOrLightmapUV : TEXCOORD1;
				float4 TtoW0 : TEXCOORD2;
				float4 TtoW1 : TEXCOORD3;
				float4 TtoW2 : TEXCOORD4;
				SHADOW_COORDS(5) 
				UNITY_FOG_COORDS(6)
				float3 reflect : TEXCOORD7;
			};

			v2f vert(a2v v)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f,o);

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv,_MainTex);

				float3 worldPos = mul(unity_ObjectToWorld,v.vertex);
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				half3 worldTangent = UnityObjectToWorldDir(v.tangent);
				half3 worldBinormal = cross(worldNormal,worldTangent) * v.tangent.w;

				o.ambientOrLightmapUV = VertexGI(v.uv1 ,worldPos,worldNormal);
				o.TtoW0 = float4(worldTangent.x,worldBinormal.x,worldNormal.x,worldPos.x);
				o.TtoW1 = float4(worldTangent.y,worldBinormal.y,worldNormal.y,worldPos.y);
				o.TtoW2 = float4(worldTangent.z,worldBinormal.z,worldNormal.z,worldPos.z);

				fixed3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));
				o.reflect = reflect(-viewDir, worldNormal);

				TRANSFER_SHADOW(o);
				UNITY_TRANSFER_FOG(o,o.pos);

				return o;
			}

			half4 frag(v2f i) : SV_Target
			{
				float3 worldPos = float3(i.TtoW0.w,i.TtoW1.w,i.TtoW2.w);
				half3 albedo = tex2D(_MainTex,i.uv).rgb * _Color.rgb;
				half4 materialMap = tex2D(_MaterialMap, i.uv);//r:AO,g:Roughenss,b:Metallic
				half metallic = saturate(materialMap.b * _MetallicMapScale);
				half roughness = saturate(1 - materialMap.g * _RoughnessMapScale);
				half occlusion = materialMap.r;

			#ifdef ENABLE_UE4
				half3 normalTangent = UnpackUE4Normal(tex2D(_BumpMap, i.uv));
			#else
				half3 normalTangent = UnpackNormalCustom(tex2D(_BumpMap, i.uv)); 
			#endif

				normalTangent.xy *= _BumpScale;
				normalTangent.z = sqrt(1.0 - saturate(dot(normalTangent.xy,normalTangent.xy)));
				half3 worldNormal = normalize(half3(dot(i.TtoW0.xyz,normalTangent),
									dot(i.TtoW1.xyz,normalTangent),dot(i.TtoW2.xyz,normalTangent)));
				half3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
				half3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));
				half3 refDir = reflect(-viewDir,worldNormal);
				half3 emission = fixed3(0,0,0);

				UNITY_LIGHT_ATTENUATION(atten,i,worldPos);

				half3 halfDir = normalize(lightDir + viewDir);
				half nv = saturate(dot(worldNormal,viewDir));
				half nl = saturate(dot(worldNormal,lightDir));
				half nh = saturate(dot(worldNormal,halfDir));
				half lv = saturate(dot(lightDir,viewDir));
				half lh = saturate(dot(lightDir,halfDir));

				half3 specColor = lerp(unity_ColorSpaceDielectricSpec.rgb,albedo,metallic);
				half oneMinusReflectivity = (1- metallic) * unity_ColorSpaceDielectricSpec.a;
				half3 diffColor = albedo * oneMinusReflectivity;

				half3 indirectDiffuse = ComputeIndirectDiffuse(i.ambientOrLightmapUV,occlusion) / _LightmapScale;
				//half3 indirectSpecular = ComputeIndirectSpecular(refDir,worldPos,roughness,occlusion);

				half grazingTerm = saturate((1 - roughness) + (1-oneMinusReflectivity));
				//indirectSpecular *= ComputeFresnelLerp(specColor,grazingTerm,nv);
				indirectDiffuse *= diffColor;
				half V = ComputeSmithJointGGXVisibilityTerm(nl,nv,roughness);
				half D = ComputeGGXTerm(nh,roughness);
				half3 F = ComputeFresnelTerm(specColor,lh);
				half3 specularTerm = V * D * F;
				specularTerm = saturate(specularTerm);

				half3 diffuseTerm = ComputeDisneyDiffuseTerm(nv,nl,lh,roughness,diffColor);
				half3 color = UNITY_PI * (diffuseTerm + specularTerm) * _LightColor0.rgb * nl * atten
								+ indirectDiffuse  + emission;


				color.rgb *= _Brightness;
				half gray = 0.2125 * color.r + 0.7154 * color.g + 0.0721 * color.b;
				half3 grayColor = fixed3(gray, gray, gray);
				color.rgb = lerp(grayColor, color.rgb, _Saturation);
				half3 avgColor = half3(0.5, 0.5, 0.5);
				color.rgb = lerp(avgColor, color.rgb, _Contrast);

				//添加cubemap反射
				#if  ENABLE_MATCAP
				color.xyz += texCUBE(_EnvTex,i.reflect) * _Spread * materialMap.b;
				#endif

                //添加叠加光
                #ifdef ENABLE_ADD_COLOR
                float3 emissive = ((pow(1.0-max(0,dot(worldNormal, viewDir)),1.5)*_AddColor.rgb*2.0)+(_AddColor.rgb*0.3));
                color += (emissive * _AddColorIntensity);
                #endif

				half4 col = half4(color, _BloomPower);
			#ifdef ENABLE_CUSTOM_FOG
				col = GetCustomFog(worldPos, col);
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
	FallBack Off
}
