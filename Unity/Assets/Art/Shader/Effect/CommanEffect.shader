﻿Shader "YanJia/Effect/CommanEffect"
{
Properties
	{
		[Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull Mode", Float) = 2
		_Color (" Color", Color) = (1,1,1,1)
		_MainTex ("Main Texture", 2D) = "white" {}
        _MaskTex("Mask Texture", 2D) = "white" {}
        _CutOff("Cut Off", Range(0, 1.0)) = 0.5
        _EdgeColor("Edge Color", Color) = (1,1,0)
        _EdgeLength("Edge Length", Range(0.0, 1)) = 0.0
        _EdgeBloomPower("Edge Bloom Power", Range(0.0, 1)) = 1
        [Space(20)]
        _MainTexX("MainTex Move X", Range (-3, 3)) = 0
        _MainTexY("MainTex Move Y", Range (-3, 3)) = 0
		_MaskTexX("MaskTex Move X", Range (-3, 3)) = 0
        _MaskTexY("MaskTex Move Y", Range (-3, 3)) = 0
        [Space(20)]
        _RimExp ("Rim Exp", Range(0, 10)) = 20
        _RimColor ("Rim Color", Color) = (1,1,1)
		_RimPower ("Rim Power", Range(0,3)) = 0.2
        [Space(20)]
		_BloomPower ("Bloom Power", Range(0,1)) = 0.5
		_Emission("Emission", Range(0,5)) = 0
		[Space(20)]
		[Toggle(ENABLE_ALPHACLIP)]_EnableAlphaClip("Enable Alpha CutOff", Float) = 0
	}

	Category 
	{
		CGINCLUDE
		#pragma multi_compile_particles
		#pragma multi_compile_fog
		#pragma multi_compile _ ENABLE_ALPHACLIP
		#include "UnityCG.cginc"
		
		fixed4 _Color;
        sampler2D _MainTex;
        float4 _MainTex_ST;
        sampler2D _MaskTex;
        float4 _MaskTex_ST;
        fixed _BloomPower;
        fixed3 _EdgeColor;
        fixed _EdgeLength;
        fixed _CutOff;
        fixed _EdgeBloomPower;
        float _MainTexX;
        float _MainTexY;
		float _MaskTexX;
        float _MaskTexY;
		fixed _RimExp;
        fixed3 _RimColor;
		fixed _RimPower;
		fixed _Emission;
	
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
            float3 worldNormal : TEXCOORD2;
            float3 viewDir : TEXCOORD3;
			UNITY_FOG_COORDS(4)
		};
	


		v2f vert (a2v v)
		{
			v2f o;
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			o.pos = UnityObjectToClipPos(v.vertex);
			o.color = v.color;
			o.uv = TRANSFORM_TEX(v.uv, _MainTex) + float2(_MainTexX * _Time.y, _MainTexY * _Time.y);
            o.uvMask = TRANSFORM_TEX(v.uv, _MaskTex)+ float2(_MaskTexX * _Time.y, _MaskTexY * _Time.y);
            o.worldNormal = UnityObjectToWorldNormal(v.normal);
            fixed3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
            o.viewDir = _WorldSpaceCameraPos.xyz - worldPos.xyz;
			UNITY_TRANSFER_FOG(o,o.pos);
			return o;
		}
	
		fixed4 frag (v2f i) : SV_Target
		{
			fixed3 viewDir = normalize(i.viewDir);
            fixed3 worldNormal = normalize(i.worldNormal);

			fixed4 col = 0;
            
            fixed4 mainTex = tex2D(_MainTex, i.uv) * _Color;
            fixed4 maskTex = tex2D(_MaskTex, i.uvMask);
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

			fixed alpha = mainTex.a * _BloomPower * i.color.a;
			fixed edgeAlpha = lerp(0, _EdgeBloomPower, cutOff > _EdgeLength ? 0 : 1);
            alpha = cutOff > 0 ? alpha + edgeAlpha : 0;
			col.a = alpha;
			
			clip(cutOff);
			UNITY_APPLY_FOG(i.fogCoord, col);
			return col;
		}
		ENDCG

		SubShader 
		{
			Tags { "Queue"="AlphaTest"  "RenderType"="TransparentCutout"  "IgnoreProjector"="True"}
			Lighting Off
			Cull [_Cull]

			Pass 
			{	
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog
				
				ENDCG 
			}
		}	
	}
}