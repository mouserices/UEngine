Shader "YanJia/UI/CutOff"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [KeywordEnum(None, Circle, Diamond, Rectangle)] _ClipShape ("Clip Shape",Float) = 0
        _Radius("Radius", Range(0,1)) = 0.5
        _AspectRatio("Aspect", Range(0,1)) = 0.5
        _OffsetX("Clip OffsetX", Range(-1,1)) = 0
        _OffsetY("Clip OffsetY", Range(-1,1)) = 0
        _MainTexOffsetX("Texture OffsetX", Range(-1,1)) = 0
        _MainTexOffsetY("Texture OffsetY", Range(-1,1)) = 0
        _BloomPower ("Bloom Power", Range(0,1)) = 0

        [Space(20)]
		_Brightness("Brightness", Range(0,2)) = 1
		_Contrast("Contrast", Range(0, 2)) = 1
		_Saturation("Saturation", Range(0,2)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "IgnoreProjector"="True" "Queue" = "Transparent"}

		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off Lighting Off ZWrite Off

        Pass
        {
            ColorMask RGB
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            #pragma multi_compile _CLIPSHAPE_NONE _CLIPSHAPE_CIRCLE _CLIPSHAPE_DIAMOND _CLIPSHAPE_RECTANGLE

            struct a2v
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed _Radius;
            fixed _AspectRatio;
            fixed _OffsetX;
            fixed _OffsetY;
            fixed _MainTexOffsetX;
            fixed _MainTexOffsetY;

            half _Brightness;
			half _Contrast;
			half _Saturation;

            v2f vert (a2v v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed2 uv = i.uv + fixed2(_MainTexOffsetX, _MainTexOffsetY);
                fixed4 col = tex2D(_MainTex, uv);

                col.rgb *= _Brightness;
				half gray = 0.2125 * col.r + 0.7154 * col.g + 0.0721 * col.b;
				half3 grayColor = fixed3(gray, gray, gray);
				col.rgb = lerp(grayColor, col.rgb, _Saturation);
				half3 avgColor = half3(0.5, 0.5, 0.5);
				col.rgb = lerp(avgColor, col.rgb, _Contrast);

                #if _CLIPSHAPE_CIRCLE
                    fixed radius = (i.uv.x - _OffsetX - 0.5) * (i.uv.x - _OffsetX - 0.5) + (i.uv.y - _OffsetY - 0.5) * (i.uv.y - _OffsetY - 0.5);
                    clip(_Radius / 2 - radius);
                #endif
                #if _CLIPSHAPE_DIAMOND
                    fixed radius = max(abs((i.uv.x - 0.5 - _OffsetX)  + (i.uv.y - 0.5 - _OffsetY)), abs((0.5 - i.uv.x + _OffsetX) - (0.5 - i.uv.y + _OffsetY)));
                    clip(_Radius - radius);
                #endif
                #if _CLIPSHAPE_RECTANGLE
                    fixed radius = max(abs(i.uv.x - 0.5 - _OffsetX) * _AspectRatio, abs(i.uv.y - 0.5 - _OffsetY) * (1 - _AspectRatio));
                    clip(_Radius - radius * 2);
                #endif
                return col;
            }
            ENDCG
        }

        Pass
        {
            Cull Off Lighting Off ZWrite Off
            COLORMASK A
            BlendOp max

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            #pragma multi_compile _CLIPSHAPE_NONE _CLIPSHAPE_CIRCLE _CLIPSHAPE_DIAMOND _CLIPSHAPE_RECTANGLE

            struct a2v
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed _Radius;
            fixed _AspectRatio;
            fixed _OffsetX;
            fixed _OffsetY;
            fixed _MainTexOffsetX;
            fixed _MainTexOffsetY;
            fixed _BloomPower;

            v2f vert (a2v v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = 0;
                fixed2 uv = i.uv + fixed2(_MainTexOffsetX, _MainTexOffsetY);
                col.a = tex2D(_MainTex, uv).a;
                #if _CLIPSHAPE_CIRCLE
                    fixed radius = (i.uv.x - _OffsetX - 0.5) * (i.uv.x - _OffsetX - 0.5) + (i.uv.y - _OffsetY - 0.5) * (i.uv.y - _OffsetY - 0.5);
                    clip(_Radius / 2 - radius);
                #endif
                #if _CLIPSHAPE_DIAMOND
                    fixed radius = max(abs((i.uv.x - 0.5 - _OffsetX)  + (i.uv.y - 0.5 - _OffsetY)), abs((0.5 - i.uv.x + _OffsetX) - (0.5 - i.uv.y + _OffsetY)));
                    clip(_Radius - radius);
                #endif
                #if _CLIPSHAPE_RECTANGLE
                    fixed radius = max(abs(i.uv.x - 0.5 - _OffsetX) * _AspectRatio, abs(i.uv.y - 0.5 - _OffsetY) * (1 - _AspectRatio));
                    clip(_Radius - radius * 2);
                #endif
                col.a *= _BloomPower;
                return col;
            }
            
            ENDCG
        }
    }
}
