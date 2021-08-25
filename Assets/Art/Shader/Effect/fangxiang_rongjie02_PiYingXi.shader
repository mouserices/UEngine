
Shader "YanJia/Effect/fangxiang_rongjie02_PiYingXi" {
    Properties {
        _Diffusse_color ("Diffusse_color", Color) = (0.5,0.5,0.5,1)
        _Tex ("Tex", 2D) = "white" {}
        _Rongjie_wenli ("Rongjie_wenli", 2D) = "white" {}
        _Rongjie ("Rongjie", Range(-2, 2)) = -2
        _MB_LD ("MB_LD", Float ) = 0.01
        _MD_Color ("MD_Color", Color) = (0.5,0.5,0.5,1)
        _node_6964 ("node_6964", Float ) = 3
        _miaobian ("miaobian", Range(0, 1)) = 0
        _miaobian_color ("miaobian_color", Color) = (0.5,0.5,0.5,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog

            uniform float _Rongjie;
            uniform sampler2D _Rongjie_wenli; uniform float4 _Rongjie_wenli_ST;
            uniform sampler2D _Tex; uniform float4 _Tex_ST;
            uniform float _MB_LD;
            uniform float _miaobian;
            uniform float4 _miaobian_color;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal*_miaobian,1) );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 _Tex_var = tex2D(_Tex,TRANSFORM_TEX(i.uv0, _Tex));
                float4 _Rongjie_wenli_var = tex2D(_Rongjie_wenli,TRANSFORM_TEX(i.uv0, _Rongjie_wenli));
                float node_7161_if_leA = step(_Rongjie,_Rongjie_wenli_var.r);
                float node_7161_if_leB = step(_Rongjie_wenli_var.r,_Rongjie);
                float node_2269 = 0.0;
                float node_3662 = 1.0;
                float node_7161 = lerp((node_7161_if_leA*node_2269)+(node_7161_if_leB*node_3662),node_3662,node_7161_if_leA*node_7161_if_leB);
                float node_3610_if_leA = step(_Rongjie,(_Rongjie_wenli_var.r+_MB_LD));
                float node_3610_if_leB = step((_Rongjie_wenli_var.r+_MB_LD),_Rongjie);
                float node_2691 = (node_7161-lerp((node_3610_if_leA*node_2269)+(node_3610_if_leB*node_3662),node_3662,node_3610_if_leA*node_3610_if_leB));
                clip((_Tex_var.a*(node_7161+node_2691)) - 0.5);
                return fixed4(_miaobian_color.rgb,0);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog

            uniform float4 _LightColor0;
            uniform float _Rongjie;
            uniform sampler2D _Rongjie_wenli; uniform float4 _Rongjie_wenli_ST;
            uniform float4 _Diffusse_color;
            uniform sampler2D _Tex; uniform float4 _Tex_ST;
            uniform float _MB_LD;
            uniform float4 _MD_Color;
            uniform float _node_6964;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float4 _Tex_var = tex2D(_Tex,TRANSFORM_TEX(i.uv0, _Tex));
                float4 _Rongjie_wenli_var = tex2D(_Rongjie_wenli,TRANSFORM_TEX(i.uv0, _Rongjie_wenli));
                float node_7161_if_leA = step(_Rongjie,_Rongjie_wenli_var.r);
                float node_7161_if_leB = step(_Rongjie_wenli_var.r,_Rongjie);
                float node_2269 = 0.0;
                float node_3662 = 1.0;
                float node_7161 = lerp((node_7161_if_leA*node_2269)+(node_7161_if_leB*node_3662),node_3662,node_7161_if_leA*node_7161_if_leB);
                float node_3610_if_leA = step(_Rongjie,(_Rongjie_wenli_var.r+_MB_LD));
                float node_3610_if_leB = step((_Rongjie_wenli_var.r+_MB_LD),_Rongjie);
                float node_2691 = (node_7161-lerp((node_3610_if_leA*node_2269)+(node_3610_if_leB*node_3662),node_3662,node_3610_if_leA*node_3610_if_leB));
                clip((_Tex_var.a*(node_7161+node_2691)) - 0.5);
                //float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                //float3 lightColor = _LightColor0.rgb;
////// Lighting:
                //float attenuation = LIGHT_ATTENUATION(i);
                //float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                //float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                //float3 directDiffuse = max( 0.0, NdotL) * float3(1,1,1);
                //float3 indirectDiffuse = float3(0,0,0);
                //indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                //float3 diffuseColor = ((_Diffusse_color.rgb*_Tex_var.rgb)*2.0);
                //float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                //float3 emissive = ((node_2691*_MD_Color.rgb)*_node_6964);
/// Final Color:
                //float3 finalColor = diffuse + emissive;
                float3 finalColor = _Tex_var;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack Off
}
