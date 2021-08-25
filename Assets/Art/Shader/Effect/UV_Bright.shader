
Shader "YanJia/Effect/UV_Bright"
{
    Properties 
    {
        _Diffuse ("Diffuse", 2D) = "white" {}
        _DifColor ("Dif.Color", Color) = (0.5,0.5,0.5,1)
        _DifSpeedU ("Dif.Speed.U", Float ) = 0
        _DifSpeedV ("Dif.Speed.V", Float ) = 0
        _Nose ("Nose", 2D) = "white" {}
        _NosSpeedU ("Nos.Speed.U", Float ) = 0
        _NosSpeedV ("Nos.Speed.V", Float ) = 0
        _VerOffset ("Ver.Offset", Float ) = 0
        _Bright ("Bright", Float ) = 1
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
        Cull Off

        Pass 
        {
            Name "FORWARD"
            Tags 
            {
                "LightMode"="ForwardBase"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #pragma multi_compile_fog


            uniform float4 _TimeEditor;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float4 _DifColor;
            uniform sampler2D _Nose; uniform float4 _Nose_ST;
            uniform float _VerOffset;
            uniform float _NosSpeedU;
            uniform float _NosSpeedV;
            uniform float _DifSpeedU;
            uniform float _DifSpeedV;
            uniform float _Bright;
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
                UNITY_FOG_COORDS(1)
            };

            VertexOutput vert (VertexInput v)
            {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                float4 node_2106 = _Time + _TimeEditor;
                float2 node_8606 = (o.uv0+(node_2106.g*float2(_DifSpeedU,_DifSpeedV)));
                float4 _Diffuse_var = tex2Dlod(_Diffuse,float4(TRANSFORM_TEX(node_8606, _Diffuse),0.0,0));
                float4 node_2606 = _Time + _TimeEditor;
                float2 node_3614 = (o.uv0+(node_2606.g*float2(_NosSpeedU,_NosSpeedV)));
                float4 _Nose_var = tex2Dlod(_Nose,float4(TRANSFORM_TEX(node_3614, _Nose),0.0,0));
                float node_5711_if_leA = step(o.vertexColor.a,_Nose_var.r);
                float node_5711_if_leB = step(_Nose_var.r,o.vertexColor.a);
                float node_1929 = 1.0;
                float node_5480 = (_Diffuse_var.r*lerp((node_5711_if_leA*0.0)+(node_5711_if_leB*node_1929),node_1929,node_5711_if_leA*node_5711_if_leB));
                float node_7479 = (node_5480*_VerOffset);
                v.vertex.xyz += float3(node_7479,node_7479,node_7479);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }

            float4 frag(VertexOutput i) : COLOR 
            {
                float4 node_2106 = _Time + _TimeEditor;
                float2 node_8606 = (i.uv0+(node_2106.g*float2(_DifSpeedU,_DifSpeedV)));
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(node_8606, _Diffuse));
                float4 node_2606 = _Time + _TimeEditor;
                float2 node_3614 = (i.uv0+(node_2606.g*float2(_NosSpeedU,_NosSpeedV)));
                float4 _Nose_var = tex2D(_Nose,TRANSFORM_TEX(node_3614, _Nose));
                float3 emissive = (i.vertexColor.a*(i.vertexColor.rgb*(((_Diffuse_var.r*_Nose_var.r)*_DifColor.rgb)*_Bright)));
                float3 finalColor = emissive;
                float node_5711_if_leA = step(i.vertexColor.a,_Nose_var.r);
                float node_5711_if_leB = step(_Nose_var.r,i.vertexColor.a);
                float node_1929 = 1.0;
                float node_5480 = (_Diffuse_var.r*lerp((node_5711_if_leA*0.0)+(node_5711_if_leB*node_1929),node_1929,node_5711_if_leA*node_5711_if_leB));
                fixed4 finalRGBA = fixed4(finalColor,node_5480);
                finalRGBA.rgb *= finalRGBA.a;
                finalRGBA.a *= _BloomPower;
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0.6603774,0.1650943,0.2000644,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
    
}
