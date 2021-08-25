
Shader "YanJia/Effect/DistortBlendAdd"
{
    Properties
	{
        _Color ("Color", Color) = (1,1,1,1)
        _ColorVal ("ColorVal", Float ) = 1
        _MainTex ("MainTex", 2D) = "white" {}
        _TexNoise01Color ("TexNoise01Color", Color) = (1,1,1,1)
        _TexNoise01ColorVal ("TexNoise01ColorVal", Float ) = 1
        _TexNoise01RRGB ("TexNoise01R-RGB", Range(0, 1)) = 0
        _TexNoise01 ("TexNoise01", 2D) = "white" {}
        _TexNoise02Color ("TexNoise02Color", Color) = (1,1,1,1)
        _TexNoise02ColorVal ("TexNoise02ColorVal", Float ) = 1
        _TexNoise02RRGB ("TexNoise02R-RGB", Range(0, 1)) = 0
        _TexNoise02 ("TexNoise02", 2D) = "white" {}
        [MaterialToggle] _U ("U", Float ) = 0
        [MaterialToggle] _V ("V", Float ) = 0
        _raodongqiangdu ("raodongqiangdu", Float ) = 0.2
        _TexNoise01Val ("TexNoise01Val", Float ) = 0.2
        _TexNoise02Val ("TexNoise02Val", Float ) = 0.5
        _HueSeparation ("HueSeparation", Float ) = 0

		[Space(10)]
        [Toggle(ENABLE_UICLIP)] _UIClip ("Enable UI Clip", Float) = 0
        //_PanelSizeX("_PanelSizeX", Range(0.000000,300.000000)) = 300.000000
		//_PanelSizeY("_PanelSizeY", Range(0.000000,300.000000)) = 300.000000
		//_PanelCenterAndSharpness("_PanelCenterAndSharpness", Vector) = (0,0,300,300)
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
            Blend One One
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


            uniform float4 _Color;
            uniform float _raodongqiangdu;
            uniform sampler2D _TexNoise01; uniform float4 _TexNoise01_ST;
            uniform sampler2D _TexNoise02; uniform float4 _TexNoise02_ST;
            uniform float _TexNoise02Val;
            uniform float _TexNoise01Val;
            uniform fixed _U;
            uniform fixed _V;
            uniform float _ColorVal;
            uniform float _HueSeparation;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _TexNoise01RRGB;
            uniform float _TexNoise02RRGB;
            uniform float4 _TexNoise02Color;
            uniform float _TexNoise02ColorVal;
            uniform float4 _TexNoise01Color;
            uniform float _TexNoise01ColorVal;

            //float4 _PanelCenterAndSharpness;
            //float _PanelSizeX;
            //float _PanelSizeY;

			float4 _Area;
            
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
                //float2 posInPanel : TEXCOORD1;

            	float2 worldPos : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) 
			{
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xy;

            	/*
            #ifdef ENABLE_UICLIP
			float2 clipUV = o.pos.xy;
			#if SHADER_API_METAL
			    #if UNITY_UV_STARTS_AT_TOP 
			        //clipUV.y = step(clipUV.y, 0)<1 ? -clipUV.y : clipUV.y
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
			*/
			
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR
			{
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 node_3042 = _Time;
                float2 node_1641 = (i.uv0+((_TexNoise01Val*node_3042.g)*2.0+-1.0)*float2(1,0));
                float4 _node_4199 = tex2D(_TexNoise01,TRANSFORM_TEX(node_1641, _TexNoise01));
                float2 node_784 = (i.uv0+((_TexNoise02Val*node_3042.g)*2.0+-1.0)*float2(1,0));
                float4 _node_3502 = tex2D(_TexNoise02,TRANSFORM_TEX(node_784, _TexNoise02));
                float node_2850 = (_node_4199.r*_node_3502.r);
                float2 node_3957 = (i.uv0+((_TexNoise01Val*node_3042.g)*2.0+-1.0)*float2(0,1));
                float4 node_4577 = tex2D(_TexNoise01,TRANSFORM_TEX(node_3957, _TexNoise01));
                float2 node_867 = (i.uv0+((node_3042.g*_TexNoise02Val)*2.0+-1.0)*float2(0,1));
                float4 node_5413 = tex2D(_TexNoise02,TRANSFORM_TEX(node_867, _TexNoise02));
                float node_1100 = (node_4577.r*node_5413.r);
                float3 node_8686 = ((lerp(float3(node_2850,node_2850,node_2850),(_node_4199.rgb*_node_3502.rgb*_TexNoise01Color.rgb*_TexNoise01ColorVal),_TexNoise01RRGB)*_U)+(_V*lerp(float3(node_1100,node_1100,node_1100),(node_4577.rgb*node_5413.rgb*_TexNoise02Color.rgb*_TexNoise02ColorVal),_TexNoise02RRGB)));
                float3 node_1702 = (float3(i.uv0,0.0)+(_raodongqiangdu*node_8686));
                float3 node_3482 = (node_1702+_HueSeparation);
                float4 _node_7162 = tex2D(_MainTex,TRANSFORM_TEX(node_3482, _MainTex));
                float4 node_6282 = tex2D(_MainTex,TRANSFORM_TEX(node_1702, _MainTex));
                float3 node_5550 = (node_1702-_HueSeparation);
                float4 node_7783 = tex2D(_MainTex,TRANSFORM_TEX(node_5550, _MainTex));
                float3 emissive = (((_Color.rgb*float3(_node_7162.r,node_6282.g,node_7783.b)*i.vertexColor.rgb)*_ColorVal)*(_node_7162.a*node_8686*i.vertexColor.a));
                float3 finalColor = emissive;


				
            #ifdef ENABLE_UICLIP
                //float2 k = float2(_PanelCenterAndSharpness.z, _PanelCenterAndSharpness.w) * 0.5;
                //float2 factor = (float2(1.0, 1.0) - abs(i.posInPanel)) * _PanelCenterAndSharpness.zw * 0.5;
                //float alpha = clamp(min(factor.x, factor.y), 0.0, 1.0);
                //clip(alpha - 0.01);

				if(_Area.x!=0 || _Area.y!=0 || _Area.z!=0 || _Area.w!=0)
				{
					bool flag = i.worldPos.x >= _Area.x && i.worldPos.x <= _Area.z && i.worldPos.y >= _Area.y && i.worldPos.y <= _Area.w;
					if(flag == false)
					{
						return fixed4(0,0,0,0);
					}
				}
				
				
            #endif
                return fixed4(finalColor,1);
            }
            ENDCG
        } 
    }
	FallBack Off
}