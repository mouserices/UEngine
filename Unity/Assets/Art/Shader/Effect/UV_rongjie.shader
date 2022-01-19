
Shader "YanJia/Effect/UV_rongjie" 
{
    Properties {
        _ColorAlpha ("Color&Alpha", Color) = (1,1,1,1)
        _N_mask ("N_mask", Float ) = 0.3
        _T_mask ("T_mask", 2D) = "white" {}
        _C_BYcolor ("C_BYcolor", Color) = (1,0,0,1)
        _N_BY_QD ("N_BY_QD", Float ) = 3
        _N_BY_KD ("N_BY_KD", Float ) = 0.01
        _Tex_01 ("Tex_01", 2D) = "white" {}
        _Tex_01_six ("Tex_01_six", Float ) = 1
        _Tex_02 ("Tex_02", 2D) = "white" {}
        _Tex_02_six ("Tex_02_six", Float ) = 1
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
            uniform sampler2D _T_mask; uniform float4 _T_mask_ST;
            uniform float4 _ColorAlpha;
            uniform float _N_mask;
            uniform float _N_BY_KD;
            uniform float4 _C_BYcolor;
            uniform float _N_BY_QD;
            uniform float _Tex_01_six;
            uniform float _Tex_02_six;
            uniform sampler2D _Tex_01; uniform float4 _Tex_01_ST;
            uniform sampler2D _Tex_02; uniform float4 _Tex_02_ST;
            float _BloomPower;
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
            };
            VertexOutput vert (VertexInput v) 
            {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : SV_TARGET 
            {
                float4 node_5033 = _Time + _TimeEditor;
                float2 node_3820 = (i.uv0+node_5033.g*float2(0,-1));
                float4 _Tex_01_var = tex2D(_Tex_01,TRANSFORM_TEX(node_3820, _Tex_01));
                float2 node_5359 = (i.uv0+node_5033.g*float2(0,-2));
                float4 _Tex_02_var = tex2D(_Tex_02,TRANSFORM_TEX(node_5359, _Tex_02));
                float3 emissive = ((_ColorAlpha.rgb*((_Tex_01_var.rgb*_Tex_01_six)+(_Tex_02_var.rgb*_Tex_02_six)))*i.vertexColor.rgb);
                float node_1423 = (i.vertexColor.a*_N_mask);
                float4 _T_mask_var = tex2D(_T_mask,TRANSFORM_TEX(i.uv0, _T_mask));
                float node_5746_if_leA = step(node_1423,_T_mask_var.r);
                float node_5746_if_leB = step(_T_mask_var.r,node_1423);
                float node_3073 = 0.0;
                float node_6272 = 1.0;
                float node_5746 = lerp((node_5746_if_leA*node_3073)+(node_5746_if_leB*node_6272),node_6272,node_5746_if_leA*node_5746_if_leB);
                float node_5217_if_leA = step(node_1423,(_T_mask_var.r+_N_BY_KD));
                float node_5217_if_leB = step((_T_mask_var.r+_N_BY_KD),node_1423);
                float node_1274 = (node_5746-lerp((node_5217_if_leA*node_3073)+(node_5217_if_leB*node_6272),node_6272,node_5217_if_leA*node_5217_if_leB));
                float3 finalColor = emissive + ((node_1274*_C_BYcolor.rgb)*_N_BY_QD);
                return fixed4(finalColor,(_ColorAlpha.a*((_Tex_01_var.a+_Tex_02_var.a)*(node_5746+node_1274))));
            }


            fixed4 fragAlpha () : SV_Target
            {
                return fixed4(0,0,0,_BloomPower);
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
