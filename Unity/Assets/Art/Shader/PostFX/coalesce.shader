﻿//间单的美颜功能 
Shader "Custom/coalesce" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_BackTex("backTex (RGB)",2D) = "white"{}
		_Opacity("Blend Opacity",Range(0,1)) = 1
		//Hue的值范围为0-359. 其他两个为0-1 ,这里我们设置到3，因为乘以3后 都不一定能到超过.
		_Hue("Hue", Range(0,359)) = 0
		_Saturation("Saturation", Range(0,3.0)) = 1.0
		_Value("Value", Range(0,3.0)) = 1.0
	}
	SubShader{
		   Pass{


		
				Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
				LOD 100
		
				ZWrite On
				Blend SrcAlpha OneMinusSrcAlpha
				// ZTest Always 
		
		
		
				CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#include "UnityCG.cginc"
				
		
				sampler2D _MainTex;
				sampler2D _BackTex;
				fixed _Opacity;
				float4 _MainTex_ST;
				half _Hue;
				half _Saturation;
				half _Value;

				struct Input {
					float2 uv_MainTex;
				};
				//RGB to HSV
				float3 RGBConvertToHSV(float3 rgb)
				{
					float R = rgb.x, G = rgb.y, B = rgb.z;
					float3 hsv;
					float max1 = max(R, max(G, B));
					float min1 = min(R, min(G, B));
					if (R == max1)
					{
						hsv.x = (G - B) / (max1 - min1);
					}
					if (G == max1)
					{
						hsv.x = 2 + (B - R) / (max1 - min1);
					}
					if (B == max1)
					{
						hsv.x = 4 + (R - G) / (max1 - min1);
					}
					hsv.x = hsv.x * 60.0;
					if (hsv.x < 0)
						hsv.x = hsv.x + 360;
					hsv.z = max1;
					hsv.y = (max1 - min1) / max1;
					return hsv;
				}

				//HSV to RGB
				float3 HSVConvertToRGB(float3 hsv)
				{
					float R, G, B;
					//float3 rgb;
					if (hsv.y == 0)
					{
						R = G = B = hsv.z;
					}
					else
					{
						hsv.x = hsv.x / 60.0;
						int i = (int)hsv.x;
						float f = hsv.x - (float)i;
						float a = hsv.z * (1 - hsv.y);
						float b = hsv.z * (1 - hsv.y * f);
						float c = hsv.z * (1 - hsv.y * (1 - f));
						if (i == 0)
						{
							R = hsv.z; G = c; B = a;
						}
						else
							if (i == 1)
							{
								R = b; G = hsv.z; B = a;
							}
							else
								if (i == 2)
								{
									R = a; G = hsv.z; B = c;
								}
								else
									if (i == 3)
									{
										R = a; G = b; B = hsv.z;
									}
									else
										if (i == 4)
										{
											R = c; G = a; B = hsv.z;
										}
										else
										{
											R = hsv.z; G = a; B = b;
										}
					}
					return float3(R, G, B);
				}

				fixed OverlayBlendMode(fixed basePixel, fixed blendPixel) {
					if (basePixel < 0.5) {
						return (2.0 * basePixel * blendPixel);
					}
					else {
						return (1.0 - 2.0 * (1.0 - basePixel) * (1.0 - blendPixel));
					}
				}

				fixed4 frag(v2f_img i) : SV_Target
				{

					fixed4 renderTex = tex2D(_MainTex, i.uv);
					fixed4 blendTex = tex2D(_BackTex, i.uv);

					fixed4 blendedImage = renderTex;

					blendedImage.r = OverlayBlendMode(renderTex.r, blendTex.r);
					blendedImage.g = OverlayBlendMode(renderTex.g, blendTex.g);
					blendedImage.b = OverlayBlendMode(renderTex.b, blendTex.b);


					float3 colorHSV;
					colorHSV.xyz = RGBConvertToHSV(renderTex.xyz);   //转换为HSV
					colorHSV.x += _Hue; //调整偏移Hue值
					colorHSV.x = colorHSV.x % 360;    //超过360的值从0开始

					colorHSV.y *= _Saturation;  //调整饱和度
					colorHSV.z *= _Value;

					renderTex.xyz = HSVConvertToRGB(colorHSV.xyz);   //将调整后的HSV，转换为RGB颜色
					UNITY_APPLY_FOG(i.fogCoord, renderTex);



					renderTex = lerp(renderTex, blendedImage, _Opacity);
					

					return  renderTex;
					
				}
			ENDCG
		}
	}
		FallBack Off
}