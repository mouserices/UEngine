// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "YanJia/ModelAppear" {
Properties {
    _TintColor ("Tint Color", Color) = (0.1,0.2,0.2,0.2)
	_Appear("Appear", Range(-1, 5)) = 5
}

Category {
    Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }

	Blend One One

    //ColorMask RGB
    //Cull Off
	Lighting Off 
	ZWrite Off

    SubShader {
        Pass {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_particles
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            fixed4 _TintColor;
			uniform float _Appear;
            struct appdata_t {
                float4 vertex : POSITION;
				
                //fixed4 color : COLOR;
                //float2 texcoord : TEXCOORD0;
                //UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
				float4 posWorld : TEXCOORD0;
                //fixed4 color : COLOR;
                //UNITY_FOG_COORDS(1)
                //UNITY_VERTEX_OUTPUT_STEREO
            };

            float4 _MainTex_ST;

            v2f vert (appdata_t v)
            {
                v2f o;
                //UNITY_SETUP_INSTANCE_ID(v);
                //UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                //o.color = v.color;
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				clip(step(i.posWorld.y, _Appear) - 0.5);

				fixed4 col = _TintColor;
                return col;
            }
            ENDCG
        }
    }
}
}
