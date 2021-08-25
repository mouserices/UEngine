Shader "YanJia/Charater/HologramEffect"
{
    Properties
    {
        _HologramColor("Hologram Color", Color) = (1, 1, 1, 0)
		_HologramAlpha("Hologram Alpha", Range(0.0, 1.0)) = 1.0
		
		// 主纹理充当颜色蒙版
		_HologramMaskMap("Hologram Mask", 2D) = "white"{}
		_HologramMaskAffect("Hologram Mask Affect", Range(0.0, 1.0)) = 0.5
		
		// 全息抖动参数设置，x代表速度，y代表抖动范围，z代表抖动偏移量，w代表频率(0~0.99)
        _HologramGliterData1("Hologram Gliter Data1", Vector) = (0, 0.1, 0, 0) 
		_HologramGliterData2("Hologram Gliter Data2", Vector) = (0, 0.1, 0, 0)
        
        // 扫描线
		_HologramLine1("HologramLine1", 2D) = "white" {}
		_HologramLine1Speed("Hologram Line1 Speed", Range(-10.0, 10.0)) = 1.0
		_HologramLine1Frequency("Hologram Line1 Frequency", Range(0.0, 100.0)) = 20.0
		_HologramLine1Alpha("Hologram Line 1 Alpha", Range(0.0, 1.0)) = 0.15

		[Toggle(_USE_SCANLINE2)]_HologramLine2Tog("Hologram Line2 Toggle", float) = 0.0
		_HologramLine2("HologramLine2", 2D) = "white" {}
		_HologramLine2Speed("Hologram Line2 Speed", Range(-10.0, 10.0)) = 1.0
		_HologramLine2Frequency("Hologram Line2 Frequency", Range(0.0, 100.0)) = 20.0
		_HologramLine2Alpha("Hologram Line 2 Alpha", Range(0.0, 1.0)) = 0.15
    
         // 全息菲涅尔
		_FresnelScale("Fresnel Scale", Float) = 1
		_FresnelPower("Fresnel Power", Float) = 2
		
		
		_HologramNoiseMap("Hologram Noise Map", 2D) = "white"{}
		// 颗粒效果
        // xy：噪声采样tilling(需要噪声图)，zw：噪声颜色区间(0~1)
		_HologramGrainData("Hologram Grain Data", Vector) = (20, 20, 0, 1)
		_HologramGrainSpeed("Hologram Grain Speed", Float) = 1.0
		_HologramGrainAffect("Hologram Grain Affect", Range(0 , 1)) = 1
		
		
		// 全息颜色故障效果
		[Toggle] _HologramColorGlitchTog("Enable Hologram Color Glitch", Float) = 0
        // 噪声速度(使用XY分量)
		_HologramColorGlitch("Hologram Color Glitch", Range(0.0, 1.0)) = 0.5
		_HologramColorGlitchData("Hologram Color Glitch Data", Vector) = (1, 1, 0, 0)
		_HologramColorGlitchMin("Hologram Color Glitch Min", Range(0.0, 1.0)) = 0.5


    }
    SubShader
    {
        Tags{"Queue" = "Transparent" "RenderType" = "Transparent"}
        CGINCLUDE
			struct a2v_hg
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal:NORMAL;
            };
            
            struct v2f_hg
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            
            //Base
            float4 _HologramColor;
            fixed _HologramAlpha;
            
            // 全息蒙版
            sampler2D _HologramMaskMap;
            float4 _HologramMaskMap_ST;
            half _HologramMaskAffect;
            
            // 全息扫描线
            sampler2D _HologramLine1;
            half _HologramLine1Speed, _HologramLine1Frequency, _HologramLine1Alpha;
            sampler2D _HologramLine2;
            half _HologramLine2Speed, _HologramLine2Frequency, _HologramLine2Alpha;
            
            // 全息菲涅尔
            half _FresnelScale, _FresnelPower;
            
            sampler2D _HologramNoiseMap;
            // 全息颜色颗粒
            half4 _HologramGrainData;
            half _HologramGrainSpeed, _HologramGrainAffect;
            
            // 采样噪声图
            float SampleNoiseMap(float2 uv)
            {
                return tex2D(_HologramNoiseMap, uv).r;
            }
            
            // 颜色故障效果
            half _HologramColorGlitchTog, _HologramColorGlitch, _HologramColorGlitchMin;
            half4 _HologramColorGlitchData;

            //顶点偏移（故障效果）
            half4 _HologramGliterData1, _HologramGliterData2;
            half3 VertexHologramOffset(float3 vertex, half4 offsetData)
            {
                half speed = offsetData.x;
                half range = offsetData.y;
                half offset = offsetData.z;
                half frequency = offsetData.w;

                half offset_time = sin(_Time.y * speed);
                // step(y, x) 如果 x >= y 则返回1，否则返回0，用来决定在正弦时间的某个地方才开始进行顶点抖动
                half timeToGliter = step(frequency, offset_time);
                half gliterPosY = sin(-vertex.x + _Time.z);
                
                float gliterPosYRange = (float)(step(0, gliterPosY) * step(gliterPosY, range));
                // 获取偏移量
                float res = gliterPosYRange * offset * timeToGliter * gliterPosY;

                // 将这个偏移量定义为视角坐标的偏移量，再转到模型坐标
                float3 view_offset = float3(res, 0, 0);
                return mul((float3x3)UNITY_MATRIX_T_MV, view_offset);
            }
            
            // 因为顶点深入写入的Pass也是使用跟全息Pass一样的顶点函数，所以挪到这边来
            v2f_hg HologramVertex(a2v_hg v)
            {
                v2f_hg o;
                // 产生模型顶点方向上的扭曲系数
                v.vertex.xyz += VertexHologramOffset(v.vertex.xyz, _HologramGliterData1);
                v.vertex.xyz += VertexHologramOffset(v.vertex.xyz, _HologramGliterData2);
                // o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_VP, o.posWorld);
                o.normalDir = mul((float3x3)unity_ObjectToWorld, v.normal);
                return o;

            }
		ENDCG
		
		Pass
		{
			Name "Depth Mask"
			// 打开深度写入，并且设置颜色遮罩，0代表什么颜色都不输出
			ZWrite On 
			ColorMask 0

			CGPROGRAM
				#pragma target 3.0
				#pragma vertex HologramVertex
				#pragma fragment HologramMaskFragment

                float4 HologramMaskFragment(v2f_hg i) : SV_TARGET
                {
                    return 0;
                }
			ENDCG
		}
        
        Pass
        {
            Name "Hologram Effect"
            Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
            CGPROGRAM
				#pragma target 3.0
				#pragma shader_feature _USE_SCANLINE2
				#pragma vertex HologramVertex
				#pragma fragment HologramFragment

                float4 HologramFragment(v2f_hg i) : SV_Target
                {
                    float4 main_color = _HologramColor;
                    // 用主纹理的r通道做颜色蒙版
                    float2 mask_uv = i.uv.xy * _HologramMaskMap_ST.xy + _HologramMaskMap_ST.zw;
                    float4 mask = tex2D(_HologramMaskMap, mask_uv);
                    // 追加一个参数用来控制遮罩效果
                    float mask_alpha = lerp(1, mask.r, _HologramMaskAffect);
                    
                    
                    // 全息效果 扫描线
                    float2 line1_uv = (i.posWorld.y * _HologramLine1Frequency + _Time.y * _HologramLine1Speed).xx;
                    float line1 = clamp(tex2D(_HologramLine1, line1_uv).r, 0.0, 1.0);
                    float4 line1_color = float4((main_color * line1).rgb, line1) * _HologramLine1Alpha;
                    float line1_alpha = clamp(((main_color).a + (line1_color).w), 0.0 , 1.0);

                    #if defined (_USE_SCANLINE2)
                        float2 line2_uv = (i.posWorld.y * _HologramLine2Frequency + _Time.y * _HologramLine2Speed).xx;
                        float line2 = clamp(tex2D(_HologramLine2, line2_uv).r, 0.0, 1.0);
                        float4 line2_color = float4((main_color * line2).rgb, line2) * _HologramLine2Alpha;
                        float line2_alpha = clamp(((main_color).a + (line2_color).w), 0.0 , 1.0);
                    #else
                        float4 line2_color = 0.0;
                        float line2_alpha = 1.0;
                    #endif
                    
                    
                    // 菲涅尔反射
                    float3 w_viewDir = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                    float3 w_normal = normalize(i.normalDir);
                    float nDotV = dot(w_normal, w_viewDir);
                    float rim_nDotV = 1.0 - nDotV;
                    float4 fresnel = _FresnelScale * pow(rim_nDotV, _FresnelPower);
                    fresnel.a = clamp(fresnel.a, 0.0, 1.0);
                    float4 fresnel_color = (float4(fresnel.rgb, 1.0) * float4(main_color.rgb, 1.0)) * fresnel.a;


                    // 颗粒效果
                    float grain_noise = SampleNoiseMap((i.posWorld.xy * _HologramGrainData.xy + _Time.y * _HologramGrainSpeed));
                    float grain_amount = lerp(_HologramGrainData.z, _HologramGrainData.w, grain_noise) * _HologramGrainAffect;

                    float4 resultColor = float4(
                        // rgb
                        main_color.rgb + line1_color.rgb * line1_alpha + line2_color.rgb * line2_alpha + fresnel_color.rgb + grain_amount, 
                        // alpha
                        _HologramAlpha * mask_alpha
                    );
                    

                    // 颜色故障效果
                    float color_glicth_noise = SampleNoiseMap(float2(_Time.x * _HologramColorGlitchData.x, _Time.x * _HologramColorGlitchData.y));
                    color_glicth_noise = color_glicth_noise * (1.0 - _HologramColorGlitchMin) + _HologramColorGlitchMin;
                    color_glicth_noise = clamp(color_glicth_noise, 0.0, 1.0);
                    float color_glitch = lerp(1.0, color_glicth_noise, _HologramColorGlitch * _HologramColorGlitchTog);

                    // 应用全局颜色故障效果
                    resultColor *= color_glitch;
                    
                    return resultColor;
                }
			ENDCG
        }
    }
    
    CustomEditor "HologramEditor"
}
