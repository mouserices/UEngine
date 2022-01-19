Shader "YanJia/Scene/Normal"
{
	Properties 
	{
		_Color ("Color Tint", Color) = (1, 1, 1, 1)
		_MainTex ("Main Tex", 2D) = "white" {}
		_BumpMap ("Normal Map", 2D) = "white" {}
		_BumpScale("Normal Scale",Range(0, 2)) = 1
		//_Roughness("Roughness", Range(-0.5 ,0.5)) = 0.5
		_DiffusePower ("Diffuse Power", Range(0,1)) = 1
		_DiffuseColor("Diffuse Color", Color) = (0,0,0)
		_SpecularColor ("Specular", Color) = (1, 1, 1, 1)
		_SpecularPower ("Specular Power", Range(0, 1)) = 1
		_Gloss ("Gloss", Range(8.0, 256)) = 20
		_FresnelColor ("Fresnel Color", Color) = (1, 1, 1)
		_FresnelPower ("Fresnel Power", Range(0,1)) = 0.03
		_FresnelScale ("Fresnel Scale", Range(0, 0.1)) = 1
		_Brightness("Brightness", Range(0,2)) = 1
		_Contrast("Contrast", Range(0, 2)) = 1
		_Saturation("Saturation", Range(0,2)) = 1
        _LightmapScale("Lightmap Scale", Range(0.1, 5)) = 1
		_BloomPower ("Bloom Power", Range(0,1)) = 0.1
		[Toggle(ENABLE_UE4)]_ISUE4Resource("Is UE4 Resource", Float) = 0
		[Toggle(ENABLE_CUSTOM_FOG)] _EnableCustomFog ("Enable Custom fog", Float) = 0
	}

	SubShader 
	{
		Pass 
		{ 
			Tags {"LightMode"="ForwardBase"}
		
			CGPROGRAM
			
			#pragma multi_compile_fwdbase
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#pragma multi_compile _ ENABLE_UE4  
			#pragma multi_compile _ ENABLE_CUSTOM_FOG

			#include "../CustomShader.cginc"
			
			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BumpMap;
			float4 _BumpMap_ST;
			fixed _BumpScale;
			fixed4 _SpecularColor;
			half _SpecularPower;
			half _Gloss;
			fixed3 _FresnelColor;
			half _FresnelPower;
			half _FresnelScale;
			half _Brightness;
			half _Contrast;
			half _Saturation;
			half _LightmapScale;
			fixed _BloomPower;
			fixed _Roughness;
			fixed _DiffusePower;
			fixed3 _DiffuseColor;
			
			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 texcoord : TEXCOORD0;
			#ifdef LIGHTMAP_ON
				float2 uvLM : TEXCOORD1;
			#endif
			};
			
			struct v2f 
			{
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
				float4 TtoW0 : TEXCOORD1;  
				float4 TtoW1 : TEXCOORD2;  
				float4 TtoW2 : TEXCOORD3; 
				UNITY_FOG_COORDS(4)
			#ifdef LIGHTMAP_ON
				float2 uvLM : TEXCOORD5;
			#endif
			};

			// half3 ComputeNormal(in half3 normalColor, in half3x3 TtoW)
			// {
			// 	normalColor.g = 1.0f - normalColor.g;//UE4法线贴图相比Unity，反转了绿通道
			// 	normalColor = 2.0f * (normalColor - 0.5f);
			// 	return normalize(mul(normalColor, TtoW));
			// }

			fixed3 UnpackUE4Normal(fixed3 normalColor)
			{
				normalColor.g = 1.0f - normalColor.g;
				normalColor = 2.0f * (normalColor - 0.5f);
				return normalize(normalColor);
			}

			// half DisneyDiffuse(half NdotV, half NdotL, half LdotH, half perceptualRoughness)
			// {
			// 	half fd90 = 0.5 + 2 * LdotH * LdotH * perceptualRoughness;
			// 	half lightScatter   = (1 + (fd90 - 1) * Pow5(1 - NdotL));
			// 	half viewScatter    = (1 + (fd90 - 1) * Pow5(1 - NdotV));
			// 	return lightScatter * viewScatter;
			// }
			
			v2f vert(a2v v) 
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				o.pos = UnityObjectToClipPos(v.vertex);
				
				o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				o.uv.zw = v.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
				
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;  
				float3 worldNormal = UnityObjectToWorldNormal(v.normal);  
				float3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);  
				float3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w; 
				
				o.TtoW0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);
				o.TtoW1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);
				o.TtoW2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);
				
			#ifdef LIGHTMAP_ON
				o.uvLM = v.uvLM.xy * unity_LightmapST.xy + unity_LightmapST.zw;
			#endif

				UNITY_TRANSFER_FOG(o,o.pos);

				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{	
				half3 worldPos = half3(i.TtoW0.w, i.TtoW1.w, i.TtoW2.w);
				fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
				fixed3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));

			#ifdef ENABLE_UE4
				fixed3 normalTangent = UnpackUE4Normal(tex2D(_BumpMap, i.uv));
			#else
				fixed3 normalTangent = UnpackNormal(tex2D(_BumpMap, i.uv)); 
			#endif
				normalTangent.xy *= _BumpScale;
				normalTangent.z = sqrt(1.0 - saturate(dot(normalTangent.xy,normalTangent.xy)));
				fixed3 worldNormal = normalize(half3(dot(i.TtoW0.xyz,normalTangent), dot(i.TtoW1.xyz,normalTangent), dot(i.TtoW2.xyz,normalTangent)));

				
				fixed3 albedo = tex2D(_MainTex, i.uv).rgb * _Color.rgb;
				
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;

				fixed3 halfDir = normalize(lightDir + viewDir);
				fixed3 specular = _LightColor0.rgb * _SpecularColor.rgb * pow(max(0, dot(worldNormal, halfDir)), _Gloss) * _SpecularPower;
				
				//fixed NdotV = max(0, dot(bump, viewDir));
				//fixed NdotL = max(0, dot(bump, lightDir));
				//fixed LdotH = max(0, dot(lightDir, halfDir));
				//fixed3 diffuse = _LightColor0.rgb * albedo * DisneyDiffuse1(NdotV, NdotL, LdotH, _Roughness) * _DiffusePower;
				fixed3 diffuse = _LightColor0.rgb * lerp(_DiffuseColor, albedo, max(0, dot(worldNormal, lightDir)))  * _DiffusePower;
				//fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(bump, lightDir))  * _DiffusePower;

				fixed4 col = 0;
				#ifdef LIGHTMAP_ON
					#ifdef ENABLE_NOLIM //临时屏蔽lightmap
						col.rgb = albedo + diffuse + specular;
					#else
						fixed3 lightmap = DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uvLM.xy)) / _LightmapScale;
						col.rgb = diffuse + albedo * lightmap +  specular;
					#endif
				#else
					col.rgb = ambient + diffuse + specular;
				#endif

				fixed p = 1 - dot(viewDir, worldNormal);
				half fresnel = _FresnelScale + (1 - _FresnelScale) *p *p *p *p *p;
				col.rgb = lerp(col.rgb, _FresnelColor, saturate(fresnel * _FresnelPower));

				col.rgb *= _Brightness;
				half gray = 0.2125 * col.r + 0.7154 * col.g + 0.0721 * col.b;
				half3 grayColor = fixed3(gray, gray, gray);
				col.rgb = lerp(grayColor, col.rgb, _Saturation);
				half3 avgColor = half3(0.5, 0.5, 0.5);
				col.rgb = lerp(avgColor, col.rgb, _Contrast);
				col.a = _BloomPower;

		#ifdef ENABLE_CUSTOM_FOG
				col = GetCustomFog(worldPos, col); 
		#else
				UNITY_APPLY_FOG(i.fogCoord, col);
		#endif
				return col;
			}
			ENDCG
		}
	} 
}
