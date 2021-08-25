Shader "YanJia/Charater/CrRoleDissolution"
{
    Properties 
	{
        _MainTex ("MainTex", 2D) = "white" {}
        _SpeShaTex ("SpeShaTex", 2D) = "white" {}
        _ShadowColor ("ShadowColor", Color) = (0.6663285,0.6544118,1,1)
        _ShadowRange ("ShadowRange", Range(0, 1)) = 0
        _ShadowIntensity ("ShadowIntensity", Range(0, 1)) = 0.7956449
		_ShadowSmooth("Shadow Smooth", Range(0, 0.5)) = 0.03
        _SpecularRange ("SpecularRange", Range(0.9, 1)) = 0.9
        _SpecularMult ("SpecularMult", Range(0, 3)) = 0
		_OutlineWidth ("Outline Width", Range(0, 1)) = 0.24
        _OutlineLightness ("OutlineLightness", Range(0, 1)) = 1
        _OutLineColor ("OutLineColor", Color) = (0.5,0.5,0.5,1)
        [Enum(Off, 0, On, 1)] _OutLine_ZWrite ("OutLine ZWrite", Float) = 1
        _rimPower ("Rim Power", Range(0, 20)) = 20
        _rimColorRight ("Rim Color", Color) = (1,1,1,1)
		_rimIntensity ("Rim Intensity", Range(0,1)) = 0.2
		//_AutoLigDir ("AutoLigDir", Float ) = 0
        _BloomPower ("Bloom Power", Range(0,1)) = 0.3
        [Toggle(ENABLE_FADE)]_EnableFade("Enable Fade", Float) = 0

        _NoiseTex("Noise", 2D) = "white" {}
		_Threshold("Threshold", Range(0.0, 1.0)) = 0
    }

    SubShader 
	{
		Pass 
		{
            Cull Front
            ZWrite  [_OutLine_ZWrite] 
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed _OutlineLightness;
            fixed _OutlineWidth;
            fixed4 _OutLineColor;

            sampler2D _NoiseTex;
			float4 _NoiseTex_ST;
			float _Threshold;

            struct a2v 
			{
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
			{
                float4 pos : SV_POSITION;
                float2 uvNoiseTex : TEXCOORD0;
            };

            v2f vert (a2v v) 
			{
                v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
                float outLineFade = 1;

                float4x4 P = unity_CameraProjection;
				float fov =  atan(1 / P._m11) * 2;
				float fovScale = degrees(fov) / 40;
                outLineFade *= pow(fovScale, 0.5);

                float3 posWorld = mul(unity_ObjectToWorld, v.vertex);
                float distance = length(_WorldSpaceCameraPos.xyz - posWorld);
                //outLineFade *= pow(distance / 20, 1) * 6;
                outLineFade *= pow(distance, 0.5);
                //outLineFade = max(1, outLineFade);

                float4 pos = mul( UNITY_MATRIX_MV, v.vertex);
                float3 normal = mul( (float3x3)UNITY_MATRIX_IT_MV, v.normal);
                normal.z = -0.5;
                outLineFade /= 100;
                pos = pos + float4(normalize(normal),0) * _OutlineWidth * outLineFade;
                o.pos = mul(UNITY_MATRIX_P, pos);
                o.uvNoiseTex = TRANSFORM_TEX(v.uv, _NoiseTex);
                //o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal * _OutlineWidth,1) );
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET 
			{
                fixed cutout = tex2D(_NoiseTex, i.uvNoiseTex).r;
				clip(cutout - _Threshold);
                return fixed4(_OutLineColor.rgb * _OutlineLightness, 0) ;
            }

            ENDCG
        }

        Pass 
		{
            Tags { "RenderType"="Opaque" "LightMode"="ForwardBase"}
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
			#include "Lighting.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile _ ENABLE_FADE

            fixed _SpecularRange;
            sampler2D _MainTex; 
			float4 _MainTex_ST;
            fixed _ShadowIntensity;
			fixed _ShadowSmooth;
            fixed _SpecularMult;
            sampler2D _SpeShaTex; 
			float4 _SpeShaTex_ST;
        	fixed4 _ShadowColor;
            fixed _ShadowRange;
			half _rimPower;
			fixed3 _rimColorRight;
			fixed _rimIntensity;
			//fixed _AutoLigDir;
			fixed4 _LigDir;
            fixed _BloomPower;

			sampler2D _NoiseTex;
			float4 _NoiseTex_ST;
			float _Threshold;

            struct a2v 
			{
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
			{
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
                float2 uvNoiseTex : TEXCOORD3;
            };

            v2f vert (a2v v)
			{
                v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uvNoiseTex = TRANSFORM_TEX(v.uv, _NoiseTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET 
			{
                fixed cutout = tex2D(_NoiseTex, i.uvNoiseTex).r;
				clip(cutout - _Threshold);

				fixed4 col = 0;
                fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                fixed3 worldNormal = normalize(i.worldNormal);
                fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
				//fixed3 worldLightDir = _AutoLigDir > 0.5 ? _LigDir : normalize(_WorldSpaceLightPos0.xyz);

                #if ENABLE_FADE
                    fixed3 lightColor = fixed3(0.5, 0.5, 0.5);
                #else
                    fixed3 lightColor = _LightColor0.rgb;
                #endif
                
				fixed NdotL = max(0,dot(worldNormal, worldLightDir));
				fixed NdotV = max(0,dot(worldNormal,viewDir));
				//fixed3 halfDir = normalize(worldLightDir + viewDir);
				//fixed NdotH = max(0,dot(worldNormal,halfDir));

                fixed4 lightTexColor = tex2D(_SpeShaTex,TRANSFORM_TEX(i.uv, _SpeShaTex));
                fixed4 baseTexColor = tex2D(_MainTex,TRANSFORM_TEX(i.uv, _MainTex));
				
                fixed3 diffuseColor = baseTexColor.rgb > 0.5 ? 1.0 - (2.0 - baseTexColor.rgb * 2) * (1.0 - lightTexColor.g) : 2.0 * baseTexColor.rgb * lightTexColor.g;
				diffuseColor = saturate(diffuseColor);
                fixed diffuseIf = smoothstep( 0.0, _ShadowSmooth, NdotL - (0.5 - lightTexColor.g) - _ShadowRange);
                fixed3 diffuseLightAera = diffuseColor * diffuseIf;
				fixed3 diffuse = diffuseLightAera + (diffuseColor *_ShadowColor.rgb *_ShadowIntensity) * (1.0 - diffuseIf);

                fixed specularIf = step(_SpecularRange,saturate(pow(NdotV, lightTexColor.r)));
				fixed3 Specular = _SpecularMult * specularIf * lightTexColor.b;

				fixed3 rim = 0;
				fixed f =  1.0 - saturate(dot(normalize(viewDir), worldNormal));
				rim = _rimColorRight.rgb * pow (f, _rimPower) * _rimIntensity;

                col.rgb = lightColor * (diffuse + Specular * diffuseLightAera + rim);
                col.a = lightTexColor.a * _BloomPower;
                return col;
            }
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
}
