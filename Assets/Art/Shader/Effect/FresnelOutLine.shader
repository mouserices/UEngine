
Shader "YanJia/Effect/FresnelOutLIne" 
{
    Properties
    {
        _TintColor ("Color", Color) = (0.5,0.5,0.5,1)
        _OutlineWidth ("Outline Width", Range(0, 0.05)) = 0.003
        _OutLineColor ("OutLine Color", Color) = (0.5,0.5,0.5,1)
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
            ColorMask 0
        }

        Pass 
        {
            Name "FORWARD"
            Tags {  "LightMode"="ForwardBase" }
            Blend One One
            ColorMask RGB
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #pragma multi_compile_fog
            uniform float4 _TintColor;

            struct VertexInput
             {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput 
            {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };

            VertexOutput vert (VertexInput v) 
            {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }

            float4 frag(VertexOutput i, float facing : VFACE) : COLOR 
            {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;

                float3 emissive = ((pow(1.0-max(0,dot(normalDirection, viewDirection)),1.5)*_TintColor.rgb*2.0)+(_TintColor.rgb*0.3));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0.5588235,0.908722,1,1));
                return finalRGBA;
            }
            ENDCG
        }

        Pass 
        {
            ColorMask RGB
            Cull Front
            Blend  SrcAlpha OneMinusSrcAlpha
            ZWrite Off 
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed _OutlineWidth;
            fixed4 _OutLineColor;

            struct a2v 
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert (a2v v) 
            {
                v2f o;
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                float outLineFade = 1;

                float4x4 P = unity_CameraProjection;
                float fov = atan(1 / P._m11) * 2;
                float fovScale = degrees(fov) / 40;
                outLineFade *= pow(fovScale, 0.5);

                float3 posWorld = mul(unity_ObjectToWorld, v.vertex);
                float distance = length(_WorldSpaceCameraPos.xyz - posWorld);
                //outLineFade *= pow(distance / 20, 1) * 6;
                outLineFade *= pow(distance, 0.5);
                //outLineFade = max(1, outLineFade);

                float4 pos = mul(UNITY_MATRIX_MV, v.vertex);
                float3 normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
                normal.z = -0.5;
                pos = pos + float4(normalize(normal), 0) * _OutlineWidth * outLineFade;
                o.pos = mul(UNITY_MATRIX_P, pos);
                //o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal * _OutlineWidth,1) );
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET 
            {
                return fixed4(_OutLineColor.rgb, 1) ;
            }
            ENDCG
        }

		Pass
		{
			ColorMask A
			BlendOp Max

			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag

			half _BloomPower;

			struct a2v
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
			};

			v2f vert (a2v v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				return fixed4(0,0,0,_BloomPower);
			}

			ENDCG
		}


    }

    Fallback Off
}
