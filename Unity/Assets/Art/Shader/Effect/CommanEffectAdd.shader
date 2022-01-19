Shader "YanJia/Effect/CommanEffectAdd"
{
Properties
	{
		[Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull Mode", Float) = 2
		_Color (" Color", Color) = (1,1,1,1)
		_MainTex ("Main Texture", 2D) = "white" {}
        _MaskTex("Mask Texture", 2D) = "white" {}
		_AlphaTex("Alpha Texture", 2D) = "white" {}
        _CutOff("Cut Off", Range(0, 1.0)) = 0.5
        _EdgeColor("Edge Color", Color) = (1,1,0)
        _EdgeLength("Edge Length", Range(0.0, 1)) = 0.0
        _EdgeBloomPower("Edge Bloom Power", Range(0.0, 1)) = 1
        [Space(20)]
        _MainTexX("MainTex Move X", Range (-3, 3)) = 0
        _MainTexY("MainTex Move Y", Range (-3, 3)) = 0
		_MaskTexX("MaskTex Move X", Range (-3, 3)) = 0
        _MaskTexY("MaskTex Move Y", Range (-3, 3)) = 0
		_AlphaTexX("AlphaTex Move X", Range (-3, 3)) = 0
        _AlphaTexY("AlphaTex Move Y", Range (-3, 3)) = 0
        [Space(20)]
        _RimExp ("Rim Exp", Range(0, 10)) = 20
        _RimColor ("Rim Color", Color) = (1,1,1)
		_RimPower ("Rim Power", Range(0,3)) = 0.2
		_RimAlpha("Rim Alpha", Range(0,1)) = 1
        [Space(20)]
        _Alpha("Alpha", Range (0, 1)) = 1
		_BloomPower ("Bloom Power", Range(0,1)) = 0.5
		_Emission("Emission", Range(0,5)) = 0

		[Space(20)]
		[Toggle(ENABLE_ALPHACLIP)]_EnableAlphaClip("Enable Alpha CutOff", Float) = 0

		[Space(10)]
        [Toggle(ENABLE_UICLIP)] _UIClip ("Enable UI Clip", Float) = 0
		//_PanelSizeX("_PanelSizeX", Range(0.000000,300.000000)) = 300.000000
		//_PanelSizeY("_PanelSizeY", Range(0.000000,300.000000)) = 300.000000
		//_PanelCenterAndSharpness("_PanelCenterAndSharpness", Vector) = (0,0,300,300)
	}

	Category 
	{
		CGINCLUDE
		#pragma multi_compile_particles
		#pragma multi_compile_fog
		#pragma multi_compile _ ENABLE_ALPHACLIP
		#pragma multi_compile _ ENABLE_UICLIP
		#include "UnityCG.cginc"
		
		fixed4 _Color;
        sampler2D _MainTex;
        float4 _MainTex_ST;
        sampler2D _MaskTex;
        float4 _MaskTex_ST;
		sampler2D _AlphaTex;
        float4 _AlphaTex_ST; 
        fixed _BloomPower;
        fixed3 _EdgeColor;
        fixed _EdgeLength;
        fixed _CutOff;
        fixed _EdgeBloomPower;
        float _MainTexX;
        float _MainTexY;
		float _MaskTexX;
        float _MaskTexY;
		float _AlphaTexX;
		float _AlphaTexY;
        fixed  _Alpha;
        fixed _RimExp;
        fixed3 _RimColor;
		fixed _RimPower;
		fixed _RimAlpha;
		fixed _Emission;

		//float4 _PanelCenterAndSharpness;
		//float _PanelSizeX;
		//float _PanelSizeY;

		float4 _Area;
		
		struct a2v 
		{
			float4 vertex : POSITION;
			fixed4 color : COLOR;
			float2 uv : TEXCOORD0;
            float3 normal : NORMAL;
		};

		struct v2f 
		{
			float4 pos : SV_POSITION;
			fixed4 color : COLOR;
			float2 uv : TEXCOORD0;
            float2 uvMask : TEXCOORD1;
			float2 uvAlpha : TEXCOORD2;
            float3 worldNormal : TEXCOORD3;
            float3 viewDir : TEXCOORD4;
			UNITY_FOG_COORDS(5)
			//float2 posInPanel : TEXCOORD6;

			float2 worldPos : TEXCOORD6;
		};
	


		v2f vert (a2v v)
		{
			v2f o;
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			o.pos = UnityObjectToClipPos(v.vertex);
			o.color = v.color;
			o.uv = TRANSFORM_TEX(v.uv, _MainTex) + float2(_MainTexX * _Time.y, _MainTexY * _Time.y);
            o.uvMask = TRANSFORM_TEX(v.uv, _MaskTex) + float2(_MaskTexX * _Time.y, _MaskTexY * _Time.y);
			o.uvAlpha = TRANSFORM_TEX(v.uv, _AlphaTex) + float2(_AlphaTexX * _Time.y, _AlphaTexY * _Time.y);
            o.worldNormal = UnityObjectToWorldNormal(v.normal);
            fixed3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
            o.viewDir = _WorldSpaceCameraPos.xyz - worldPos.xyz;
			UNITY_TRANSFER_FOG(o,o.pos);
			o.worldPos = mul(unity_ObjectToWorld, v.vertex).xy;

			/*
			#ifdef ENABLE_UICLIP
			float2 clipUV = o.pos.xy;
			#if SHADER_API_METAL
			    #if UNITY_UV_STARTS_AT_TOP
			    //clipUV.y = step(clipUV.y, 0)<1 ? -clipUV.y : clipUV.y
			    //clipUV.y = -clipUV.y;
			     clipUV.y = abs(clipUV.y);
			    #endif
			#endif
				//float2 clipSpace = clipUV / o.pos.w;
				//o.posInPanel = (clipSpace.xy + 1) * 0.5;
				//o.posInPanel.x -= _PanelCenterAndSharpness.x;
				//o.posInPanel.y -= _PanelCenterAndSharpness.y;
				//o.posInPanel.x *= (2 / _PanelSizeX);
				//o.posInPanel.y *= (2 / _PanelSizeY);
			#endif
			*/
			
			return o;
		}
	
		fixed4 frag (v2f i, float facing : VFACE) : SV_Target
		{
            fixed3 viewDir = normalize(i.viewDir);
            float faceSign = ( facing >= 0 ? 1 : -1 );
            fixed3 worldNormal = normalize(i.worldNormal) * faceSign;
            
			fixed4 col = 0;
            fixed4 mainTex = tex2D(_MainTex, i.uv) * _Color;
            fixed4 maskTex = tex2D(_MaskTex, i.uvMask);
            fixed4 alphaTex = tex2D(_AlphaTex, i.uvAlpha);
            col = mainTex;

			fixed cutOff = saturate(maskTex.r - _CutOff + 0.001);
		#ifdef ENABLE_ALPHACLIP
			cutOff = saturate(cutOff + i.color.a - 1);
		#endif

            col.rgb = lerp(col.rgb, _EdgeColor, cutOff > _EdgeLength ? 0 : 1);
			fixed3 rim = 0;
            fixed f =  1.0 - saturate(dot(normalize(viewDir), worldNormal));
            rim = _RimColor.rgb * pow (f, _RimExp) * _RimPower;
            col.rgb += rim;
			col.rgb *= i.color.rgb;
			col.rgb += col.rgb * _Emission;

            col.a = max(0, cutOff > 0 ? mainTex.a : 0);
			col.a = lerp(_RimAlpha * col.a, col.a, rim);
            col.a *= _Alpha * alphaTex.r * i.color.a;
			UNITY_APPLY_FOG(i.fogCoord, col);

		#ifdef ENABLE_UICLIP
			//float2 k = float2(_PanelCenterAndSharpness.z, _PanelCenterAndSharpness.w) * 0.5;
			//float2 factor = (float2(1.0, 1.0) - abs(i.posInPanel)) * _PanelCenterAndSharpness.zw * 0.5;
			//col.a *= clamp(min(factor.x, factor.y), 0.0, 1.0);

			if(_Area.x!=0 || _Area.y!=0 || _Area.z!=0 || _Area.w!=0)
			{
				bool flag = i.worldPos.x >= _Area.x && i.worldPos.x <= _Area.z && i.worldPos.y >= _Area.y && i.worldPos.y <= _Area.w;
				if(flag == false)
				{
					return fixed4(0,0,0,0);
				}
			}
			
			
		#endif
			return col;
		}

		fixed4 fragAlpha (v2f i) : SV_Target
		{
			fixed alpha = tex2D(_MainTex, i.uv).a * _BloomPower;
            fixed maskTex = tex2D(_MaskTex, i.uvMask);

			fixed cutOff = saturate(maskTex.r - _CutOff + 0.001);
		#ifdef ENABLE_ALPHACLIP
			cutOff = saturate(cutOff + i.color.a - 1);
		#endif

            fixed edgeAlpha = lerp(0, _EdgeBloomPower, cutOff > _EdgeLength ? 0 : 1);
            alpha = cutOff > 0 ? alpha + edgeAlpha : 0;
            alpha *= _Alpha;
			return fixed4(0,0,0,alpha);
		}
		ENDCG

		SubShader 
		{
			Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
			Cull [_Cull]
            Lighting Off 
            ZWrite Off

			Pass 
			{	
				Blend SrcAlpha One	
				//ColorMask RGB

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog
				
				ENDCG 
			}

			// Pass
			// {
			// 	Blend One OneMinusSrcAlpha
			// 	COLORMASK A

			// 	CGPROGRAM
			// 	#pragma vertex vert
			// 	#pragma fragment fragAlpha
				
			// 	ENDCG
			// }
		}	
	}
}
