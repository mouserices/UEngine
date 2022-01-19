
Shader "YanJia/Effect/DistortBlendAlpha"
{
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _ColorVal ("ColorVal", Float ) = 1
        _AlphaVal ("AlphaVal", Float ) = 1
        _MainTex ("MainTex", 2D) = "white" {}
        _SpeedUV01 ("Speed(U\V0~1)", Range(0, 1)) = 0
        _MainTexSpeed ("MainTexSpeed", Float ) = 0
        _TexNoise01 ("TexNoise01", 2D) = "white" {}
        _TexNoise02 ("TexNoise02", 2D) = "white" {}
        [MaterialToggle] _U ("U", Float ) = 0
        [MaterialToggle] _V ("V", Float ) = 0
        _raodongqiangdu ("raodongqiangdu", Float ) = 0.2
        _TexNoise01Val ("TexNoise01Val", Float ) = 0.2
        _TexNoise02Val ("TexNoise02Val", Float ) = 0.5
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5

        [Space(10)]
        [Toggle(ENABLE_UICLIP)] _UIClip ("Enable UI Clip", Float) = 0
		_PanelSizeX("_PanelSizeX", Range(0.000000,300.000000)) = 300.000000
		_PanelSizeY("_PanelSizeY", Range(0.000000,300.000000)) = 300.000000
		_PanelCenterAndSharpness("_PanelCenterAndSharpness", Vector) = (0,0,300,300)
    }


    SubShader		
	{
        Tags 
		{
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        Pass 
		{
            Name "FORWARD"
            Tags 
			{
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            //#define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
			#pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile _ ENABLE_UICLIP

            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _Color;
            uniform float _raodongqiangdu;
            uniform sampler2D _TexNoise01; uniform float4 _TexNoise01_ST;
            uniform sampler2D _TexNoise02; uniform float4 _TexNoise02_ST;
            uniform float _TexNoise02Val;
            uniform float _TexNoise01Val;
            uniform fixed _U;
            uniform fixed _V;
            uniform float _ColorVal;
            uniform float _AlphaVal;
            uniform float _SpeedUV01;
            uniform float _MainTexSpeed;

            float4 _PanelCenterAndSharpness;
            float _PanelSizeX;
            float _PanelSizeY;

            struct VertexInput 
			{
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput 
			{
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                float2 posInPanel : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v)
			{
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );

            #ifdef ENABLE_UICLIP
			float2 clipUV = o.pos.xy;
			
			#if SHADER_API_METAL
			    #if UNITY_UV_STARTS_AT_TOP 
			        //clipUV.y = step(clipUV.y, 0)<1 ? -clipUV.y : clipUV.y
			    	//clipUV.y = -clipUV.y;
			    	clipUV.y = abs(clipUV.y);
			    #endif
			#endif
			
			
				float2 clipSpace = clipUV / o.pos.w;
				o.posInPanel = (clipSpace.xy + 1) * 0.5;
				o.posInPanel.x -= _PanelCenterAndSharpness.x;
				o.posInPanel.y -= _PanelCenterAndSharpness.y;
				o.posInPanel.x *= (2 / _PanelSizeX);
				o.posInPanel.y *= (2 / _PanelSizeY);
			#endif
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR
			{
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 node_3042 = _Time;
                float node_7747 = (node_3042.g*_MainTexSpeed);
                float2 node_9269 = float2(i.uv0.r,i.uv0.g);
                float2 node_3413 = (node_9269+node_7747*float2(0,1));
                float2 node_5050 = (node_9269+node_7747*float2(1,0));
                float2 node_1641 = (i.uv0+((_TexNoise01Val*node_3042.g)*2.0+-1.0)*float2(1,0));
                float4 _node_4199 = tex2D(_TexNoise01,TRANSFORM_TEX(node_1641, _TexNoise01));
                float2 node_784 = (i.uv0+((_TexNoise02Val*node_3042.g)*2.0+-1.0)*float2(1,0));
                float4 _node_3502 = tex2D(_TexNoise02,TRANSFORM_TEX(node_784, _TexNoise02));
                float2 node_3957 = (i.uv0+((_TexNoise01Val*node_3042.g)*2.0+-1.0)*float2(0,1));
                float4 node_4577 = tex2D(_TexNoise01,TRANSFORM_TEX(node_3957, _TexNoise01));
                float2 node_867 = (i.uv0+((node_3042.g*_TexNoise02Val)*2.0+-1.0)*float2(0,1));
                float4 node_5413 = tex2D(_TexNoise02,TRANSFORM_TEX(node_867, _TexNoise02));
                float node_8686 = (((_node_4199.r*_node_3502.r)*_U)+(_V*(node_4577.r*node_5413.r)));
                float2 node_1702 = (lerp(node_3413,node_5050,_SpeedUV01)+(_raodongqiangdu*node_8686));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_1702, _MainTex));
                float3 emissive = ((_Color.rgb*_MainTex_var.rgb*i.vertexColor.rgb)*_ColorVal);
                float3 finalColor = emissive;
                fixed4 col =  fixed4(finalColor,(_AlphaVal*(_MainTex_var.a*node_8686*i.vertexColor.a)));

            #ifdef ENABLE_UICLIP
                float2 k = float2(_PanelCenterAndSharpness.z, _PanelCenterAndSharpness.w) * 0.5;
                float2 factor = (float2(1.0, 1.0) - abs(i.posInPanel)) * _PanelCenterAndSharpness.zw * 0.5;
                col.a *= clamp(min(factor.x, factor.y), 0.0, 1.0);
            #endif
                return col;
            }
            ENDCG
        }
    }
	FallBack Off
}