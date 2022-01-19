Shader "YanJia/Charater/CelFace"
{
	Properties
	{
		_MainTex ("MainTex", 2D) = "white" {}
		_SpeShaTex ("SpeShaTex", 2D) = "black" {}
        _FaceMapTex ("FaceMapTex", 2D) = "gray" {}
		_FaceMapScale ("FaceMapTex Scale", Range(0,2)) = 0.5
		_FaceMapOffset ("FaceMapTex Offset", Range(-0.5,0.5)) = 0

		[Space(20)]
		_ShadowColor ("Shadow Color", Color) = (0.7, 0.7, 0.7)
		_ShadowSmooth("Shadow Smooth", Range(0, 0.1)) = 0

		[Space(20)]
		_SurfaceRage ("Surface Rage", Range(0.01, 1.5)) = 0.9
        _SurfaceMult ("Surface Mult", Range(0,0.5)) = 0.07

		[Space(20)]
		_OutlineWidth ("Outline Width", Range(0, 1)) = 0.24
        _OutlineLightness ("Outline Lightness", Range(0, 1)) = 1
        _OutLineColor ("OutLine Color", Color) = (0.5,0.5,0.5,1)

		[Space(20)]
		_rimPower ("Rim Power", Range(0, 20)) = 20
        _rimColorRight ("Rim Color", Color) = (1,1,1,1)
		_rimIntensity ("Rim Intensity", Range(0,1)) = 0.2
		_RimBloomExp("Rim Bloom Exp", Range(0,10)) = 3.3
        _BloomPower ("Bloom Power", Range(0,1)) = 0.3

		[Space(20)]
		_Brightness("Brightness", Range(0,2)) = 1
		_Contrast("Contrast", Range(0, 2)) = 1
		_Saturation("Saturation", Range(0,2)) = 1

		//[Space(10)]
		//_AutoLigDir ("AutoLigDir", Float ) = 0

		[Space(20)]
        [KeywordEnum(All, R, G, B)] _Chanel ("Debuge Chanel",Float) = 0

		[Space(10)]
		[Toggle(ENABLE_SMOOTH_NORMAL)]_EnableSmoothNormal("Enable Smooth Normal", Float) = 0

		[Space(30)]
		[Toggle(ENABLE_CUTOFF)] _EnableCutOff ("Enable CutOff", Float) = 0
		_MaskTex("Mask Texture", 2D) = "white" {}
		_CutOff("Cut Off", Range(0, 1.0)) = 0
        _EdgeColor("Edge Color", Color) = (1,1,0)
        _EdgeLength("Edge Length", Range(0.0, 0.1)) = 0.012
	}

	SubShader
	{
		LOD 300

		tags {"RenderType" = "charater"}
		Pass 
		{
			Tags {"LightMode"="ForwardBase"}
			 
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            half _OutlineLightness;
            half _OutlineWidth;
            half4 _OutLineColor;
			#pragma multi_compile _ ENABLE_SMOOTH_NORMAL
			#pragma multi_compile _ ENABLE_CUTOFF

			sampler2D _MaskTex;
        	float4 _MaskTex_ST;
			fixed3 _EdgeColor;
			fixed _EdgeLength;
			fixed _CutOff;

            struct a2v 
			{
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float4 vertColor : COLOR;
               
#if ENABLE_SMOOTH_NORMAL
                float3 smoothNormal : TANGENT;
#endif
            };

            struct v2f
			{
                float4 pos : SV_POSITION;
                float2 uvMask : TEXCOORD5;
            };

            v2f vert (a2v v) 
			{
                v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
                float outLineFade = 1;

                float4x4 P = unity_CameraProjection;
				float fov =  atan(1 / P._m11) * 2;
				float fovScale = degrees(fov) / 40;
                outLineFade *= pow(fovScale, 0.5);

                float3 posWorld = mul(unity_ObjectToWorld, v.vertex);
                float distance = length(_WorldSpaceCameraPos.xyz - posWorld);
                //outLineFade *= pow(distance / 20, 1) * 6;
                outLineFade *= pow(distance, 0.5);
                //outLineFade = max(1, outLineFade);

                float4 pos = mul( UNITY_MATRIX_MV, v.vertex);
                
#if ENABLE_SMOOTH_NORMAL
                float3 normal = mul( (float3x3)(UNITY_MATRIX_IT_MV), v.smoothNormal);
#else                              
                float3 normal = mul( (float3x3)UNITY_MATRIX_IT_MV, v.normal);
#endif
                normal.z = -0.5;
                outLineFade /= 100; 
                pos = pos + float4(normalize(normal),0) * _OutlineWidth * outLineFade * v.vertColor.a;
                o.pos = mul(UNITY_MATRIX_P, pos);
                o.uvMask = TRANSFORM_TEX(v.uv, _MaskTex);
                //o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal * _OutlineWidth,1) );
                return o;
            }

            half4 frag(v2f i) : SV_TARGET 
			{
#if ENABLE_CUTOFF
				//溶解消失
				fixed4 maskTex = tex2D(_MaskTex, i.uvMask);
				clip(maskTex.r - _CutOff);
#endif
                return half4(_OutLineColor.rgb * _OutlineLightness, 0) ;
            }

            ENDCG
        }

		Pass
		{
			Tags { "LightMode"="ForwardBase"}

			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
            #include "AutoLight.cginc"
			
            #pragma multi_compile_fwdbase
            #pragma shader_feature _CHANEL_ALL _CHANEL_R _CHANEL_G _CHANEL_B
			#pragma multi_compile _ ADD_FACE_LIGHT
			#pragma multi_compile _ ENABLE_FIX_HEAD_BONE
			#pragma multi_compile _ ENABLE_CUTOFF
 
            sampler2D _MainTex; 
			float4 _MainTex_ST;
			sampler2D _SpeShaTex; 
            sampler2D _FaceMapTex; 
			float4 _FaceMapTex_ST;

			half3 _ShadowColor;
			half _ShadowFeather;
			half _ShadowSmooth;
			half _ShadowAera;

			//half _AutoLigDir;
			//half4 _LigDir;
			half _RimBloomExp;
            half _BloomPower;

			half _SurfaceRage;
			half _SurfaceMult;

           	half _rimPower;
			half3 _rimColorRight;
			half _rimIntensity;

			half _FaceMapOffset;
			half _FaceMapScale;

			float3 _VirtualLightDir;
			float4x4 _HeadWorldToLocalMatrix;

			half _Saturation;
			half _Brightness;
			half _Contrast;

			sampler2D _MaskTex;
        	float4 _MaskTex_ST;
			fixed3 _EdgeColor;
			fixed _EdgeLength;
			fixed _CutOff;

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
				float3 worldNormal : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 faceMapUV : TEXCOORD3; 
				UNITY_FOG_COORDS(4)
				float2 uvMask : TEXCOORD5;
			};

	
			
			v2f vert (a2v v)
			{
				v2f o = (v2f)0;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

				half3 faceMapUV;
				half3 lightLocalDir;
				half3 worldNormal = normalize(o.worldNormal);			
                fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
				//fixed3 worldLightDir = _AutoLigDir > 0.5 ? _LigDir : normalize(_WorldSpaceLightPos0.xyz);

#if ADD_FACE_LIGHT
				worldLightDir = _VirtualLightDir;
#endif

#if ENABLE_FIX_HEAD_BONE
				lightLocalDir = normalize(mul(_HeadWorldToLocalMatrix, worldLightDir));
#else
				lightLocalDir = normalize(mul(unity_WorldToObject, worldLightDir));
#endif

				half temp1;
				half temp2;
				temp2 = min (abs((lightLocalDir.z / lightLocalDir.y)), 1.0) / max (abs((lightLocalDir.z / lightLocalDir.y)), 1.0);
				half temp3;
				temp3 = temp2 * temp2;
				temp3 = ((((((((((-0.012 * temp3) + 0.054) * temp3) - 0.117)* temp3) + 0.194) * temp3) - 0.334)* temp3) + 1) * temp2;
				temp3 = temp3 + (half((abs((lightLocalDir.z / lightLocalDir.y)) > 1.0)) * ((temp3 * -2.0)+ 1.57));
				temp1 = temp3 * sign((lightLocalDir.z / lightLocalDir.y));
				if (abs(lightLocalDir.y) > (1e-08 * abs(lightLocalDir.z))) 
				{
					if (lightLocalDir.y < 0.0) 
					{
						if (lightLocalDir.z >= 0.0) 
						{
							temp1 += UNITY_PI;
						} 
						else 
						{
							temp1 = temp1 - UNITY_PI;
						};
					};
				} else 
				{
					temp1 = sign(lightLocalDir.z) * 1.57;
				};
				
				half flipInYAxis = -temp1 * 57.3;
				half lightingThreshold = clamp (abs((flipInYAxis / 180.0)), 0.0001, 0.9999);
				
				if (flipInYAxis > 0.0) 
				{
					faceMapUV.xy = v.uv.xy * half2(-1.0, 1.0);
				}
				else
				{
					faceMapUV.xy = v.uv;
				}
				faceMapUV.z = lightingThreshold;
				o.faceMapUV = faceMapUV;

				o.uvMask = TRANSFORM_TEX(v.uv, _MaskTex);

				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}
			
			half4 frag (v2f i) : SV_Target
			{
				//贴图采样
				half4 col = 0;
				half4 baseTexColor = tex2D (_MainTex, i.uv);
				half3 lightTexColor = tex2D (_SpeShaTex, i.uv).rgb;
				half diffuseThreshold = tex2D (_FaceMapTex, i.faceMapUV.xy).a * _FaceMapScale + _FaceMapOffset;
				half lightingThreshold = i.faceMapUV.z;

				//half threshold =  (diffuseThreshold - lightingThreshold) * 1000;
				half threshold = clamp (((diffuseThreshold - lightingThreshold) / (min ((lightingThreshold + _ShadowSmooth), 0.9999)- lightingThreshold)), 0.0, 1.0);
				//threshold = saturate(threshold);
				half3 diffuse = lerp( baseTexColor.rgb * _ShadowColor, baseTexColor.rgb ,  saturate(threshold * (1 - lightTexColor.b)));

				//计算变量
				half3 lightColor = _LightColor0.rgb;
				half3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
				half3 worldNormal = normalize(i.worldNormal);			
				//half3 worldLightDir = _AutoLigDir > 0.5 ? _LigDir : normalize(_WorldSpaceLightPos0.xyz);
				half3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
				half NdotL = max(0, dot(worldNormal, worldLightDir));
				half NdotV = max(0, dot(worldNormal, viewDir));

				//边缘光
				half3 rim = 0;
				half f =  1.0 - saturate(dot(normalize(viewDir), worldNormal));
				//rim = _rimColorRight.rgb * pow (f, _rimPower) * _rimIntensity;

				//流光
				half surface = 0;
				if(lightTexColor.r > 0.05)
				{
					surface = step(_SurfaceRage,saturate(pow (NdotV,1 - lightTexColor.b))) * _SurfaceMult;
				}
				half3 surfaceLight = half3(surface,surface,surface);

				//合并计算
				col.rgb = (diffuse + surfaceLight) * lightColor + rim;

				//色调调整
				half3 color = col.rgb;
				color.rgb *= _Brightness;
				half gray = 0.2125 * color.r + 0.7154 * color.g + 0.0721 * color.b;
				half3 grayColor = fixed3(gray, gray, gray);
				color.rgb = lerp(grayColor, color.rgb, _Saturation);
				half3 avgColor = half3(0.5, 0.5, 0.5);
				color.rgb = lerp(avgColor, color.rgb, _Contrast);
				col.rgb = color;

				//边缘发光
				half luminance = 0.2125 * baseTexColor.r + 0.7154  * baseTexColor.g + 0.0721 * baseTexColor.b;
				luminance = 1 - luminance / 1.5;
				half rimBloom = pow (f, _RimBloomExp) * 50 * NdotL;
				col.a = baseTexColor.a * _BloomPower / 3 +  rimBloom;
				col.a = min(0.7, col.a * luminance);

#if ENABLE_CUTOFF
				//溶解消失
				fixed4 maskTex = tex2D(_MaskTex, i.uvMask);
				col.rgb = lerp(col.rgb, _EdgeColor, maskTex.r - _CutOff > _EdgeLength ? 0 : 1);
				clip(maskTex.r - _CutOff);
#endif

                #if _CHANEL_R
                    col.rgb = half3(lightTexColor.r,0,0);
                #endif
                #if _CHANEL_G
                    col.rgb = half3(0,lightTexColor.g,0);
                #endif
                #if _CHANEL_B
                    col.rgb = half3(0,0,lightTexColor.b);
                #endif
				// diffuseThreshold = diffuseThreshold > _SurfaceMult ? 1:0;
				// diffuseThreshold *= 10;
				// diffuseThreshold = ceil(diffuseThreshold);
				// diffuseThreshold /= 10;
				// col.rgb = lerp(0,1,diffuseThreshold);
				UNITY_APPLY_FOG(i.fogCoord, col);
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

	SubShader
	{
		LOD 200

		tags {"RenderType" = "charater"}

		Pass
		{
			Tags { "LightMode"="ForwardBase"}

			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
            #include "AutoLight.cginc"
			
            #pragma multi_compile_fwdbase
            #pragma shader_feature _CHANEL_ALL _CHANEL_R _CHANEL_G _CHANEL_B
			#pragma multi_compile _ ADD_FACE_LIGHT
			#pragma multi_compile _ ENABLE_FIX_HEAD_BONE
			#pragma multi_compile _ ENABLE_CUTOFF
 
            sampler2D _MainTex; 
			float4 _MainTex_ST;
			sampler2D _SpeShaTex; 
            sampler2D _FaceMapTex; 
			float4 _FaceMapTex_ST;

			half3 _ShadowColor;
			half _ShadowFeather;
			half _ShadowSmooth;
			half _ShadowAera;

			//half _AutoLigDir;
			//half4 _LigDir;
			half _RimBloomExp;
            half _BloomPower;

			half _SurfaceRage;
			half _SurfaceMult;

           	half _rimPower;
			half3 _rimColorRight;
			half _rimIntensity;

			half _FaceMapOffset;
			half _FaceMapScale;

			float3 _VirtualLightDir;
			float4x4 _HeadWorldToLocalMatrix;

			half _Saturation;
			half _Brightness;
			half _Contrast;

			sampler2D _MaskTex;
        	float4 _MaskTex_ST;
			fixed3 _EdgeColor;
			fixed _EdgeLength;
			fixed _CutOff;

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
				float3 worldNormal : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 faceMapUV : TEXCOORD3; 
				UNITY_FOG_COORDS(4)
				float2 uvMask : TEXCOORD5;
			};

	
			
			v2f vert (a2v v)
			{
				v2f o = (v2f)0;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

				half3 faceMapUV;
				half3 lightLocalDir;
				half3 worldNormal = normalize(o.worldNormal);			
                fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
				//fixed3 worldLightDir = _AutoLigDir > 0.5 ? _LigDir : normalize(_WorldSpaceLightPos0.xyz);

#if ADD_FACE_LIGHT
				worldLightDir = _VirtualLightDir;
#endif

#if ENABLE_FIX_HEAD_BONE
				lightLocalDir = normalize(mul(_HeadWorldToLocalMatrix, worldLightDir));
#else
				lightLocalDir = normalize(mul(unity_WorldToObject, worldLightDir));
#endif

				half temp1;
				half temp2;
				temp2 = min (abs((lightLocalDir.z / lightLocalDir.y)), 1.0) / max (abs((lightLocalDir.z / lightLocalDir.y)), 1.0);
				half temp3;
				temp3 = temp2 * temp2;
				temp3 = ((((((((((-0.012 * temp3) + 0.054) * temp3) - 0.117)* temp3) + 0.194) * temp3) - 0.334)* temp3) + 1) * temp2;
				temp3 = temp3 + (half((abs((lightLocalDir.z / lightLocalDir.y)) > 1.0)) * ((temp3 * -2.0)+ 1.57));
				temp1 = temp3 * sign((lightLocalDir.z / lightLocalDir.y));
				if (abs(lightLocalDir.y) > (1e-08 * abs(lightLocalDir.z))) 
				{
					if (lightLocalDir.y < 0.0) 
					{
						if (lightLocalDir.z >= 0.0) 
						{
							temp1 += UNITY_PI;
						} 
						else 
						{
							temp1 = temp1 - UNITY_PI;
						};
					};
				} else 
				{
					temp1 = sign(lightLocalDir.z) * 1.57;
				};
				
				half flipInYAxis = -temp1 * 57.3;
				half lightingThreshold = clamp (abs((flipInYAxis / 180.0)), 0.0001, 0.9999);
				
				if (flipInYAxis > 0.0) 
				{
					faceMapUV.xy = v.uv.xy * half2(-1.0, 1.0);
				}
				else
				{
					faceMapUV.xy = v.uv;
				}
				faceMapUV.z = lightingThreshold;
				o.faceMapUV = faceMapUV;

				o.uvMask = TRANSFORM_TEX(v.uv, _MaskTex);

				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}
			
			half4 frag (v2f i) : SV_Target
			{
				//贴图采样
				half4 col = 0;
				half4 baseTexColor = tex2D (_MainTex, i.uv);
				half3 lightTexColor = tex2D (_SpeShaTex, i.uv).rgb;
				half diffuseThreshold = tex2D (_FaceMapTex, i.faceMapUV.xy).a * _FaceMapScale + _FaceMapOffset;
				half lightingThreshold = i.faceMapUV.z;

				//half threshold =  (diffuseThreshold - lightingThreshold) * 1000;
				half threshold = clamp (((diffuseThreshold - lightingThreshold) / (min ((lightingThreshold + _ShadowSmooth), 0.9999)- lightingThreshold)), 0.0, 1.0);
				//threshold = saturate(threshold);
				half3 diffuse = lerp( baseTexColor.rgb * _ShadowColor, baseTexColor.rgb ,  saturate(threshold * (1 - lightTexColor.b)));

				//计算变量
				half3 lightColor = _LightColor0.rgb;
				half3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
				half3 worldNormal = normalize(i.worldNormal);			
				//half3 worldLightDir = _AutoLigDir > 0.5 ? _LigDir : normalize(_WorldSpaceLightPos0.xyz);
				half3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
				half NdotL = max(0, dot(worldNormal, worldLightDir));
				half NdotV = max(0, dot(worldNormal, viewDir));

				//边缘光
				half3 rim = 0;
				half f =  1.0 - saturate(dot(normalize(viewDir), worldNormal));
				//rim = _rimColorRight.rgb * pow (f, _rimPower) * _rimIntensity;

				//流光
				half surface = 0;
				if(lightTexColor.r > 0.05)
				{
					surface = step(_SurfaceRage,saturate(pow (NdotV,1 - lightTexColor.b))) * _SurfaceMult;
				}
				half3 surfaceLight = half3(surface,surface,surface);

				//合并计算
				col.rgb = (diffuse + surfaceLight) * lightColor + rim;

				//色调调整
				half3 color = col.rgb;
				color.rgb *= _Brightness;
				half gray = 0.2125 * color.r + 0.7154 * color.g + 0.0721 * color.b;
				half3 grayColor = fixed3(gray, gray, gray);
				color.rgb = lerp(grayColor, color.rgb, _Saturation);
				half3 avgColor = half3(0.5, 0.5, 0.5);
				color.rgb = lerp(avgColor, color.rgb, _Contrast);
				col.rgb = color;

				//边缘发光
				half luminance = 0.2125 * baseTexColor.r + 0.7154  * baseTexColor.g + 0.0721 * baseTexColor.b;
				luminance = 1 - luminance / 1.5;
				half rimBloom = pow (f, _RimBloomExp) * 50 * NdotL;
				col.a = baseTexColor.a * _BloomPower / 3 +  rimBloom;
				col.a = min(0.7, col.a * luminance);

#if ENABLE_CUTOFF
				//溶解消失
				fixed4 maskTex = tex2D(_MaskTex, i.uvMask);
				col.rgb = lerp(col.rgb, _EdgeColor, maskTex.r - _CutOff > _EdgeLength ? 0 : 1);
				clip(maskTex.r - _CutOff);
#endif

                #if _CHANEL_R
                    col.rgb = half3(lightTexColor.r,0,0);
                #endif
                #if _CHANEL_G
                    col.rgb = half3(0,lightTexColor.g,0);
                #endif
                #if _CHANEL_B
                    col.rgb = half3(0,0,lightTexColor.b);
                #endif
				// diffuseThreshold = diffuseThreshold > _SurfaceMult ? 1:0;
				// diffuseThreshold *= 10;
				// diffuseThreshold = ceil(diffuseThreshold);
				// diffuseThreshold /= 10;
				// col.rgb = lerp(0,1,diffuseThreshold);
				UNITY_APPLY_FOG(i.fogCoord, col);
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
