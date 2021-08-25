
Shader "YanJia/Effect/DissolutionAdditive" 
{
    Properties
     {
        _DiffuseColor ("Diffuse Color", Color) = (0.6985294,0.6985294,0.6985294,1)
        _DiffuseTexture ("Diffuse Texture", 2D) = "white" {}
        _N_mask ("N_mask", Float ) = 0.3
        _MaskTexture ("Mask Texture", 2D) = "white" {}
        _C_BYcolor ("C_BYcolor", Color) = (1,0,0,1)
        _N_BY_QD ("N_BY_QD", Float ) = 3
        _N_BY_KD ("N_BY_KD", Float ) = 0.01
        _BloomPower ("Bloom Power", Range(0,1)) = 0.5
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
            ZWrite Off
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            uniform sampler2D _MaskTexture; uniform float4 _MaskTexture_ST;
            uniform sampler2D _DiffuseTexture; uniform float4 _DiffuseTexture_ST;
            uniform float4 _DiffuseColor;
            uniform float _N_mask;
            uniform float _N_BY_KD;
            uniform float4 _C_BYcolor;
            uniform float _N_BY_QD;
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
            float4 frag(VertexOutput i) : COLOR
             {
                float4 _DiffuseTexture_var = tex2D(_DiffuseTexture,TRANSFORM_TEX(i.uv0, _DiffuseTexture));
                float node_654 = (i.vertexColor.a*_N_mask);
                float4 _MaskTexture_var = tex2D(_MaskTexture,TRANSFORM_TEX(i.uv0, _MaskTexture));
                float node_5746_if_leA = step(node_654,_MaskTexture_var.r);
                float node_5746_if_leB = step(_MaskTexture_var.r,node_654);
                float node_3073 = 0.0;
                float node_6272 = 1.0;
                float node_5746 = lerp((node_5746_if_leA*node_3073)+(node_5746_if_leB*node_6272),node_6272,node_5746_if_leA*node_5746_if_leB);
                float node_5217_if_leA = step(node_654,(_MaskTexture_var.r+_N_BY_KD));
                float node_5217_if_leB = step((_MaskTexture_var.r+_N_BY_KD),node_654);
                float node_1274 = (node_5746-lerp((node_5217_if_leA*node_3073)+(node_5217_if_leB*node_6272),node_6272,node_5217_if_leA*node_5217_if_leB));
                float node_6450 = (node_5746+node_1274);
                float3 node_7666 = ((node_1274*_C_BYcolor.rgb)*_N_BY_QD);
                float3 emissive = (_DiffuseColor.a*(((_DiffuseColor.rgb*_DiffuseTexture_var.rgb)*node_6450)+node_7666));
                float3 finalColor = emissive + (_DiffuseColor.a*node_7666);
                fixed4 col = fixed4(finalColor,(_DiffuseColor.a*(_DiffuseTexture_var.a*node_6450)));
                //col.rgb *= col.a;
				col.a *= _BloomPower;
                return col;
            }
            ENDCG
        }
    }
    FallBack Off
}
