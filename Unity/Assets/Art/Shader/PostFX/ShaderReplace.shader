Shader "Hidden/ShaderReplace" 
{
    SubShader 
	{
	    Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Brush"}

		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off

        Pass 
		{    
            CGPROGRAM
			#include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag

			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _BloomPower;
			fixed _WindSpeed;
			fixed _WindPower;

			struct a2v
			{
				float4 vertex : POSITION;
				fixed4 vertexColor : COLOR;
				float4 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				fixed3 vertexColor : COLOR;
			};

			v2f vert(a2v v)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				v.vertex.x += sin(_Time.z * _WindSpeed) * o.uv.y * _WindPower; //添加随风摇动
				o.pos = UnityObjectToClipPos(v.vertex);
				o.vertexColor = v.vertexColor;//支持特效系统控制颜色
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = fixed4(0,0,0,1);
				fixed4 tex = tex2D(_MainTex, i.uv);
				col.r = tex.a;
				col.g =  tex.a * _BloomPower;
				return col;
			}
            ENDCG
        }
    }

	SubShader
	{
		Tags { "RenderType" = "charater" "Queue"="Geometry"}

		Pass
		{
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ ENABLE_CUTOFF
			half _CutOff;

			struct a2v
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
			};

			v2f vert(a2v v)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				#if ENABLE_CUTOFF
					fixed fade = 1 - _CutOff;
					return fixed4(fade,fade,fade,fade);
				#endif
				return fixed4(1, 1, 1, 1);
			}
			ENDCG
		}
	}

	SubShader
	{
		Tags { "RenderType" = "ShadowMask" "Queue"="Geometry"}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct a2v
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
			};

			v2f vert(a2v v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return fixed4(0, 0, 0, 0);
			}
			ENDCG
		}
	}
	Fallback Off
}
