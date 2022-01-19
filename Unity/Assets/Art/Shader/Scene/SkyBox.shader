Shader "YanJia/Scene/SkyBox"
{
	Properties 
	{
		_FrontTex ("Front (+Z)", 2D) = "white" {}
		_BackTex ("Back (-Z)", 2D) = "white" {}
		_LeftTex ("Left (+X)", 2D) = "white" {}
		_RightTex ("Right (-X)", 2D) = "white" {}
		_UpTex ("Up (+Y)", 2D) = "white" {}
		_DownTex ("Down (-Y)", 2D) = "white" {}
		_BloomPower ("Bloom Power", Range(0,1)) = 0.1
	}

	SubShader
	{
		Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }

		Cull Off ZWrite Off Fog { Mode Off }

		Pass
		{
			SetTexture [_FrontTex]
			{
            	constantColor (1,1,1,[_BloomPower])
            	combine texture, constant 
			}
		}
		Pass
		{
			SetTexture [_BackTex]			
			{
            	constantColor (1,1,1,[_BloomPower])
            	combine texture, constant 
			}
		}
		Pass
		{
			SetTexture [_LeftTex] 			
			{
            	constantColor (1,1,1,[_BloomPower])
            	combine texture, constant 
			}
		}
		Pass
		{
			SetTexture [_RightTex]			
			{
            	constantColor (1,1,1,[_BloomPower])
            	combine texture, constant 
			}
		}
		Pass
		{
			SetTexture [_UpTex]			
			{
            	constantColor (1,1,1,[_BloomPower])
            	combine texture, constant 
			}
		}
		Pass
		{
			SetTexture [_DownTex]			
			{
            	constantColor (1,1,1,[_BloomPower])
            	combine texture, constant 
			}
		}
	}
}
