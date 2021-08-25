
Shader "YanJia/Effect/distortalpha"
 {
    Properties 
    {
        _Color ("Color", Color) = (0.5019608,0.5019608,0.5019608,1)
        _ColorVal ("ColorVal", Float ) = 1
        _MainTex ("MainTex", 2D) = "white" {}
        _Alpha ("Alpha", 2D) = "black" {}
        _Mask ("MaskTex", 2D) = "black" {}
        _AlphaPower ("AlphaPower", Float ) = 0.2
        _Speed_U ("Speed_U", Float ) = 0
        _Speed_V ("Speed_V", Float ) = 0
		[Space(20)]
		[Toggle(ENABLE_ALPHADISTORT)]_EnableAlphaDistort("Enable Alpha Distort", Float) = 0
    }


    SubShader {
        Tags 
        {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass
         {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
			#pragma multi_compile _ ENABLE_ALPHADISTORT

            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _Alpha; uniform float4 _Alpha_ST;
            sampler2D _Mask;
            float4 _Mask_ST;
            uniform float _AlphaPower;
            uniform float _ColorVal;
            uniform float _Speed_U;
            uniform float4 _Color;
            uniform float _Speed_V;
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
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v)
             {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR
             {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 node_1354 = _Time;
                float2 node_9615 = ((i.uv0+((node_1354.g*_Speed_U)*2.0+-1.0)*float2(0,1))+(i.uv0+((node_1354.g*_Speed_V)*2.0+-1.0)*float2(1,0)));
                float4 _Alpha_var = tex2D(_Alpha,TRANSFORM_TEX(node_9615, _Alpha));
#ifdef ENABLE_ALPHADISTORT
				float temp = (1 - i.vertexColor.a) / 5;
				float2 node_905 = (i.uv0 + (_Alpha_var.r * temp));
				float4 _MainTex_var = tex2D(_MainTex, TRANSFORM_TEX(node_905 - float2(temp / 2, temp / 2), _MainTex));
#else
				float2 node_905 = (i.uv0 + (_Alpha_var.r*_AlphaPower));
				float4 _MainTex_var = tex2D(_MainTex, TRANSFORM_TEX(node_905, _MainTex));
#endif
                float3 emissive = ((_ColorVal*_MainTex_var.rgb*_Color.rgb)*i.vertexColor.rgb);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,(_MainTex_var.a*i.vertexColor.a));
                float4 maskTex = tex2D(_Mask, TRANSFORM_TEX(i.uv0, _Mask));
                clip(i.vertexColor.a - maskTex.r);

                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }     
    }
    FallBack Off
}
