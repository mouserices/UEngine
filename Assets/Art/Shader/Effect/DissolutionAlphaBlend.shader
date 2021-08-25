
Shader "YanJia/Effect/DissolutionAlphaBlend"
 {
    Properties {
        _ColorAlpha ("Color&Alpha", Color) = (1,1,1,1)
        _DiffuseTexture ("Diffuse Texture", 2D) = "white" {}
        _N_mask ("N_mask", Float ) = 0.3
        _T_mask ("T_mask", 2D) = "white" {}
		_C_BYcolor("C_BYcolor", Color) = (1,0,0,1)
		_N_BY_QD("N_BY_QD", Float) = 3
		_N_BY_KD("N_BY_KD", Float) = 0.01
		_Emission("Emission", Range(-1,5)) = 0
        _BloomPower ("Bloom Power", Range(0,1)) = 0.5
    }



    CGINCLUDE
        #include "UnityCG.cginc"
        uniform sampler2D _T_mask; uniform float4 _T_mask_ST;
        uniform sampler2D _DiffuseTexture; uniform float4 _DiffuseTexture_ST;
        uniform float4 _ColorAlpha;
        uniform float _N_mask;
        uniform float _N_BY_KD;
        uniform float4 _C_BYcolor;
        uniform float _N_BY_QD;
        float _BloomPower;
		float _Emission;

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
            float3 emissive = ((_ColorAlpha.rgb*_DiffuseTexture_var.rgb)*i.vertexColor.rgb);
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
			finalColor += finalColor * _Emission;
            return fixed4(finalColor,(_ColorAlpha.a*(_DiffuseTexture_var.a*(node_5746+node_1274))));
        }

        float4 fragAlpha (VertexOutput i) : SV_Target
        {
            float4 _DiffuseTexture_var = tex2D(_DiffuseTexture,TRANSFORM_TEX(i.uv0, _DiffuseTexture));
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
            float alpha = (_ColorAlpha.a*(_DiffuseTexture_var.a*(node_5746+node_1274))) * _BloomPower;
            return fixed4(0,0,0,alpha);
        }
    ENDCG



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
	        ZWrite Off
        	Cull Off

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
	        ZWrite Off
        	Cull Off
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
