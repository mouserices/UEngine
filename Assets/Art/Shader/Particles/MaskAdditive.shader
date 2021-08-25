Shader "YanJia/Particles/AdditiveMask" 
{

	Properties 
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_Mask ("Mask ( R Channel )", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_BloomPower ("Bloom Power", Range(0,1)) = 0.5
	}

	Category
	{
		CGINCLUDE
		#pragma multi_compile_particles
		#pragma multi_compile_fog

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		sampler2D _Mask;
		fixed4 _TintColor;
	
		struct appdata_t 
		{
			float4 vertex : POSITION;
			fixed4 color : COLOR;
			float2 texcoord : TEXCOORD0;
		};

		struct v2f 
		{
			float4 vertex : SV_POSITION;
			fixed4 color : COLOR;
			float2 texcoord : TEXCOORD0;
			UNITY_FOG_COORDS(1)
			#ifdef SOFTPARTICLES_ON
			float4 projPos : TEXCOORD2;
			#endif
			float2 texcoordMask : TEXCOORD3;
		};
	
		float4 _Mask_ST;
		float4 _MainTex_ST;

		v2f vert (appdata_t v)
		{
			v2f o;
			UNITY_INITIALIZE_OUTPUT(v2f, o);

			o.vertex = UnityObjectToClipPos(v.vertex);
			#ifdef SOFTPARTICLES_ON
			o.projPos = ComputeScreenPos (o.vertex);
			COMPUTE_EYEDEPTH(o.projPos.z);
			#endif
			o.color = v.color;
			o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
			o.texcoordMask = TRANSFORM_TEX(v.texcoord,_Mask);
			UNITY_TRANSFER_FOG(o,o.vertex);
			return o;
		}

		sampler2D_float _CameraDepthTexture;
		fixed _InvFade;
		fixed _BloomPower;
	
		fixed4 frag (v2f i) : SV_Target
		{
			#ifdef SOFTPARTICLES_ON
			half sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
			half partZ = i.projPos.z;
			half fade = saturate (_InvFade * (sceneZ-partZ));
			i.color.a *= fade;
			#endif
		
			fixed4 col = tex2D(_MainTex, i.texcoord);
			col.a *= tex2D(_Mask, i.texcoordMask).r;
			col *= 2.0f * i.color * _TintColor;
			UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0,0,0,0)); // fog towards black due to our blend mode
			return col;
		}

		fixed4 fragAlpha (v2f i) : SV_Target
		{
			fixed4 mask = tex2D(_Mask, i.texcoordMask);
			fixed alpha = tex2D(_MainTex, i.texcoord).a * min(mask.r, mask.a);
			return fixed4(0,0,0,_BloomPower * alpha);
		}
		ENDCG
	
		SubShader 
		{
			Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
			Cull Off Lighting Off ZWrite Off

			Pass
			{
				ColorMask RGB
				Blend SrcAlpha One	

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog
				
				ENDCG
			}

			Pass
			{
				COLORMASK A
				Blend One OneMinusSrcAlpha

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment fragAlpha
				
				ENDCG
			}
		}	
	}
}