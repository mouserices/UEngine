Shader "YanJia/UI/AddtiveClip" 
{
	Properties
	{
		_TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex("Particle Texture", 2D) = "white" {}
		_Brightness("Brightness", float) = 2.0
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 4 //"less"

		//_PanelSizeX("_PanelSizeX", Range(0.000000,300.000000)) = 300.000000
		//_PanelSizeY("_PanelSizeY", Range(0.000000,300.000000)) = 300.000000
		//_PanelCenterAndSharpness("_PanelCenterAndSharpness", Vector) = (0,0,300,300)
	}

		Category
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
			}

			Cull Off
			Lighting Off
			ZWrite Off
			ZTest[_ZTest]
			Fog{ Mode Off }
			ColorMask RGB
			AlphaTest Greater .01
			Blend SrcAlpha One

			SubShader
			{
				Pass
				{
					CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma multi_compile_particles

					#include "UnityCG.cginc"

					sampler2D _MainTex;
					fixed4 _TintColor;
					fixed _Brightness;
					//float4 _PanelCenterAndSharpness;
					//float _PanelSizeX;
					//float _PanelSizeY;

					float4 _Area;

					struct appdata
					{
						float4 vertex : POSITION;
						fixed4 color : COLOR;
						float2 uv : TEXCOORD0;
					};

					struct v2f
					{
						float4 vertex : SV_POSITION;
						fixed4 color : COLOR;
						float2 uv : TEXCOORD0;
						//float2 posInPanel : TEXCOORD1;
						float2 worldPos : TEXCOORD1;
					};

					float4 _MainTex_ST;

					v2f vert(appdata v)
					{
						v2f o;

						//o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
						//o.color = v.color;
						//o.uv = v.uv;
						//o.worldPos = v.vertex.xy * _ClipRange0.zw + _ClipRange0.xy;


						o.vertex = UnityObjectToClipPos(v.vertex);
						o.worldPos = mul(unity_ObjectToWorld, v.vertex).xy;
						o.color = v.color;
						o.uv = TRANSFORM_TEX(v.uv, _MainTex);

						/*
						float2 clipUV = o.vertex.xy;
						#if SHADER_API_METAL
			                #if UNITY_UV_STARTS_AT_TOP 
			                    //clipUV.y = step(clipUV.y, 0)<1 ? -clipUV.y : clipUV.y
			    	            clipUV.y = abs(clipUV.y);
			                #endif
			            #endif
						float2 clipSpace = clipUV / o.vertex.w;	

						// Normalize clip space  

						o.posInPanel = (clipSpace.xy + 1) * 0.5;

						// Adjust for panel offset  
						o.posInPanel.x -= _PanelCenterAndSharpness.x;
						o.posInPanel.y -= _PanelCenterAndSharpness.y;

						// Adjust for panel size  
						o.posInPanel.x *= (2 / _PanelSizeX);
						o.posInPanel.y *= (2 / _PanelSizeY);
						*/

						return o;
					}

					half4 frag(v2f IN) : SV_TARGET
					{
						//float2 k = float2(_PanelCenterAndSharpness.z, _PanelCenterAndSharpness.w) * 0.5;
						// Softness factor
						//float2 factor = (float2(1.0, 1.0) - abs(IN.posInPanel)) * _PanelCenterAndSharpness.zw * 0.5;
						//  (float2(1, 1) - abs(vx’, vy’)) * (pw’ / sx, ph’ / sy) 
						//                                    pw’ = 0.5 * pw, ph’ = 0.5 * ph
						// Sample the texture
						//col.a *= clamp(min(factor.x, factor.y), 0.0, 1.0);
						if(_Area.x!=0 || _Area.y!=0 || _Area.z!=0 || _Area.w!=0)
						{
							bool flag = IN.worldPos.x >= _Area.x && IN.worldPos.x <= _Area.z && IN.worldPos.y >= _Area.y && IN.worldPos.y <= _Area.w;
							if(!flag)
							{
								return fixed4(0,0,0,0);
							}
						}
						
						
						half4 col = _Brightness * tex2D(_MainTex, IN.uv) * _TintColor * IN.color;
						return  col;
					}
					ENDCG
				}
			}
		}
}