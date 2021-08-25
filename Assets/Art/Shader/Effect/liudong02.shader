
Shader "YanJia/Effect/liudong02"
 {
    Properties
     {
        _node_3204 ("node_3204", 2D) = "white" {}
        _node_9565 ("node_9565", 2D) = "white" {}
        _BloomPower ("Bloom Power", Range(0,1)) = 0.5
    }
    SubShader 
    {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        ZWrite Off

        CGINCLUDE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
 
            uniform float4 _TimeEditor;
            uniform sampler2D _node_3204; uniform float4 _node_3204_ST;
            uniform sampler2D _node_9565; uniform float4 _node_9565_ST;
            float _BloomPower;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) 
            {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR 
            {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );

                float4 node_2412 = _Time + _TimeEditor;
                float2 node_2022 = ((i.uv0*float2(2,1))+node_2412.g*float2(0.5,1));
                float4 _node_3204_var = tex2D(_node_3204,TRANSFORM_TEX(node_2022, _node_3204));
                float3 emissive = _node_3204_var.rgb;
                float3 finalColor = emissive;
                float4 _node_9565_var = tex2D(_node_9565,TRANSFORM_TEX(i.uv0, _node_9565));
                return fixed4(finalColor,_node_9565_var.g);
            }

            fixed4 fragAlpha (VertexOutput i, float facing : VFACE) : SV_Target
            {
                float4 _node_9565_var = tex2D(_node_9565,TRANSFORM_TEX(i.uv0, _node_9565));
                return fixed4(0,0,0,_node_9565_var.g * _BloomPower);
            }
        ENDCG

        Pass 
        {
            Name "FORWARD"
            Tags 
            {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGB
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            ENDCG
        }

        Pass
        {
            Blend One OneMinusSrcAlpha
            COLORMASK A

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragAlpha
            
            ENDCG
        }
    }
    FallBack Off
}
