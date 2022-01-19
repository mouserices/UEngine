Shader "YanJia/Effect/FrameAnimation"
{
	Properties
	{
		_Color("Base Color", Color) = (1,1,1,1)
		_MainTex("Base(RGB)", 2D) = "white" {}
		_ColNum("行数",Float) = 4
		_RowNum("列数", Float) = 5
		_Speed("Speed",Float) = 10
	}

	SubShader
	{
		tags{"Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True"}
		Blend SrcAlpha OneMinusSrcAlpha
		Zwrite Off
		Cull Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float4 _Color;
			sampler2D _MainTex;
			fixed _Speed;
			fixed _RowNum;
			fixed _ColNum;

			struct a2v
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			v2f vert(a2v v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.color = v.color;

				float ColSpace = 1.0 / _RowNum;
				float RowSpace = 1.0 / _ColNum;
				float2 uv = v.uv.xy;
				uv.x *= ColSpace;
				uv.y *= RowSpace;
				float col = floor(_Time.y *_Speed / _RowNum);
				float row = floor(_Time.y *_Speed - col * _RowNum);
				uv.x += row * ColSpace;
				uv.y += (_ColNum - 1 - col) * RowSpace;
				o.uv = uv;
				return o;
			}

			half4 frag(v2f i) :COLOR
			{
				half4 c = tex2D(_MainTex , i.uv) * _Color * i.color;
				return c;
			}

			ENDCG
		}
	}
}