Shader "YanJia/Charater/CelCharaterSSS"
{
	Properties
	{
		_MainTex ("MainTex", 2D) = "white" {}
        _SpeShaTex ("SpeShaTex", 2D) = "white" {}

		[Space(20)]
		_MainColor("Main Color", Color) = (1,1,1)
		_ShadowColor ("Shadow Color (A > 0.7)", Color) = (0.7, 0.7, 0.7)
		_ShadowColor1 ("Shadow Color1 (A > 0.3)", Color) = (0.6, 0.6, 0.6)
		_ShadowColor2 ("Shadow Color2 (A < 0.3)", Color) = (0.5, 0.5, 0.5)
		_ShadowSmooth("Shadow Smooth", Range(0, 0.03)) = 0.002
		_ShadowAera ("Shadow Range", Range(0, 1)) = 0.6
		_ShadowMult ("Shadow Mult", Range(0, 1)) = 0

		[Space(20)]
		_SurfaceRage ("Surface Rage", Range(0.01, 1.5)) = 0.9
        _SurfaceMult ("Surface Mult", Range(0,1)) = 0.07

		[Space(20)]
		_SpecularAera ("Specular Range", Range(0, 1)) = 0.9
        _SpecularMulti ("Specular Mult", Range(0, 10)) = 0.9
		_SpecularGloss("Sprecular Gloss", Range(0.001, 32)) = 4

		[Space(20)]
		_OutlineWidth ("Outline Width", Range(0, 1)) = 0.24
        _OutlineLightness ("Outline Lightness", Range(0, 1)) = 1
        _OutLineColor ("OutLine Color", Color) = (0.5,0.5,0.5,1)

		[Space(20)]
		_RimColor("Rim Color", color) = (1,1,1,0.3)
		_RimMin ("Rim Min", Range(0,1)) = 0.7
		_RimMax ("Rim Max", Range(0,1)) = 1.0
		_RimSmooth("Rim Smooth", Range(0.005,1)) = 0.03

		[Space(20)]
		_RimBloomExp("Bloom Rim Exp", Range(0,10)) = 3.3
		_RimBloomMulti("Bloom Rim Multi", Range(0,20)) = 10
        _BloomMulti ("Bloom Multi", Range(0,1)) = 0.1
		_BloomMulti1 ("Bloom Multi1", Range(0,1)) = 0.1
		_BloomMulti2 ("Bloom Multi2", Range(0,1)) = 0.1
		_Emission("Emission", Range(0,1)) = 0

		[Space(20)]
		_Saturation("Saturation", Range(0, 2)) = 1
		_MatelSaturation("Matel Saturation", Range(0, 3)) = 2
		_Brightness("Brightness", Range(0.5,1.5)) = 1
		_Contrast("Contrast", Range(0.5, 1.5)) = 1

		[Space(30)]
        [KeywordEnum(All, R, G, B, A)] _Chanel ("Debuge Chanel",Float) = 0

		[Space(10)]
		[Toggle(ENABLE_MODIFYCHANNEL)]_EnableModifyChannel("Enable Fixed Shadow", Float) = 0
		[Toggle(ENABLE_SMOOTH_NORMAL)]_EnableSmoothNormal("Enable Smooth Normal", Float) = 0
		_FixedShadowCutOff("Fixed Shadow CutOff", Range(0,0.8)) = 0.31
		_FixedShadowMult("Fixed Shadow Mult", Range(0,1)) = 0.2

		[Space(30)]
		[Toggle(ENABLE_CUTOFF)] _EnableCutOff ("Enable CutOff", Float) = 0
		_MaskTex("Mask Texture", 2D) = "white" {}
		_CutOff("Cut Off", Range(0, 1.0)) = 0
        _EdgeColor("Edge Color", Color) = (1,1,0)
        _EdgeLength("Edge Length", Range(0.0, 0.1)) = 0.012
	}

	SubShader
	{
		tags {"RenderType" = "charater"}
		LOD 300

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
            #pragma shader_feature _CHANEL_ALL _CHANEL_R _CHANEL_G _CHANEL_B _CHANEL_A
            #pragma multi_compile _ ENABLE_MODIFYCHANNEL
			#pragma multi_compile _ ADD_FACE_LIGHT
			#pragma multi_compile _ ENABLE_CUTOFF
 
            sampler2D _MainTex; 
			float4 _MainTex_ST;
            sampler2D _SpeShaTex; 
			float4 _SpeShaTex_ST;

			fixed3 _MainColor;
			fixed3 _ShadowColor;
			fixed3 _ShadowColor1;
			fixed3 _ShadowColor2;
			fixed _ShadowSmooth;
			fixed _ShadowAera;
			fixed _ShadowMult;

			fixed _SurfaceRage;
			fixed _SurfaceMult;
			
			fixed _SpecularAera;
        	fixed _SpecularMulti;
			fixed _SpecularGloss;

			fixed _RimMin;
			fixed _RimMax;
			fixed4 _RimColor;
			fixed _RimSmooth;
			half _RimBloomExp;
			fixed _RimBloomMulti;
            fixed _BloomMulti;
			fixed _BloomMulti1;
			fixed _BloomMulti2;
			fixed _Emission;

			fixed _ShadowRange;
			fixed _SpecularRange;
			fixed _SpecularMult;
			fixed _ShadowIntensity;
			fixed _Saturation;
			fixed _Brightness;
			fixed _Contrast;
			fixed _MatelSaturation;
			fixed _FixedShadowCutOff;
			fixed _FixedShadowMult;

			sampler2D _MaskTex;
        	float4 _MaskTex_ST;
			fixed3 _EdgeColor;
			fixed _EdgeLength;
			fixed _CutOff;

			float3 _VirtualLightDir;

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
				UNITY_FOG_COORDS(3)
#if ENABLE_CUTOFF
				float2 uvMask : TEXCOORD4;
#endif
			};

	
			
			v2f vert (a2v v)
			{
				v2f o = (v2f)0;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				UNITY_TRANSFER_FOG(o,o.pos);
#if ENABLE_CUTOFF
				o.uvMask = TRANSFORM_TEX(v.uv, _MaskTex);
#endif
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//计算变量
				fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
				fixed3 worldNormal = normalize(i.worldNormal);
				fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
#if ADD_FACE_LIGHT
				worldLightDir = _VirtualLightDir;
#endif
				fixed3 lightColor = _LightColor0.rgb;
				fixed3 halfDir = normalize(worldLightDir + viewDir);
				fixed NdotL = max(0, dot(worldNormal, worldLightDir));
				fixed NdotV = max(0, dot(worldNormal, viewDir));
				fixed NdotH = max(0, dot(worldNormal, halfDir));
				fixed halfLambert = dot(worldNormal, worldLightDir) * 0.5 + 0.5;

				//贴图采样
				fixed4 col = 0;
				fixed4 baseTexColor = tex2D (_MainTex, i.uv);
				fixed4 lightTexColor = tex2D (_SpeShaTex, i.uv);

				//调整整体饱和度
				fixed luminance = 0.2125 * baseTexColor.r + 0.7154  * baseTexColor.g + 0.0721 * baseTexColor.b;
				fixed3 luminanceColor = fixed3(luminance, luminance, luminance);
				fixed3 color = baseTexColor.rgb;
				baseTexColor.rgb = lerp(luminanceColor, color, _Saturation);

				//调整金属饱和度
				luminance = 0.2125 * baseTexColor.r + 0.7154  * baseTexColor.g + 0.0721 * baseTexColor.b;
				luminanceColor = fixed3(luminance, luminance, luminance);
				fixed3 matelColor = baseTexColor.rgb;
				matelColor = lerp(luminanceColor, matelColor, _MatelSaturation);
				baseTexColor.rgb = lerp(baseTexColor.rgb, matelColor, lightTexColor.b);

				//调整整体亮度、对比度
				baseTexColor.rgb *= _Brightness;
				fixed3 avgColor = fixed3(0.5, 0.5, 0.5);
				baseTexColor.rgb = lerp(avgColor, baseTexColor.rgb, _Contrast);

				
				//阴影残留
				#if ENABLE_MODIFYCHANNEL
				fixed3 fixedShadowColor =  baseTexColor.rgb > 0.5 ? 1.0 - (2.0 - baseTexColor.rgb * 2) * (1.0 - lightTexColor.g) : 2.0 * baseTexColor.rgb * lightTexColor.g;
				fixed fixedShadow =  saturate(saturate(lightTexColor.g - _FixedShadowCutOff) * 10);
				fixedShadow = saturate(fixedShadow  + saturate(1 - _FixedShadowMult * 3));
				baseTexColor.rgb = lerp(fixedShadowColor,  baseTexColor.rgb, fixedShadow);
				#endif

				//根据alpha通道数值设置色调
				fixed thresholdA = lightTexColor.a;
				fixed3 shadowColor = _ShadowColor;
				fixed bloomMulti = _BloomMulti;
				if(thresholdA < 0.7)
				{
					shadowColor = _ShadowColor1;
					bloomMulti = _BloomMulti1;
					if(thresholdA < 0.3)
					{
						shadowColor = _ShadowColor2;
						bloomMulti = _BloomMulti2;
					}
				}
				

				//漫反射
				fixed3 diffuse = 0;
				fixed threshold = (halfLambert + lightTexColor.g) * 0.5;
				fixed ramp = saturate(_ShadowAera  - threshold); // 新的阴影柔化
				ramp =  smoothstep(0, _ShadowSmooth, ramp);// 新的阴影柔化
				diffuse = lerp(baseTexColor.rgb * _MainColor, baseTexColor.rgb * shadowColor * (1 - _ShadowMult), ramp);
				fixed3 diffuseLightAera = diffuse * (1 - ramp);;

				//流光
				fixed3 surfaceLight = 0;
				fixed surfaceLightIf = 0;
				surfaceLightIf = lightTexColor.r > 0.05 ? 1 : 0;
				surfaceLight = step(_SurfaceRage,saturate(pow(NdotV,1 - lightTexColor.b))) * _SurfaceMult  * diffuseLightAera;
				surfaceLightIf *= 1 - ramp;
				surfaceLight *= surfaceLightIf;

				//高光
				fixed3 specular = 0;
				half SpecularSize = pow(NdotH, _SpecularGloss);
				fixed specularMask = lightTexColor.b;
				if (SpecularSize >= 1 - specularMask * _SpecularAera)
				{
					specular = _SpecularMulti * (lightTexColor.r) * diffuseLightAera;//真实高光
				}
				SpecularSize = pow(NdotV, _SpecularGloss);
				if (SpecularSize >= 1 - specularMask * _SpecularAera)
				{
					specular = _SpecularMulti * (lightTexColor.r) * diffuseLightAera;//视线方向的作假高光
				}
				fixed specularIf = lightTexColor.r > 0.1 ? 1 : 0;
				specular *= specularIf;

				//边缘光
				fixed f =  1.0 - saturate(dot(normalize(viewDir), worldNormal));
				fixed rim = smoothstep(_RimMin, _RimMax, f);
				rim = smoothstep(0, _RimSmooth, rim);
				fixed3 rimColor = rim * _RimColor.rgb * diffuse * _RimColor.a * (1 - luminance / 2);

				//合并计算
				col.rgb = (diffuse + max(rimColor, max(surfaceLight, specular))) * lightColor;

				//边缘发光
				fixed BloomFade = 1 - luminance;
				fixed rimBloom = pow (f, _RimBloomExp) * _RimBloomMulti * NdotL; //新rim bloom
				col.a = (baseTexColor.a * bloomMulti +  rimBloom) * BloomFade;

				//自发光
				col.rgb += baseTexColor.rgb * _Emission * (1 - bloomMulti);
				col.a += _Emission * (1 - bloomMulti);

#if ENABLE_CUTOFF
				//溶解消失
				fixed4 maskTex = tex2D(_MaskTex, i.uvMask);
				col.rgb = lerp(col.rgb, _EdgeColor, maskTex.r - _CutOff > _EdgeLength ? 0 : 1);
				clip(maskTex.r - _CutOff);
#endif

                #if _CHANEL_R
                    col.rgb = fixed3(lightTexColor.r,0,0);
                #endif
                #if _CHANEL_G
                    col.rgb = fixed3(0,lightTexColor.g,0);
                #endif
                #if _CHANEL_B
                    col.rgb = fixed3(0,0,lightTexColor.b);
                #endif
				#if _CHANEL_A
					 col.rgb = fixed3(lightTexColor.a,lightTexColor.a,lightTexColor.a);
				#endif

				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}

		Pass 
		{
			Tags {"LightMode"="ForwardBase"}
			 
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed _OutlineLightness;
            fixed _OutlineWidth;
            fixed4 _OutLineColor;
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
//#if ENABLE_SMOOTH_NORMAL
//                float3 smoothNormal : TANGENT;
//#endif
            };

            struct v2f
			{
                float4 pos : SV_POSITION;
#if ENABLE_CUTOFF
				float2 uvMask : TEXCOORD0;
#endif
				float4 vertColor : TEXCOORD1;
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
                outLineFade *= pow(distance, 0.5);
                float4 pos = mul( UNITY_MATRIX_MV, v.vertex);
                
//#if ENABLE_SMOOTH_NORMAL
//                float3 normal = mul( (float3x3)(UNITY_MATRIX_IT_MV), v.smoothNormal);
//#else                              
                float3 normal = mul( (float3x3)UNITY_MATRIX_IT_MV, v.normal);
//#endif
                normal.z = -0.5;
                outLineFade /= 100; 
                pos = pos + float4(normalize(normal),0) * _OutlineWidth * outLineFade * v.vertColor.a;
                o.pos = mul(UNITY_MATRIX_P, pos);

#if ENABLE_CUTOFF
				o.uvMask = TRANSFORM_TEX(v.uv, _MaskTex);
#endif
				o.vertColor = v.vertColor;
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET 
			{
#if ENABLE_CUTOFF
				//溶解消失
				fixed4 maskTex = tex2D(_MaskTex, i.uvMask);
				clip(maskTex.r - _CutOff);
#endif
                return fixed4(_OutLineColor.rgb * i.vertColor.rgb * _OutlineLightness, 0);
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
            
			#include "UnityCG.cginc"
			half _OutlineWidth;

			struct v2f
			{
				V2F_SHADOW_CASTER;
			};

			v2f vert(appdata_base v)
			{
				half outLineFade = 1;
                float4x4 P = unity_CameraProjection;
				half fov =  atan(1 / P._m11) * 2;
				half fovScale = degrees(fov) / 40;
                outLineFade *= pow(fovScale, 0.5);
                half3 posWorld = mul(unity_ObjectToWorld, v.vertex);
                half distance = length(_WorldSpaceCameraPos.xyz - posWorld);
                outLineFade *= pow(distance, 0.5);
                outLineFade /= 80; //比描边多外扩一些写深度
				v.vertex.xyz += v.normal * _OutlineWidth * outLineFade;//描边外扩

				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				SHADOW_CASTER_FRAGMENT(i)
			}
           
            ENDCG
        }
	}

	SubShader
	{
		tags {"RenderType" = "charater"}
		LOD 200

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
            #pragma shader_feature _CHANEL_ALL _CHANEL_R _CHANEL_G _CHANEL_B _CHANEL_A
            #pragma multi_compile _ ENABLE_MODIFYCHANNEL
			#pragma multi_compile _ ADD_FACE_LIGHT
			#pragma multi_compile _ ENABLE_CUTOFF
 
            sampler2D _MainTex; 
			float4 _MainTex_ST;
            sampler2D _SpeShaTex; 
			float4 _SpeShaTex_ST;

			fixed3 _MainColor;
			fixed3 _ShadowColor;
			fixed3 _ShadowColor1;
			fixed3 _ShadowColor2;
			fixed _ShadowSmooth;
			fixed _ShadowAera;
			fixed _ShadowMult;

			fixed _SurfaceRage;
			fixed _SurfaceMult;
			
			fixed _SpecularAera;
        	fixed _SpecularMulti;
			fixed _SpecularGloss;

			fixed _RimMin;
			fixed _RimMax;
			fixed4 _RimColor;
			fixed _RimSmooth;
			half _RimBloomExp;
			fixed _RimBloomMulti;
            fixed _BloomMulti;
			fixed _BloomMulti1;
			fixed _BloomMulti2;
			fixed _Emission;

			fixed _ShadowRange;
			fixed _SpecularRange;
			fixed _SpecularMult;
			fixed _ShadowIntensity;
			fixed _Saturation;
			fixed _Brightness;
			fixed _Contrast;
			fixed _MatelSaturation;
			fixed _FixedShadowCutOff;
			fixed _FixedShadowMult;

			sampler2D _MaskTex;
        	float4 _MaskTex_ST;
			fixed3 _EdgeColor;
			fixed _EdgeLength;
			fixed _CutOff;

			float3 _VirtualLightDir;

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
				UNITY_FOG_COORDS(3)
#if ENABLE_CUTOFF
				float2 uvMask : TEXCOORD4;
#endif
			};

	
			
			v2f vert (a2v v)
			{
				v2f o = (v2f)0;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				UNITY_TRANSFER_FOG(o,o.pos);
#if ENABLE_CUTOFF
				o.uvMask = TRANSFORM_TEX(v.uv, _MaskTex);
#endif
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//计算变量
				fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
				fixed3 worldNormal = normalize(i.worldNormal);
				fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
#if ADD_FACE_LIGHT
				worldLightDir = _VirtualLightDir;
#endif
				fixed3 lightColor = _LightColor0.rgb;
				fixed3 halfDir = normalize(worldLightDir + viewDir);
				fixed NdotL = max(0, dot(worldNormal, worldLightDir));
				fixed NdotV = max(0, dot(worldNormal, viewDir));
				fixed NdotH = max(0, dot(worldNormal, halfDir));
				fixed halfLambert = dot(worldNormal, worldLightDir) * 0.5 + 0.5;

				//贴图采样
				fixed4 col = 0;
				fixed4 baseTexColor = tex2D (_MainTex, i.uv);
				fixed4 lightTexColor = tex2D (_SpeShaTex, i.uv);

				//调整整体饱和度
				fixed luminance = 0.2125 * baseTexColor.r + 0.7154  * baseTexColor.g + 0.0721 * baseTexColor.b;
				fixed3 luminanceColor = fixed3(luminance, luminance, luminance);
				fixed3 color = baseTexColor.rgb;
				baseTexColor.rgb = lerp(luminanceColor, color, _Saturation);

				//调整金属饱和度
				luminance = 0.2125 * baseTexColor.r + 0.7154  * baseTexColor.g + 0.0721 * baseTexColor.b;
				luminanceColor = fixed3(luminance, luminance, luminance);
				fixed3 matelColor = baseTexColor.rgb;
				matelColor = lerp(luminanceColor, matelColor, _MatelSaturation);
				baseTexColor.rgb = lerp(baseTexColor.rgb, matelColor, lightTexColor.b);

				//调整整体亮度、对比度
				baseTexColor.rgb *= _Brightness;
				fixed3 avgColor = fixed3(0.5, 0.5, 0.5);
				baseTexColor.rgb = lerp(avgColor, baseTexColor.rgb, _Contrast);

				
				//阴影残留
				#if ENABLE_MODIFYCHANNEL
				fixed3 fixedShadowColor =  baseTexColor.rgb > 0.5 ? 1.0 - (2.0 - baseTexColor.rgb * 2) * (1.0 - lightTexColor.g) : 2.0 * baseTexColor.rgb * lightTexColor.g;
				fixed fixedShadow =  saturate(saturate(lightTexColor.g - _FixedShadowCutOff) * 10);
				fixedShadow = saturate(fixedShadow  + saturate(1 - _FixedShadowMult * 3));
				baseTexColor.rgb = lerp(fixedShadowColor,  baseTexColor.rgb, fixedShadow);
				#endif

				//根据alpha通道数值设置色调
				fixed thresholdA = lightTexColor.a;
				fixed3 shadowColor = _ShadowColor;
				fixed bloomMulti = _BloomMulti;
				if(thresholdA < 0.7)
				{
					shadowColor = _ShadowColor1;
					bloomMulti = _BloomMulti1;
					if(thresholdA < 0.3)
					{
						shadowColor = _ShadowColor2;
						bloomMulti = _BloomMulti2;
					}
				}
				

				//漫反射
				fixed3 diffuse = 0;
				fixed threshold = (halfLambert + lightTexColor.g) * 0.5;
				fixed ramp = saturate(_ShadowAera  - threshold); // 新的阴影柔化
				ramp =  smoothstep(0, _ShadowSmooth, ramp);// 新的阴影柔化
				diffuse = lerp(baseTexColor.rgb * _MainColor, baseTexColor.rgb * shadowColor * (1 - _ShadowMult), ramp);
				fixed3 diffuseLightAera = diffuse * (1 - ramp);;

				//流光
				fixed3 surfaceLight = 0;
				fixed surfaceLightIf = 0;
				surfaceLightIf = lightTexColor.r > 0.05 ? 1 : 0;
				surfaceLight = step(_SurfaceRage,saturate(pow(NdotV,1 - lightTexColor.b))) * _SurfaceMult  * diffuseLightAera;
				surfaceLightIf *= 1 - ramp;
				surfaceLight *= surfaceLightIf;

				//高光
				fixed3 specular = 0;
				half SpecularSize = pow(NdotH, _SpecularGloss);
				fixed specularMask = lightTexColor.b;
				if (SpecularSize >= 1 - specularMask * _SpecularAera)
				{
					specular = _SpecularMulti * (lightTexColor.r) * diffuseLightAera;//真实高光
				}
				SpecularSize = pow(NdotV, _SpecularGloss);
				if (SpecularSize >= 1 - specularMask * _SpecularAera)
				{
					specular = _SpecularMulti * (lightTexColor.r) * diffuseLightAera;//视线方向的作假高光
				}
				fixed specularIf = lightTexColor.r > 0.1 ? 1 : 0;
				specular *= specularIf;

				//边缘光
				fixed f =  1.0 - saturate(dot(normalize(viewDir), worldNormal));
				fixed rim = smoothstep(_RimMin, _RimMax, f);
				rim = smoothstep(0, _RimSmooth, rim);
				fixed3 rimColor = rim * _RimColor.rgb * diffuse * _RimColor.a * (1 - luminance / 2);

				//合并计算
				col.rgb = (diffuse + max(rimColor, max(surfaceLight, specular))) * lightColor;

				//边缘发光
				fixed BloomFade = 1 - luminance;
				fixed rimBloom = pow (f, _RimBloomExp) * _RimBloomMulti * NdotL; //新rim bloom
				col.a = (baseTexColor.a * bloomMulti +  rimBloom) * BloomFade;

				//自发光
				col.rgb += baseTexColor.rgb * _Emission * (1 - bloomMulti);
				col.a += _Emission * (1 - bloomMulti);

#if ENABLE_CUTOFF
				//溶解消失
				fixed4 maskTex = tex2D(_MaskTex, i.uvMask);
				col.rgb = lerp(col.rgb, _EdgeColor, maskTex.r - _CutOff > _EdgeLength ? 0 : 1);
				clip(maskTex.r - _CutOff);
#endif

                #if _CHANEL_R
                    col.rgb = fixed3(lightTexColor.r,0,0);
                #endif
                #if _CHANEL_G
                    col.rgb = fixed3(0,lightTexColor.g,0);
                #endif
                #if _CHANEL_B
                    col.rgb = fixed3(0,0,lightTexColor.b);
                #endif
				#if _CHANEL_A
					 col.rgb = fixed3(lightTexColor.a,lightTexColor.a,lightTexColor.a);
				#endif

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
            
			#include "UnityCG.cginc"
			half _OutlineWidth;

			struct v2f
			{
				V2F_SHADOW_CASTER;
			};

			v2f vert(appdata_base v)
			{
				half outLineFade = 1;
                float4x4 P = unity_CameraProjection;
				half fov =  atan(1 / P._m11) * 2;
				half fovScale = degrees(fov) / 40;
                outLineFade *= pow(fovScale, 0.5);
                half3 posWorld = mul(unity_ObjectToWorld, v.vertex);
                half distance = length(_WorldSpaceCameraPos.xyz - posWorld);
                outLineFade *= pow(distance, 0.5);
                outLineFade /= 80; //比描边多外扩一些写深度
				v.vertex.xyz += v.normal * _OutlineWidth * outLineFade;//描边外扩

				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				SHADOW_CASTER_FRAGMENT(i)
			}
           
            ENDCG
        }
	}
	
	FallBack Off
}
