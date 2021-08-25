Shader "YanJia/Particles/Additive" 
{

	Properties 
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_BloomPower ("Bloom Power", Range(0,1)) = 0.5

		[Space(10)]
		[Toggle(ENABLE_UICLIP)] _UIClip ("Enable UI Clip", Float) = 0
		//_PanelSizeX("_PanelSizeX", Range(0.000000,300.000000)) = 300.000000
		//_PanelSizeY("_PanelSizeY", Range(0.000000,300.000000)) = 300.000000
		//_PanelCenterAndSharpness("_PanelCenterAndSharpness", Vector) = (0,0,300,300)
	}

	Category
	{
		CGINCLUDE
		#pragma multi_compile_particles
		#pragma multi_compile_fog
		#pragma multi_compile _ ENABLE_UICLIP

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		fixed4 _TintColor;

		//float4 _PanelCenterAndSharpness;
		//float _PanelSizeX;
		//float _PanelSizeY;

		float4 _Area;
		
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
			
			//float2 posInPanel : TEXCOORD3;
			float2 worldPos : TEXCOORD3;
		};
	
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
			UNITY_TRANSFER_FOG(o,o.vertex);
			o.worldPos = mul(unity_ObjectToWorld, v.vertex).xy;

			/*
			#ifdef ENABLE_UICLIP
			float2 clipUV = o.vertex.xy;
			#if SHADER_API_METAL
			    #if UNITY_UV_STARTS_AT_TOP 
			        //clipUV.y = step(clipUV.y, 0)<1 ? -clipUV.y : clipUV.y
			    	clipUV.y = abs(clipUV.y);
			    #endif
			#endif
				float2 clipSpace = clipUV / o.vertex.w;
				o.posInPanel = (clipSpace.xy + 1) * 0.5;
				o.posInPanel.x -= _PanelCenterAndSharpness.x;
				o.posInPanel.y -= _PanelCenterAndSharpness.y;
				o.posInPanel.x *= (2 / _PanelSizeX);
				o.posInPanel.y *= (2 / _PanelSizeY);
			#endif
			*/
			
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
		
			fixed4 col = 2.0f * i.color * _TintColor * tex2D(_MainTex, i.texcoord);
			UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0,0,0,0)); // fog towards black due to our blend mode

		#ifdef ENABLE_UICLIP

			if(_Area.x!=0 || _Area.y!=0 || _Area.z!=0 || _Area.w!=0)
			{
				bool flag = i.worldPos.x >= _Area.x && i.worldPos.x <= _Area.z && i.worldPos.y >= _Area.y && i.worldPos.y <= _Area.w;
				if(flag == false)
				{
					return fixed4(0,0,0,0);
				}
			}
			
			
			//float2 k = float2(_PanelCenterAndSharpness.z, _PanelCenterAndSharpness.w) * 0.5;
			//float2 factor = (float2(1.0, 1.0) - abs(i.posInPanel)) * _PanelCenterAndSharpness.zw * 0.5;
			//col.a *= clamp(min(factor.x, factor.y), 0.0, 1.0);
		#endif
	
			return col;
		}

		fixed4 fragAlpha (v2f i) : SV_Target
		{
			fixed alpha = tex2D(_MainTex, i.texcoord).a * tex2D(_MainTex, i.texcoord).r;
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
				BlendOp max

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment fragAlpha
				
				ENDCG
			}
		}	
	}
}