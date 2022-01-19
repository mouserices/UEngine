Shader "YanJia/Object"
{
	Properties
	{
		//[KeywordEnum(Opaque, Transparent)] _Mode ("Mode",Float) = 0
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color Tint", Color) = (1, 1, 1, 1)
		_EmissionColor("Emmission Color", Color) = (1, 1, 1) 
		_Emmission("Emmission", Float) = 0
		_BloomPower ("Bloom Power", Range(0,1)) = 0.1

		[Toggle(ENABLE_FOG)] _EnableFog ("Enable fog", Float) = 0
		[Toggle(ENABLE_CUSTOM_FOG)] _EnableCustomFog ("Enable Custom fog", Float) = 0
		[Toggle(ENABLE_FAKE_LIGHTING)]_FakeLight("FakeLight", Float) = 0
		_FakeLightColor("Fake Light Color", COLOR) = (0.765, 0.765, 0.765)
		_FakeLightDir("FakeLightDir", Vector) = (-0.430, 0.766, -0.478, 0)

		[Toggle(ENABLE_DIFFUSE)]_EnableDiffuse("Enable diffuse", Float) = 0
		_DiffusePower ("Diffuse Power", Float) = 1

		[Toggle(ENABLE_SPECULAR)]_EnableSpecular("Enable Specular", Float) = 0
		_SpecularColor ("Specular Color", Color) = (1, 1, 1)
		_SpecularPower ("Specular Power", Float) = 1
		_Gloss ("Gloss", Float) = 8

		[Toggle(ENABLE_FRESNEL)]_EnableFresnel("Enable Fresnel", Float) = 0
		_FresnelColor ("Fresnel Color", Color) = (1, 1, 1)
		_FresnelPower ("Fresnel Power", Float) = 1
		_FresnelScale ("Fresnel Scale", Float) = 0.03

		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend Mode", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Blend Mode", Float) = 1
		[Enum(Off, 0, On, 1)] _ZWrite ("ZWrite", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Float) = 0
		[Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull Mode", Float) = 2

		[Toggle(ENABLE_MATCAP)]_EnableMatCap("Enable MatCap", Float) = 0
		_MetalTex("Metal Tex", 2D) = "white" {}
		_EnvTex ("Cube env tex", CUBE) = "black" {}
		_Spread("Spread", Range (0.01,1.0)) = 0.5
	}

	SubShader
	{		
		Tags { "Queue"="Geometry"  "RenderType"="Opaque"}

		Pass
		{ 
			Tags { "LightMode"="ForwardBase" }

			//Blend [_SrcBlend] [_DstBlend]
			//ZWrite [_ZWrite]
			//ZTest [_ZTest]
			Cull [_Cull]

			CGPROGRAM
			
			#pragma shader_feature ENABLE_FAKE_LIGHTING
			#pragma multi_compile _ ENABLE_DIFFUSE
			#pragma shader_feature ENABLE_SPECULAR
			#pragma shader_feature ENABLE_FRESNEL
			#pragma multi_compile _ ENABLE_FOG
			#pragma multi_compile _ ENABLE_CUSTOM_FOG
			#pragma multi_compile _ ENABLE_MATCAP

			#pragma multi_compile_fwdbase
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#define OBJECT_PASS_FORWARD_BASE
            #include "CustomShader.cginc"
		
			ENDCG
		}


		Pass
        {
            Name "Meta"
            Tags { "LightMode" = "Meta" }

            Cull Off

            CGPROGRAM
            //#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
            //#pragma skip_variants INSTANCING_ON
            #pragma vertex vert_meta
            #pragma fragment frag_meta

            #define OBJECT_PASS_META
            #include "CustomShader.cginc"
            ENDCG
        }

		Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZTest LEqual
            ZWrite On

            CGPROGRAM

            #pragma skip_variants SHADOWS_SOFT
            #pragma multi_compile_shadowcaster
            #pragma vertex vert   
            #pragma fragment frag
            
			#define OBJECT_PASS_SHADOWCASTER
			#include "CustomShader.cginc"
           
            ENDCG
        }

	}
	FallBack Off

	CustomEditor "CustomShaderEditor"
}
