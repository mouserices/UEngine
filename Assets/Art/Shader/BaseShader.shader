
Shader "FBX/Base"
{
    Properties{
        _MainTex("texture", 2D) = "black"{}
    }
    
    SubShader{
    tags {"RenderType" = "charater"}
        LOD 300
        
        Pass{
            CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
            
            sampler2D _MainTex;
            fixed4 _MainTex_ST;
            
            struct vIn{
                half4 vertex:POSITION;
                float2 texcoord:TEXCOORD0;
               // fixed4 color:COLOR;
            };
            
            struct vOut{
                half4 pos:SV_POSITION;
                float2 uv:TEXCOORD0;
               // fixed4 color:COLOR;
            };
            
            vOut vert(vIn v){
                vOut o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
               // o.color = v.color;
                return o;
            }
            
            fixed4 frag(vOut i):COLOR{
                fixed4 tex = tex2D(_MainTex, i.uv);
                return tex;
            }
            ENDCG
        }
        
    }
}