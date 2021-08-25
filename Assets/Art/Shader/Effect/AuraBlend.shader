Shader "YanJia/Effect/AuraBlend"
{
	Properties
	{
		_MainTex ("MainTex", 2D) = "white" {}
        _SpeShaTex ("SpeShaTex", 2D) = "white" {}

		[Space(20)]
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
        _SpecularMulti ("Specular Mult", Range(0, 5)) = 0.9
		_SpecularGloss("Sprecular Gloss", Range(0.001, 32)) = 4

		[Space(20)]
		_OutlineWidth ("Outline Width", Range(0, 1)) = 0.24
        _OutlineLightness ("Outline Lightness", Range(0, 1)) = 1
        _OutLineColor ("OutLine Color", Color) = (0.5,0.5,0.5,1)
        _rimPower ("Rim Power", Range(0, 20)) = 20
        _rimColorRight ("Rim Color", Color) = (1,1,1,1)
		_rimIntensity ("Rim Intensity", Range(0,1)) = 0.2
		_AutoLigDir ("AutoLigDir", Float ) = 0
		_RimBloomExp("Rim Bloom Exp", Range(0,10)) = 3.3
        _BloomPower ("Bloom Power", Range(0,1)) = 0.3
		_Alpha ("Alpha", Range(0,1)) = 1

		[Space(20)]
		_Saturation("Saturation", Range(0, 1)) = 1
		_MatelSaturation("Matel Saturation", Range(0, 3)) = 2

		[Space(20)]
        [KeywordEnum(All, R, G, B)] _Chanel ("Debuge Chanel",Float) = 0

		[Space(10)]
		[Toggle(ENABLE_MODIFYCHANNEL)]_EnableModifyChannel("Enable Fixed Shadow", Float) = 0
		[Toggle(ENABLE_SMOOTH_NORMAL)]_EnableSmoothNormal("Enable Smooth Normal", Float) = 0
		_FixedShadowCutOff("Fixed Shadow CutOff", Range(0,0.4)) = 0.31
		_FixedShadowMult("Fixed Shadow Mult", Range(0,1)) = 0.2
        
		[Space(30)]
		_MaskTex("Mask Texture", 2D) = "white" {}
		_MaskCutout("Mask Cutout", Range(0, 1.0)) = 0
		_MaskScale("Mask Scale", Range(0, 1.0)) = 1
		_Scale("Mask Tiling", Range(0.0, 5)) = 1
		_SpeedX("Mask Speed X", Range(-10, 10)) = 0
		_SpeedY("Mask Speed Y", Range(-10, 10)) = 3.0
		_Blend("Blend", Range(0, 1.0)) = 0
        _EdgeColor("Edge Color", Color) = (1,1,0)
		_EdgeColorR("Rim Edge Color", Color) = (1,1,0)
		_EdgeColorBlend("Edge Color Blend", Range(0.0, 1)) = 0.5
		_EdgeColorCutout("Edge Color Cutout", Range(0.0, 1)) = 0
		_EdgeColorScale("Edge Color Scale", Range(0.0, 10)) = 1
        _EdgeLength("Edge Length", Range(0.0, 0.1)) = 0.012
		_PostionTop("Position Top", Float) = 2
		_PostionButtom("Postion Buttom", Float) = 0
	}

	SubShader
	{
		Tags { "RenderType"="Transparent"  "Queue"="Transparent" "LightMode" = "ForwardBase"}

		// Pass
		// {
		// 	ColorMask 0
		// }

		Pass
		{
			Blend  SrcAlpha OneMinusSrcAlpha
            ZWrite Off 
			ColorMask RGB
            
			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
            #include "AutoLight.cginc"
			
            #pragma multi_compile_fwdbase
            #pragma shader_feature _CHANEL_ALL _CHANEL_R _CHANEL_G _CHANEL_B
            #pragma multi_compile _ ENABLE_MODIFYCHANNEL
 
            sampler2D _MainTex; 
			float4 _MainTex_ST;
            sampler2D _SpeShaTex; 
			float4 _SpeShaTex_ST;

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

           	half _rimPower;
			fixed3 _rimColorRight;
			fixed _rimIntensity;
			fixed _AutoLigDir;
			fixed4 _LigDir;
			half _RimBloomExp;
            fixed _BloomPower;

			fixed _ShadowRange;
			fixed _SpecularRange;
			fixed _SpecularMult;
			fixed _ShadowIntensity;
			fixed _Saturation;
			fixed _MatelSaturation;
			fixed _FixedShadowCutOff;
			fixed _FixedShadowMult;
            fixed _Alpha;

			sampler2D _MaskTex;
        	float4 _MaskTex_ST;
			fixed3 _EdgeColor;
			fixed3 _EdgeColorR;
			half _EdgeColorBlend;
			fixed _EdgeColorCutout;
			fixed _EdgeColorScale;
			fixed _EdgeLength;
			fixed _Blend;
			float  _PostionTop;
			float _PostionButtom;
			fixed _MaskCutout;
			fixed _MaskScale;
			fixed _Scale;
			fixed _SpeedX, _SpeedY;



			struct a2v
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float3 vertexColor : COLOR;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 vertexColor : COLOR;
				float2 uv : TEXCOORD0;	
				float3 worldNormal : TEXCOORD1;
				float3 worldPos : TEXCOORD2; 
				UNITY_FOG_COORDS(3)
				float2 uvMask : TEXCOORD4;
			};

	
			
			v2f vert (a2v v)
			{
				v2f o = (v2f)0;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.vertexColor = v.vertexColor;
				o.uvMask = TRANSFORM_TEX(v.uv, _MaskTex);
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//计算变量
				fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
				fixed3 worldNormal = normalize(i.worldNormal);
				fixed3 worldLightDir = _AutoLigDir > 0.5 ? _LigDir : normalize(_WorldSpaceLightPos0.xyz);
				fixed3 lightColor = _LightColor0.rgb;
				fixed3 halfDir = normalize(worldLightDir + viewDir);
				fixed NdotL = max(0, dot(worldNormal, worldLightDir));
				fixed NdotV = max(0, dot(worldNormal, viewDir));
				fixed NdotH = max(0, dot(worldNormal, halfDir));
				fixed halfLambert = dot(worldNormal, worldLightDir) * 0.5 + 0.5;

				//贴图采样
				fixed4 col = 0;
				fixed4 baseTexColor = tex2D (_MainTex, i.uv);
				fixed3 lightTexColor = tex2D (_SpeShaTex, i.uv).rgb;

				//lightTexColor.g = (lightTexColor.g + 0.2) / 1.5;//贴图画的不好，采样修正
				_ShadowSmooth /= 10;
				
				
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

				
				//阴影残留
				#if ENABLE_MODIFYCHANNEL
				fixed3 shadowColor = baseTexColor.rgb * lightTexColor.g;
				fixed fixedShadow = lightTexColor.g - _FixedShadowCutOff > 0 ? 1 : 0;
				fixedShadow = saturate(fixedShadow + (1 - _FixedShadowMult));
				baseTexColor.rgb = lerp(shadowColor, baseTexColor.rgb, fixedShadow);
				#endif

				//漫反射
				fixed3 diffuse = 0;
				//fixed threshold = (halfLambert * 0.5 + (lighftTexColor.g * _LimGScale ) * 0.5);
				fixed threshold = (halfLambert + lightTexColor.g) * 0.5;
				fixed ramp = smoothstep(0, _ShadowSmooth, _ShadowAera  - threshold);
				diffuse = lerp(baseTexColor.rgb , baseTexColor.rgb * _ShadowColor * (1 - _ShadowMult), ramp);
				fixed3 diffuseLightAera = diffuse * (1 - ramp);

				//流光
				fixed3 surfaceLight = 0;
				fixed surfaceLightIf = 0;
				surfaceLightIf = lightTexColor.r > 0.05 ? 1 : 0;
				surfaceLight = step(_SurfaceRage,saturate(pow(NdotV,1 - lightTexColor.b))) * _SurfaceMult  * diffuseLightAera;
				surfaceLightIf *= 1 - ramp;
				//surfaceLight =  surfaceLight * (lightTexColor.r + 0.15) * 3;
				surfaceLight *= surfaceLightIf;

				//高光
				fixed3 specular = 0;
				half SpecularSize = pow(NdotH, _SpecularGloss);
				fixed specularMask = lightTexColor.b;
				//fixed specularMask =  lightTexColor.b > 0.5 ? lightTexColor.b : 0;
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
				fixed3 rim = 0;
				fixed f =  1.0 - saturate(dot(normalize(viewDir), worldNormal));
				rim = _rimColorRight.rgb * pow (f, _rimPower) * _rimIntensity;

				//合并计算
				col.rgb = (diffuse + max(surfaceLight, specular)) * lightColor + rim;

                #if _CHANEL_R
                    col.rgb = fixed3(lightTexColor.r,0,0);
                #endif
                #if _CHANEL_G
                    col.rgb = fixed3(0,lightTexColor.g,0);
                #endif
                #if _CHANEL_B
                    col.rgb = fixed3(0,0,lightTexColor.b);
                #endif

				col.a = baseTexColor.a * _Alpha;

				//溶解消失
				float2 uv =  float2(i.worldPos.x * _MaskTex_ST.x * _Scale - (_Time.x * _SpeedX), i.worldPos.y * _MaskTex_ST.y * _Scale - (_Time.x * _SpeedY));
				fixed4 maskTex = tex2D(_MaskTex, uv);
				half positionOffset = 0;
				if(_PostionTop> _PostionButtom)
				{
					positionOffset = i.worldPos.y - lerp(_PostionTop, _PostionButtom, _Blend);
				}
				else
				{
					positionOffset = -i.worldPos.y + lerp(_PostionTop, _PostionButtom, _Blend);
				}
				//col.rgb = lerp(col.rgb, _EdgeColor, maskTex.r + offset - _EdgeLength> 0 ? 0: 1);
				fixed maskOffset = (maskTex.r - _MaskCutout) * _MaskScale;
				fixed3 auraColor = lerp(_EdgeColor, _EdgeColorR, (maskTex.r - _EdgeColorCutout) * _EdgeColorScale * _EdgeColorBlend);
				col.rgb = lerp(col.rgb, auraColor, positionOffset + _EdgeLength - maskOffset> 0 ? 1 : 0);
				col.a = max(0,  positionOffset  + maskOffset< 0 ? col.a : 0);
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}

// 		Pass 
// 		{
// 			ColorMask RGB
//             Cull Front
//             Blend  SrcAlpha OneMinusSrcAlpha
//             ZWrite Off 
            
//             CGPROGRAM
//             #pragma vertex vert
//             #pragma fragment frag
//             #include "UnityCG.cginc"
// 			#pragma multi_compile _ ENABLE_SMOOTH_NORMAL

//             fixed _OutlineLightness;
//             fixed _OutlineWidth;
//             fixed4 _OutLineColor;
// 			fixed _Alpha;

// 			sampler2D _MaskTex;
//         	float4 _MaskTex_ST;
// 			fixed _Blend;

//             struct a2v 
// 			{
//                 float4 vertex : POSITION;
//                 float3 normal : NORMAL;
//                 float2 uv : TEXCOORD0;
//                 float4 vertColor : COLOR;
// #if ENABLE_SMOOTH_NORMAL
//                 float3 smoothNormal : TANGENT;
// #endif
//             };

//             struct v2f
// 			{
//                 float4 pos : SV_POSITION;
// 				float2 uvMask : TEXCOORD0;
//             };

//             v2f vert (a2v v) 
// 			{
//                 v2f o;
// 				UNITY_INITIALIZE_OUTPUT(v2f, o);
//                 float outLineFade = 1;

//                 float4x4 P = unity_CameraProjection;
// 				float fov =  atan(1 / P._m11) * 2;
// 				float fovScale = degrees(fov) / 40;
//                 outLineFade *= pow(fovScale, 0.5);

//                 float3 posWorld = mul(unity_ObjectToWorld, v.vertex);
//                 float distance = length(_WorldSpaceCameraPos.xyz - posWorld);
//                 //outLineFade *= pow(distance / 20, 1) * 6;
//                 outLineFade *= pow(distance, 0.5);
//                 //outLineFade = max(1, outLineFade);

//                 float4 pos = mul( UNITY_MATRIX_MV, v.vertex);
                
// #if ENABLE_SMOOTH_NORMAL
//                 float3 normal = mul( (float3x3)(UNITY_MATRIX_IT_MV), v.smoothNormal);
// #else                              
//                 float3 normal = mul( (float3x3)UNITY_MATRIX_IT_MV, v.normal);
// #endif
//                 normal.z = -0.5;
//                 outLineFade /= 100; 
//                 pos = pos + float4(normalize(normal),0) * _OutlineWidth * outLineFade * v.vertColor.a;
//                 o.pos = mul(UNITY_MATRIX_P, pos);
// 				o.uvMask = TRANSFORM_TEX(v.uv, _MaskTex);
//                 //o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal * _OutlineWidth,1) );
//                 return o;
//             }

//             fixed4 frag(v2f i) : SV_TARGET 
// 			{
// 				//溶解消失
// 				fixed4 maskTex = tex2D(_MaskTex, i.uvMask);
// 				fixed alpha = max(0, maskTex.r - _Blend > 0 ? 1 : 0);
//                 return fixed4(_OutLineColor.rgb * _OutlineLightness, _Alpha * alpha) ;
//             }

//             ENDCG
//         }

        Pass 
        {
			ZWrite Off 
        	COLORMASK A
			//BlendOp Max
			Blend  SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed _BloomPower;

           struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 worldPos : TEXCOORD0; 
			};

			v2f vert (a2v v)
			{
				v2f o = (v2f)0;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			sampler2D _MaskTex;
        	float4 _MaskTex_ST;
			fixed _EdgeLength;
			fixed _Blend;
			float  _PostionTop;
			float _PostionButtom;
			fixed _MaskCutout;
			fixed _MaskScale;
			fixed _Scale;
			fixed _SpeedX, _SpeedY;

			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = 1;
				float2 uv =  float2(i.worldPos.x * _MaskTex_ST.x * _Scale - (_Time.x * _SpeedX), i.worldPos.y * _MaskTex_ST.y * _Scale - (_Time.x * _SpeedY));
				fixed4 maskTex = tex2D(_MaskTex, uv);
				half positionOffset = 0;
				if(_PostionTop> _PostionButtom)
				{
					positionOffset = i.worldPos.y - lerp(_PostionTop, _PostionButtom, _Blend);
				}
				else
				{
					positionOffset = -i.worldPos.y + lerp(_PostionTop, _PostionButtom, _Blend);
				}
				//col.rgb = lerp(col.rgb, _EdgeColor, maskTex.r + offset - _EdgeLength> 0 ? 0: 1);
				fixed maskOffset = (maskTex.r - _MaskCutout) * _MaskScale;
				col.a = max(0,  positionOffset  + maskOffset< 0 ? _BloomPower : 0);
				col.a = max(0,  positionOffset  - maskOffset> 0 ? col.a : 0);
				return col;
			}
			ENDCG
        }
	}

	FallBack Off
}
