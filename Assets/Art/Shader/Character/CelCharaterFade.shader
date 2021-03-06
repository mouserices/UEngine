Shader "YanJia/Charater/CelCharaterFade"
{
	Properties
	{
		_MainTex ("MainTex", 2D) = "white" {}
        _SpeShaTex ("SpeShaTex", 2D) = "white" {}

		[Space(20)]
		_MainColor("Main Color", Color) = (1,1,1)
		_ShadowColor ("Shadow Color", Color) = (0.7, 0.7, 0.7)
		_ShadowSmooth("Shadow Smooth", Range(0, 0.03)) = 0.002
		_ShadowAera ("Shadow Range", Range(0, 1)) = 0.6
		_ShadowMult ("Shadow Mult", Range(0, 1)) = 0
		//_LimGScale("Lim Gchannel Scale", Range(0,3)) = 1

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
		_Emission("Emission", Range(0,1)) = 0

		[Space(20)]
		_Saturation("Saturation", Range(0, 2)) = 1
		_MatelSaturation("Matel Saturation", Range(0, 3)) = 2
		_Brightness("Brightness", Range(0.5,1.5)) = 1
		_Contrast("Contrast", Range(0.5, 1.5)) = 1

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

		[Space(30)]
		_Color("Aura Color", Color) = (0,0,1,1)
		_ColorR("Rim Color", Color) = (0,1,1,1)
		_BackColor ("BackColor", Color) = (0.2,0.2,0.2,1)
		_Outline("Outline width", Range(.002, 0.8)) = 0.3
		_OutlineZ("Outline Z", Range(-1, 0)) = -0.1
		_NoiseTex("Noise Texture", 2D) = "white" { }
		_NoiseScale("Noise tex Scale", Range(0,2)) = 1
        _NoiseCutOff("Noise Cutoff", Range(0,1)) = 0
		_FadeTex("Fade Tex", 2D) = "white" {}
		_FadeScale("Fade Tex", Range(0,5)) = 1
		_FadeOffset("Fade Offset", Range(-1,1)) = 0
		_Scale("Noise Scale", Range(0.0, 5)) = 1
		_SpeedX("Speed X", Range(-10, 10)) = 0
		_SpeedY("Speed Y", Range(-10, 10)) = 3.0
		_Opacity("Noise Opacity", Range(0.01, 10.0)) = 10
		_FadeBrightness("Fade Brightness", Range(0.5, 5)) = 2
		_Edge("Rim Edge", Range(0.0, 5)) = 0.1
		_RimPower("Rim Power", Range(0.01, 5)) = 1
		_WorldOffsetX("World Offset X", Range(0,20)) = 5
		_WorldOffsetY("World Offset Y", Range(0,20)) = 0
	}

	SubShader
	{
		tags {"RenderType" = "charater"}
		LOD 300

		Pass
		{
			Tags { "LightMode"="ForwardBase"}

			Cull Off
			
			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
            #include "AutoLight.cginc"
			
            #pragma multi_compile_fwdbase
            #pragma multi_compile _ ENABLE_MODIFYCHANNEL
			#pragma multi_compile _ ADD_FACE_LIGHT
			#pragma multi_compile _ ENABLE_CUTOFF
 
            sampler2D _MainTex; 
			float4 _MainTex_ST;
            sampler2D _SpeShaTex; 
			float4 _SpeShaTex_ST;

			fixed3 _MainColor;
			fixed3 _ShadowColor;
			fixed _ShadowSmooth;
			fixed _ShadowAera;
			fixed _ShadowMult;
			//fixed _LimGScale;

			fixed _SurfaceRage;
			fixed _SurfaceMult;
			
			fixed _SpecularAera;
        	fixed _SpecularMulti;
			fixed _SpecularGloss;

           	// half _rimPower;
			// fixed3 _rimColorRight;
			// fixed _rimIntensity;
			// fixed _rimSmooth;
			fixed _RimMin;
			fixed _RimMax;
			fixed4 _RimColor;
			fixed _RimSmooth;
			//fixed _AutoLigDir;
			//fixed4 _LigDir;
			half _RimBloomExp;
			fixed _RimBloomMulti;
            fixed _BloomMulti;
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

			sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
			sampler2D _FadeTex;
			float _FadeScale;
			float _FadeOffset;
			float4 _FadeTex_ST;
            half _Scale, _Opacity, _Edge;
            fixed4 _Color, _ColorR, _BackColor;
            half _FadeBrightness, _SpeedX, _SpeedY;
            half _Outline;
            half _OutlineZ;
            half _RimPower;
            fixed _NoiseCutOff;
			half _NoiseScale;
			half _WorldOffsetX;
			half _WorldOffsetY;


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
				float4 vertex : TEXCOORD5;
			};

	
			
			v2f vert (a2v v)
			{
				v2f o = (v2f)0;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.vertex = v.vertex;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				UNITY_TRANSFER_FOG(o,o.pos);
#if ENABLE_CUTOFF
				o.uvMask = TRANSFORM_TEX(v.uv, _MaskTex);
#endif
				return o;
			}
			
			fixed4 frag (v2f i,  fixed facing : VFACE) : SV_Target
			{
				//????????????
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

				//????????????
				fixed4 col = 0;
				fixed4 baseTexColor = tex2D (_MainTex, i.uv);
				fixed4 lightTexColor = tex2D (_SpeShaTex, i.uv);

			

				//?????????????????????
				fixed luminance = 0.2125 * baseTexColor.r + 0.7154  * baseTexColor.g + 0.0721 * baseTexColor.b;
				fixed3 luminanceColor = fixed3(luminance, luminance, luminance);
				fixed3 color = baseTexColor.rgb;
				baseTexColor.rgb = lerp(luminanceColor, color, _Saturation);

				//?????????????????????
				luminance = 0.2125 * baseTexColor.r + 0.7154  * baseTexColor.g + 0.0721 * baseTexColor.b;
				luminanceColor = fixed3(luminance, luminance, luminance);
				fixed3 matelColor = baseTexColor.rgb;
				matelColor = lerp(luminanceColor, matelColor, _MatelSaturation);
				baseTexColor.rgb = lerp(baseTexColor.rgb, matelColor, lightTexColor.b);

				//??????????????????????????????
				baseTexColor.rgb *= _Brightness;
				fixed3 avgColor = fixed3(0.5, 0.5, 0.5);
				baseTexColor.rgb = lerp(avgColor, baseTexColor.rgb, _Contrast);

				
				//????????????
				#if ENABLE_MODIFYCHANNEL
				fixed3 shadowColor =  baseTexColor.rgb > 0.5 ? 1.0 - (2.0 - baseTexColor.rgb * 2) * (1.0 - lightTexColor.g) : 2.0 * baseTexColor.rgb * lightTexColor.g;
				fixed fixedShadow =  saturate(saturate(lightTexColor.g - _FixedShadowCutOff) * 10);
				fixedShadow = saturate(fixedShadow  + saturate(1 - _FixedShadowMult * 3));
				baseTexColor.rgb = lerp(shadowColor,  baseTexColor.rgb, fixedShadow);
				#endif

				//?????????
				fixed3 diffuse = 0;
				fixed threshold = (halfLambert + lightTexColor.g) * 0.5;
				fixed ramp = saturate(_ShadowAera  - threshold);
				ramp =  smoothstep(0, _ShadowSmooth, ramp);
				diffuse = lerp(baseTexColor.rgb * _MainColor, baseTexColor.rgb * _ShadowColor * (1 - _ShadowMult), ramp);
				fixed3 diffuseLightAera = diffuse * (1 - ramp);

				//??????
				fixed3 surfaceLight = 0;
				fixed surfaceLightIf = 0;
				surfaceLightIf = lightTexColor.r > 0.05 ? 1 : 0;
				surfaceLight = step(_SurfaceRage,saturate(pow(NdotV,1 - lightTexColor.b))) * _SurfaceMult  * diffuseLightAera;
				surfaceLightIf *= 1 - ramp;
				surfaceLight *= surfaceLightIf;

				//??????
				fixed3 specular = 0;
				half SpecularSize = pow(NdotH, _SpecularGloss);
				fixed specularMask = lightTexColor.b;
				//fixed specularMask =  lightTexColor.b > 0.5 ? lightTexColor.b : 0;
				if (SpecularSize >= 1 - specularMask * _SpecularAera)
				{
					specular = _SpecularMulti * (lightTexColor.r) * diffuseLightAera;//????????????
				}
				SpecularSize = pow(NdotV, _SpecularGloss);
				if (SpecularSize >= 1 - specularMask * _SpecularAera)
				{
					specular = _SpecularMulti * (lightTexColor.r) * diffuseLightAera;//???????????????????????????
				}
				fixed specularIf = lightTexColor.r > 0.1 ? 1 : 0;
				specular *= specularIf;

				//?????????
				fixed f =  1.0 - saturate(dot(normalize(viewDir), worldNormal));
				fixed rim = smoothstep(_RimMin, _RimMax, f);
				rim = smoothstep(0, _RimSmooth, rim);
				fixed3 rimColor = rim * _RimColor.rgb * diffuse * _RimColor.a * (1 - luminance / 2);

				//????????????
				col.rgb = (diffuse + max(rimColor, max(surfaceLight, specular))) * lightColor;



				//????????????
				fixed BloomFade = 1 - luminance;
				fixed rimBloom = pow (f, _RimBloomExp) * _RimBloomMulti * NdotL; //???rim bloom
				col.a = (baseTexColor.a * _BloomMulti +  rimBloom) * BloomFade;


				//?????????
				col.rgb += baseTexColor.rgb * _Emission * (1 - lightTexColor.a);
				col.a += _Emission * (1 - lightTexColor.a);

#if ENABLE_CUTOFF
				//????????????
				fixed4 maskTex = tex2D(_MaskTex, i.uvMask);
				col.rgb = lerp(col.rgb, _EdgeColor, maskTex.r - _CutOff > _EdgeLength ? 0 : 1);
				clip(maskTex.r - _CutOff);
#endif

				float2 clipPos = float2(i.vertex.y, i.vertex.x);
				float uvX = i.uv.x * _NoiseTex_ST.x * _Scale - (_Time.x * _SpeedX);
				uvX = uvX + clipPos.y * _WorldOffsetX;
				float uvY = i.uv.y * _NoiseTex_ST.y * _Scale - (_Time.x * _SpeedY);
				uvY += clipPos.x * _WorldOffsetY;
				float2 uv = float2(uvX, uvY);
				fixed4 noise = tex2D(_NoiseTex, uv) * _NoiseScale;
			 	fixed4 fadeTex = (tex2D(_FadeTex, i.uv) + _FadeOffset) * _FadeScale;
				fixed rimFade =  fadeTex.r;
				rimFade -= noise.r + _NoiseCutOff;
				clip(rimFade - 0.01);
				
				fixed texturedRim = saturate(rimFade * _Opacity);
				fixed extraRim = (saturate((_Edge + rimFade) * _Opacity) - texturedRim) * _FadeBrightness;
				col = (col * texturedRim) + (_ColorR * extraRim);
				col = facing > 0 ? col: _BackColor * texturedRim + (_ColorR * extraRim);

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
#if ENABLE_SMOOTH_NORMAL
                float3 smoothNormal : TANGENT;
#endif
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

#if ENABLE_CUTOFF
				o.uvMask = TRANSFORM_TEX(v.uv, _MaskTex);
#endif
				o.vertColor = v.vertColor;
                //o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal * _OutlineWidth,1) );
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET 
			{
#if ENABLE_CUTOFF
				//????????????
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
                outLineFade /= 80; //?????????????????????????????????
				v.vertex.xyz += v.normal * _OutlineWidth * outLineFade;//????????????

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

			Cull Off
			
			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
            #include "AutoLight.cginc"
			
            #pragma multi_compile_fwdbase
            #pragma multi_compile _ ENABLE_MODIFYCHANNEL
			#pragma multi_compile _ ADD_FACE_LIGHT
			#pragma multi_compile _ ENABLE_CUTOFF
 
            sampler2D _MainTex; 
			float4 _MainTex_ST;
            sampler2D _SpeShaTex; 
			float4 _SpeShaTex_ST;

			fixed3 _MainColor;
			fixed3 _ShadowColor;
			fixed _ShadowSmooth;
			fixed _ShadowAera;
			fixed _ShadowMult;
			//fixed _LimGScale;

			fixed _SurfaceRage;
			fixed _SurfaceMult;
			
			fixed _SpecularAera;
        	fixed _SpecularMulti;
			fixed _SpecularGloss;

           	// half _rimPower;
			// fixed3 _rimColorRight;
			// fixed _rimIntensity;
			// fixed _rimSmooth;
			fixed _RimMin;
			fixed _RimMax;
			fixed4 _RimColor;
			fixed _RimSmooth;
			//fixed _AutoLigDir;
			//fixed4 _LigDir;
			half _RimBloomExp;
			fixed _RimBloomMulti;
            fixed _BloomMulti;
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

			sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
			sampler2D _FadeTex;
			float _FadeScale;
			float _FadeOffset;
			float4 _FadeTex_ST;
            half _Scale, _Opacity, _Edge;
            fixed4 _Color, _ColorR, _BackColor;
            half _FadeBrightness, _SpeedX, _SpeedY;
            half _Outline;
            half _OutlineZ;
            half _RimPower;
            fixed _NoiseCutOff;
			half _NoiseScale;
			half _WorldOffsetX;
			half _WorldOffsetY;


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
				float4 vertex : TEXCOORD5;
			};

	
			
			v2f vert (a2v v)
			{
				v2f o = (v2f)0;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.vertex = v.vertex;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				UNITY_TRANSFER_FOG(o,o.pos);
#if ENABLE_CUTOFF
				o.uvMask = TRANSFORM_TEX(v.uv, _MaskTex);
#endif
				return o;
			}
			
			fixed4 frag (v2f i,  fixed facing : VFACE) : SV_Target
			{
				//????????????
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

				//????????????
				fixed4 col = 0;
				fixed4 baseTexColor = tex2D (_MainTex, i.uv);
				fixed4 lightTexColor = tex2D (_SpeShaTex, i.uv);

			

				//?????????????????????
				fixed luminance = 0.2125 * baseTexColor.r + 0.7154  * baseTexColor.g + 0.0721 * baseTexColor.b;
				fixed3 luminanceColor = fixed3(luminance, luminance, luminance);
				fixed3 color = baseTexColor.rgb;
				baseTexColor.rgb = lerp(luminanceColor, color, _Saturation);

				//?????????????????????
				luminance = 0.2125 * baseTexColor.r + 0.7154  * baseTexColor.g + 0.0721 * baseTexColor.b;
				luminanceColor = fixed3(luminance, luminance, luminance);
				fixed3 matelColor = baseTexColor.rgb;
				matelColor = lerp(luminanceColor, matelColor, _MatelSaturation);
				baseTexColor.rgb = lerp(baseTexColor.rgb, matelColor, lightTexColor.b);

				//??????????????????????????????
				baseTexColor.rgb *= _Brightness;
				fixed3 avgColor = fixed3(0.5, 0.5, 0.5);
				baseTexColor.rgb = lerp(avgColor, baseTexColor.rgb, _Contrast);

				
				//????????????
				#if ENABLE_MODIFYCHANNEL
				fixed3 shadowColor =  baseTexColor.rgb > 0.5 ? 1.0 - (2.0 - baseTexColor.rgb * 2) * (1.0 - lightTexColor.g) : 2.0 * baseTexColor.rgb * lightTexColor.g;
				fixed fixedShadow =  saturate(saturate(lightTexColor.g - _FixedShadowCutOff) * 10);
				fixedShadow = saturate(fixedShadow  + saturate(1 - _FixedShadowMult * 3));
				baseTexColor.rgb = lerp(shadowColor,  baseTexColor.rgb, fixedShadow);
				#endif

				//?????????
				fixed3 diffuse = 0;
				fixed threshold = (halfLambert + lightTexColor.g) * 0.5;
				fixed ramp = saturate(_ShadowAera  - threshold);
				ramp =  smoothstep(0, _ShadowSmooth, ramp);
				diffuse = lerp(baseTexColor.rgb * _MainColor, baseTexColor.rgb * _ShadowColor * (1 - _ShadowMult), ramp);
				fixed3 diffuseLightAera = diffuse * (1 - ramp);

				//??????
				fixed3 surfaceLight = 0;
				fixed surfaceLightIf = 0;
				surfaceLightIf = lightTexColor.r > 0.05 ? 1 : 0;
				surfaceLight = step(_SurfaceRage,saturate(pow(NdotV,1 - lightTexColor.b))) * _SurfaceMult  * diffuseLightAera;
				surfaceLightIf *= 1 - ramp;
				surfaceLight *= surfaceLightIf;

				//??????
				fixed3 specular = 0;
				half SpecularSize = pow(NdotH, _SpecularGloss);
				fixed specularMask = lightTexColor.b;
				//fixed specularMask =  lightTexColor.b > 0.5 ? lightTexColor.b : 0;
				if (SpecularSize >= 1 - specularMask * _SpecularAera)
				{
					specular = _SpecularMulti * (lightTexColor.r) * diffuseLightAera;//????????????
				}
				SpecularSize = pow(NdotV, _SpecularGloss);
				if (SpecularSize >= 1 - specularMask * _SpecularAera)
				{
					specular = _SpecularMulti * (lightTexColor.r) * diffuseLightAera;//???????????????????????????
				}
				fixed specularIf = lightTexColor.r > 0.1 ? 1 : 0;
				specular *= specularIf;

				//?????????
				fixed f =  1.0 - saturate(dot(normalize(viewDir), worldNormal));
				fixed rim = smoothstep(_RimMin, _RimMax, f);
				rim = smoothstep(0, _RimSmooth, rim);
				fixed3 rimColor = rim * _RimColor.rgb * diffuse * _RimColor.a * (1 - luminance / 2);

				//????????????
				col.rgb = (diffuse + max(rimColor, max(surfaceLight, specular))) * lightColor;



				//????????????
				fixed BloomFade = 1 - luminance;
				fixed rimBloom = pow (f, _RimBloomExp) * _RimBloomMulti * NdotL; //???rim bloom
				col.a = (baseTexColor.a * _BloomMulti +  rimBloom) * BloomFade;


				//?????????
				col.rgb += baseTexColor.rgb * _Emission * (1 - lightTexColor.a);
				col.a += _Emission * (1 - lightTexColor.a);

#if ENABLE_CUTOFF
				//????????????
				fixed4 maskTex = tex2D(_MaskTex, i.uvMask);
				col.rgb = lerp(col.rgb, _EdgeColor, maskTex.r - _CutOff > _EdgeLength ? 0 : 1);
				clip(maskTex.r - _CutOff);
#endif

				float2 clipPos = float2(i.vertex.y, i.vertex.x);
				float uvX = i.uv.x * _NoiseTex_ST.x * _Scale - (_Time.x * _SpeedX);
				uvX = uvX + clipPos.y * _WorldOffsetX;
				float uvY = i.uv.y * _NoiseTex_ST.y * _Scale - (_Time.x * _SpeedY);
				uvY += clipPos.x * _WorldOffsetY;
				float2 uv = float2(uvX, uvY);
				fixed4 noise = tex2D(_NoiseTex, uv) * _NoiseScale;
			 	fixed4 fadeTex = (tex2D(_FadeTex, i.uv) + _FadeOffset) * _FadeScale;
				fixed rimFade =  fadeTex.r;
				rimFade -= noise.r + _NoiseCutOff;
				clip(rimFade - 0.01);
				
				fixed texturedRim = saturate(rimFade * _Opacity);
				fixed extraRim = (saturate((_Edge + rimFade) * _Opacity) - texturedRim) * _FadeBrightness;
				col = (col * texturedRim) + (_ColorR * extraRim);
				col = facing > 0 ? col: _BackColor * texturedRim + (_ColorR * extraRim);

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
                outLineFade /= 80; //?????????????????????????????????
				v.vertex.xyz += v.normal * _OutlineWidth * outLineFade;//????????????

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
