Shader "Hidden/PostFx"
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Saturation("Saturation", Float) = 1
		IsFading("IsFading", Float) = 0
		Zhezhao ("Zhezhao", 2D) = "black" {}
		IsZheZhao("IsZheZhao", Float) = 0
	}

	SubShader
	{
		ZTest Off
		ZWrite Off
		Cull Off
		Blend Off


		Pass //0 Fade
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment fragFade
			#pragma fragmentoption ARB_precision_hint_fastest 
			#define IMG_0 sceneRT
			#include "PostFx.cginc"
			ENDCG
		}

		Pass //1
		{
	    	CGPROGRAM
	    	#pragma vertex vert
	    	#pragma fragment frag_bloom_setup
			#pragma multi_compile __ HDR_ON
			#pragma fragmentoption ARB_precision_hint_fastest 
			#define IMG_0 sceneRT
			#define PARAM_0 sceneRT_TexelSize
			#define PARAM_1 _BloomParams
			#include "PostFx.cginc"
	    	ENDCG
	  	}

		Pass //2
		{
	    	CGPROGRAM
	    	#pragma vertex vert_s4
	    	#pragma fragment frag_bloom_setup_s4
			#pragma multi_compile __ HDR_ON
			#pragma fragmentoption ARB_precision_hint_fastest 
			#define IMG_0 sceneRT
			#define PARAM_0 sceneRT_TexelSize
			#define PARAM_1 _BloomParams
			#include "PostFx.cginc"
	    	ENDCG
	  	}

		Pass //3
		{
	    	CGPROGRAM
	    	#pragma vertex vert_bloomdown
	    	#pragma fragment frag_bloomdown
			#pragma fragmentoption ARB_precision_hint_fastest 
			#define IMG_0 bloomSetup
			#define PARAM_0 bloomSetup_TexelSize
			#include "PostFx.cginc"
	    	ENDCG
	  	}

		Pass //4
		{
	    	CGPROGRAM
	    	#pragma vertex vert_bloomdown
	    	#pragma fragment frag_bloomdown
			#pragma fragmentoption ARB_precision_hint_fastest 
			#define IMG_0 bloomDown0
			#define PARAM_0 bloomDown0_TexelSize
			#include "PostFx.cginc"
	    	ENDCG
	  	}

		Pass //5
		{
	    	CGPROGRAM
	    	#pragma vertex vert_bloomdown
	    	#pragma fragment frag_bloomdown
			#pragma fragmentoption ARB_precision_hint_fastest 
			#define IMG_0 bloomDown1
			#define PARAM_0 bloomDown1_TexelSize
			#include "PostFx.cginc"
	    	ENDCG
	  	}

		Pass //6
		{
	    	CGPROGRAM
	    	#pragma vertex vert_bloomup
	    	#pragma fragment frag_bloomup
			#pragma fragmentoption ARB_precision_hint_fastest 
			#define IMG_0 bloomDown2
			#define IMG_1 bloomDown1
			#define PARAM_0 bloomDown2_TexelSize
			#define PARAM_1 bloomDown1_TexelSize
			#define PARAM_2 bloomHint0
			#define PARAM_3 bloomHint1
			#include "PostFx.cginc"
	    	ENDCG
	  	}

		Pass //7
		{
	    	CGPROGRAM
	    	#pragma vertex vert_bloomup
	    	#pragma fragment frag_bloomup
			#pragma fragmentoption ARB_precision_hint_fastest 
			#define IMG_0 bloomUp0
			#define IMG_1 bloomDown0
			#define PARAM_0 bloomUp0_TexelSize
			#define PARAM_1 bloomDown0_TexelSize
			#define PARAM_2 bloomHint2
			#define PARAM_3 bloomHint3
			#include "PostFx.cginc"
	    	ENDCG
	  	}

		Pass //8
		{
	    	CGPROGRAM
	    	#pragma vertex vert_bloom_merge
	    	#pragma fragment frag_bloom_merge
			#pragma fragmentoption ARB_precision_hint_fastest 
			#define BLOOM_MERGE
			#include "PostFx.cginc"
	    	ENDCG
	  	}

        Pass //9
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_final_merge
			#pragma fragmentoption ARB_precision_hint_fastest 
            #define FINAL_OUTPUT
            #define IMG_0 sceneRT
            #pragma multi_compile __ LUT_ON
            #pragma multi_compile __ HDR_ON
            #pragma multi_compile __ BLOOM_ON
            #include "PostFx.cginc"
            ENDCG
        }

		Pass //10
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag_chromatic_aberration
			#pragma fragmentoption ARB_precision_hint_fastest 
			#define IMG_0 sceneRT
			#include "PostFx.cginc"
			ENDCG
		}

		Pass //11
		{
			CGPROGRAM
			#pragma vertex vert_bloomdown
			#pragma fragment frag_bloomdown
			#pragma fragmentoption ARB_precision_hint_fastest 
			#define IMG_0 sceneRT
			#define PARAM_0 sceneRT_TexelSize
			#include "PostFx.cginc"
			ENDCG
		}

		Pass //12
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag_dof
			#pragma fragmentoption ARB_precision_hint_fastest 
			#define IMG_0 sceneRT
			#define PARAM_0 sceneRT_TexelSize
			#define IMG_1 bloomTex
			#define PARAM_1 bloomTex_TexelSize
			#include "PostFx.cginc"
			ENDCG
		}

		Pass //13 FXAA
		{
			CGPROGRAM
			#pragma vertex VertexProgram
			#pragma fragment FragmentProgram
			#pragma fragmentoption ARB_precision_hint_fastest 
			#define IMG_0 sceneRT
			#define PARAM_0 sceneRT_TexelSize
			#include "PostFx.cginc"

			float4 FragmentProgram(Interpolators i) : SV_Target
			{
				float4 col = ApplyFXAA(i.uv);
				return col;
			}
			ENDCG
		}

		Pass //14
		{
			CGPROGRAM
			#pragma vertex vert_planar_blur
			#pragma fragment frag_scale_s4
			#pragma fragmentoption ARB_precision_hint_fastest 
			#define IMG_0 mirrorShadowRES
			#define PARAM_0 mirrorShadowRES_TexelSize
			#include "PostFx.cginc"
			ENDCG
		}

		
		Pass //15
		{
	    	CGPROGRAM
	    	#pragma vertex vert_s4
	    	#pragma fragment frag_bloom_setup_s4_Replace
			#pragma multi_compile __ HDR_ON
			#pragma fragmentoption ARB_precision_hint_fastest 
			#define IMG_0 sceneRT
			#define IMG_1 replaceRT
			#define PARAM_0 sceneRT_TexelSize
			#define PARAM_1 _BloomParams
			#include "PostFx.cginc"
	    	ENDCG
	  	}

		Pass //16
		{
	    	CGPROGRAM
	    	#pragma vertex vert
	    	#pragma fragment fragRadialBlur
			#pragma fragmentoption ARB_precision_hint_fastest 
			#define IMG_0 downRT
			#define PARAM_0 blurParm
			#include "PostFx.cginc"
	    	ENDCG
	  	}

		Pass  //17
		{ 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment fragCombine
			#pragma fragmentoption ARB_precision_hint_fastest 
			#define IMG_0 sceneRT
			#define IMG_1 blurRt
			#define PARAM_0 blurParm
			#include "PostFx.cginc"
			ENDCG	 
		}
		
	    Pass  //18
		{ 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment fragShatter
			#pragma fragmentoption ARB_precision_hint_fastest 
			#define IMG_0 sceneRT			
			#define IMG_1 shatterTex
            #define PARAM_0 sceneRT_TexelSize
			#include "PostFx.cginc"
			ENDCG	 
		}
	}
}
