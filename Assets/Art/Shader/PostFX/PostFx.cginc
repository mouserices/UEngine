#include "UnityCG.cginc"

#ifdef IMG_0
sampler2D IMG_0;
#endif
#ifdef IMG_1
sampler2D IMG_1;
#endif
#ifdef IMG_2
sampler2D IMG_2;
#endif
#ifdef IMG_3
sampler2D IMG_3;
#endif

#ifdef PARAM_0
float4 PARAM_0;
#endif

#ifdef PARAM_1
float4 PARAM_1;
#endif

#ifdef PARAM_2
float4 PARAM_2;
#endif

#ifdef PARAM_3
float4 PARAM_3;
#endif


//half4 _ColorMix;
half _BloomIntensity;
half bloomRangeScale;
half IsFading;
half _Saturation;
half IsZheZhao;
sampler2D Zhezhao;

sampler2D_float _CameraDepthTexture;
float _focalDistance;
float _nearBlurScale;
float _farBlurScale;

half GetLuminance(half3 LinearColor)
{
	return dot(LinearColor, half3(0.299, 0.587, 0.114));
}

float2 Circle(float Start, float Points, float Point)
{
	float Rad = (3.141592653589 * 2.0 * (1.0 / Points)) * (Point + Start);
	return float2(sin(Rad), cos(Rad)) * bloomRangeScale;
}

struct v2f
{
	float4 pos : SV_POSITION;
	float2 uv  : TEXCOORD0;
};

struct v2f_s4
{
	float4 pos : SV_POSITION;
	float4 uv01  : TEXCOORD0;
	float4 uv23  : TEXCOORD1;
};

struct v2f_s7
{
	float4 pos : SV_POSITION;
	float4 uv01 : TEXCOORD1;
	float4 uv23 : TEXCOORD2;
	float4 uv45 : TEXCOORD3;
	float2 uv6 : TEXCOORD4;
};

struct v2f_s15
{
	float4 pos : SV_POSITION;
	half4 uv01 : TEXCOORD0;
	half4 uv23 : TEXCOORD1;
	half4 uv45 : TEXCOORD2;
	half4 uv67 : TEXCOORD3;
	half4 uv89 : TEXCOORD4;
	half4 uv0A : TEXCOORD5;
	half4 uvBC : TEXCOORD6;
	half2 uvD : TEXCOORD7;
};

v2f vert(appdata_img v)
{
	v2f o;
	o.pos = UnityObjectToClipPos(v.vertex);
	o.uv = v.vertex.xy;
	return o;
}

#if defined(PARAM_0)
v2f_s4 vert_s4(appdata_img v)
{
	v2f_s4 o;
	o.pos = UnityObjectToClipPos(v.vertex);
	o.uv01.xy = v.texcoord.xy + PARAM_0 * float2(-1, -1);
	o.uv01.zw = v.texcoord.xy + PARAM_0 * float2(1, -1);
	o.uv23.xy = v.texcoord.xy + PARAM_0 * float2(-1, 1);
	o.uv23.zw = v.texcoord.xy + PARAM_0 * float2(1, 1);
	return o;
}

v2f_s4 vert_planar_blur(appdata_img v)
{
	v2f_s4 o;
	o.pos = UnityObjectToClipPos(v.vertex);
	o.uv01.xy = v.texcoord.xy + PARAM_0 * float2(-1.5, -1.5);
	o.uv01.zw = v.texcoord.xy + PARAM_0 * float2(1.5, -1.5);
	o.uv23.xy = v.texcoord.xy + PARAM_0 * float2(-1.5, 1.5);
	o.uv23.zw = v.texcoord.xy + PARAM_0 * float2(1.5, 1.5);
	return o;
}

v2f_s15 vert_bloomdown(appdata_img v)
{
	v2f_s15 o;
	const float Start = 2.0 / 14.0;
	const float Scale = 2.64;
	o.pos = UnityObjectToClipPos(v.vertex);
	o.uv01.xy = v.texcoord.xy;
	o.uv01.zw = v.texcoord.xy + Circle(Start, 14.0, 0.0) * Scale * PARAM_0.xy;
	o.uv23.xy = v.texcoord.xy + Circle(Start, 14.0, 1.0) * Scale * PARAM_0.xy;
	o.uv23.zw = v.texcoord.xy + Circle(Start, 14.0, 2.0) * Scale * PARAM_0.xy;
	o.uv45.xy = v.texcoord.xy + Circle(Start, 14.0, 3.0) * Scale * PARAM_0.xy;
	o.uv45.zw = v.texcoord.xy + Circle(Start, 14.0, 4.0) * Scale * PARAM_0.xy;
	o.uv67.xy = v.texcoord.xy + Circle(Start, 14.0, 5.0) * Scale * PARAM_0.xy;
	o.uv67.zw = v.texcoord.xy + Circle(Start, 14.0, 6.0) * Scale * PARAM_0.xy;
	o.uv89.xy = v.texcoord.xy + Circle(Start, 14.0, 7.0) * Scale * PARAM_0.xy;
	o.uv89.zw = v.texcoord.xy + Circle(Start, 14.0, 8.0) * Scale * PARAM_0.xy;
	o.uv0A.xy = v.texcoord.xy + Circle(Start, 14.0, 9.0) * Scale * PARAM_0.xy;
	o.uv0A.zw = v.texcoord.xy + Circle(Start, 14.0, 10.0) * Scale * PARAM_0.xy;
	o.uvBC.xy = v.texcoord.xy + Circle(Start, 14.0, 11.0) * Scale * PARAM_0.xy;
	o.uvBC.zw = v.texcoord.xy + Circle(Start, 14.0, 12.0) * Scale * PARAM_0.xy;
	o.uvD.xy = v.texcoord.xy + Circle(Start, 14.0, 13.0) * Scale * PARAM_0.xy;
	return o;
}
#endif

#if defined(PARAM_0) && defined(PARAM_1)
v2f_s15 vert_bloomup(appdata_img v)
{
	v2f_s15 o;
	const float Start = 2.0 / 7.0;
	const float Scale = 1.32;
	o.pos = UnityObjectToClipPos(v.vertex);
	o.uv01.xy = v.texcoord.xy + Circle(Start, 7.0, 0.0) * Scale * PARAM_0.xy;
	o.uv01.zw = v.texcoord.xy + Circle(Start, 7.0, 1.0) * Scale * PARAM_0.xy;
	o.uv23.xy = v.texcoord.xy + Circle(Start, 7.0, 2.0) * Scale * PARAM_0.xy;
	o.uv23.zw = v.texcoord.xy + Circle(Start, 7.0, 3.0) * Scale * PARAM_0.xy;
	o.uv45.xy = v.texcoord.xy + Circle(Start, 7.0, 4.0) * Scale * PARAM_0.xy;
	o.uv45.zw = v.texcoord.xy + Circle(Start, 7.0, 5.0) * Scale * PARAM_0.xy;
	o.uv67.xy = v.texcoord.xy + Circle(Start, 7.0, 6.0) * Scale * PARAM_0.xy;
	o.uv67.zw = v.texcoord.xy;
	o.uv89.xy = v.texcoord.xy + Circle(Start, 7.0, 0.0) * Scale * PARAM_1.xy;
	o.uv89.zw = v.texcoord.xy + Circle(Start, 7.0, 1.0) * Scale * PARAM_1.xy;
	o.uv0A.xy = v.texcoord.xy + Circle(Start, 7.0, 2.0) * Scale * PARAM_1.xy;
	o.uv0A.zw = v.texcoord.xy + Circle(Start, 7.0, 3.0) * Scale * PARAM_1.xy;
	o.uvBC.xy = v.texcoord.xy + Circle(Start, 7.0, 4.0) * Scale * PARAM_1.xy;
	o.uvBC.zw = v.texcoord.xy + Circle(Start, 7.0, 5.0) * Scale * PARAM_1.xy;
	o.uvD.xy = v.texcoord.xy + Circle(Start, 7.0, 6.0) * Scale * PARAM_1.xy;
	return o;
}
#endif

#if defined(IMG_0)
half4 frag_scale_s1(v2f i) : SV_Target
{
	return tex2D(IMG_0, i.uv);
}
#endif

#if defined(IMG_0)
float chromaticIntensity;
half4 frag_chromatic_aberration(v2f i) : SV_Target
{
	float amount = 0.004 * chromaticIntensity;
	half3 color;
	color.r = tex2D(IMG_0, float2(i.uv.x + amount, i.uv.y)).r;
	color.g = tex2D(IMG_0, float2(i.uv.x, i.uv.y)).g;
	color.b = tex2D(IMG_0, float2(i.uv.x - amount, i.uv.y)).b;
	return half4(color, 1);
}
#endif

#if defined(IMG_0)
half4 frag_scale_s4(v2f_s4 i) : SV_Target
{
	half4 s = tex2D(IMG_0, i.uv01.xy);
	s += tex2D(IMG_0, i.uv01.zw);
	s += tex2D(IMG_0, i.uv23.xy);
	s += tex2D(IMG_0, i.uv23.zw);
	return s * 0.25;
}
#endif

#if defined(IMG_0) && defined(PARAM_0) && defined(PARAM_1)
half4 frag_bloom_setup(v2f i) : SV_Target
{
	half4 color = tex2D(IMG_0, i.uv);
	color.rgb *= color.rgb;
#ifdef HDR_ON
	half threshold = PARAM_1.z;
#else
	half threshold = PARAM_1.x;
#endif
	half amount = saturate(GetLuminance(color.rgb) - threshold);
	color.rgb *= amount;
	return color;
}
#endif

#if defined(IMG_0) && defined(PARAM_0) && defined(PARAM_1)
half4 frag_bloom_setup_s4(v2f_s4 i) : SV_Target
{
	half4 color;
	half4 C0 = tex2D(IMG_0, i.uv01.xy);
	half4 C1 = tex2D(IMG_0, i.uv01.zw);
	half4 C2 = tex2D(IMG_0, i.uv23.xy);
	half4 C3 = tex2D(IMG_0, i.uv23.zw);
	color = (C0 * 0.25) + (C1 * 0.25) + (C2 * 0.25) + (C3 * 0.25);
	color.rgb *= color.rgb;
#ifdef HDR_ON
	half threshold = PARAM_1.z;
#else
	half threshold = PARAM_1.x;
#endif
	color.rgb = max(0, (color.rgb - threshold) * color.a * _BloomIntensity);
	return color;
}
#endif

#if defined(IMG_0) && defined(IMG_1) &&defined(PARAM_0) && defined(PARAM_1)
half4 frag_bloom_setup_s4_Replace(v2f_s4 i) : SV_Target
{
	half4 color;
	half4 rcolor;
	half4 C0 = tex2D(IMG_0, i.uv01.xy);
	half4 C1 = tex2D(IMG_0, i.uv01.zw);
	half4 C2 = tex2D(IMG_0, i.uv23.xy);
	half4 C3 = tex2D(IMG_0, i.uv23.zw);
	half4 rC0 = tex2D(IMG_1, i.uv01.xy);
	half4 rC1 = tex2D(IMG_1, i.uv01.zw);
	half4 rC2 = tex2D(IMG_1, i.uv23.xy);
	half4 rC3 = tex2D(IMG_1, i.uv23.zw);
	color = (C0 * 0.25) + (C1 * 0.25) + (C2 * 0.25) + (C3 * 0.25);
	rcolor = (rC0 * 0.25) + (rC1 * 0.25) + (rC2 * 0.25) + (rC3 * 0.25);
	color.a = rcolor.r > 0 ? rcolor.g : color.a;
	color.rgb *= color.rgb;
#ifdef HDR_ON
	half threshold = PARAM_1.z;
#else
	half threshold = PARAM_1.x;
#endif
	color.rgb = max(0, (color.rgb - threshold) * color.a * _BloomIntensity);
	return color;
}
#endif

#if defined(IMG_0) && defined(PARAM_0)
half4 frag_bloomdown(v2f_s15 i) : SV_Target
{
	half4 N0 = tex2D(IMG_0, i.uv01.xy);
	half4 N1 = tex2D(IMG_0, i.uv01.zw);
	half4 N2 = tex2D(IMG_0, i.uv23.xy);
	half4 N3 = tex2D(IMG_0, i.uv23.zw);
	half4 N4 = tex2D(IMG_0, i.uv45.xy);
	half4 N5 = tex2D(IMG_0, i.uv45.zw);
	half4 N6 = tex2D(IMG_0, i.uv67.xy);
	half4 N7 = tex2D(IMG_0, i.uv67.zw);
	half4 N8 = tex2D(IMG_0, i.uv89.xy);
	half4 N9 = tex2D(IMG_0, i.uv89.zw);
	half4 N10 = tex2D(IMG_0, i.uv0A.xy);
	half4 N11 = tex2D(IMG_0, i.uv0A.zw);
	half4 N12 = tex2D(IMG_0, i.uvBC.xy);
	half4 N13 = tex2D(IMG_0, i.uvBC.zw);
	half4 N14 = tex2D(IMG_0, i.uvD.xy);
	half W = 1.0 / 15.0;
	half4 color;
	color.rgb = (N0 * W) +
		(N1 * W) +
		(N2 * W) +
		(N3 * W) +
		(N4 * W) +
		(N5 * W) +
		(N6 * W) +
		(N7 * W) +
		(N8 * W) +
		(N9 * W) +
		(N10 * W) +
		(N11 * W) +
		(N12 * W) +
		(N13 * W) +
		(N14 * W);
	return color;
}
#endif

#if defined(IMG_0) && defined(IMG_1) && defined(PARAM_0) && defined(PARAM_1) && defined(PARAM_2) && defined(PARAM_3)
half4 frag_bloomup(v2f_s15 i) : SV_Target
{
	half4 A0 = tex2D(IMG_0, i.uv01.xy);
	half4 A1 = tex2D(IMG_0, i.uv01.zw);
	half4 A2 = tex2D(IMG_0, i.uv23.xy);
	half4 A3 = tex2D(IMG_0, i.uv23.zw);
	half4 A4 = tex2D(IMG_0, i.uv45.xy);
	half4 A5 = tex2D(IMG_0, i.uv45.zw);
	half4 A6 = tex2D(IMG_0, i.uv67.xy);
	half4 A7 = tex2D(IMG_0, i.uv67.zw);

	half3 B0 = tex2D(IMG_1, i.uv67.zw);
	half3 B1 = tex2D(IMG_1, i.uv89.xy);
	half3 B2 = tex2D(IMG_1, i.uv89.zw);
	half3 B3 = tex2D(IMG_1, i.uv0A.xy);
	half3 B4 = tex2D(IMG_1, i.uv0A.zw);
	half3 B5 = tex2D(IMG_1, i.uvBC.xy);
	half3 B6 = tex2D(IMG_1, i.uvBC.zw);
	half3 B7 = tex2D(IMG_1, i.uvD.xy);

	half3 WA = PARAM_2.rgb;
	half3 WB = PARAM_3.rgb;

	half4 color;
	color.rgb =
		A0 * WA +
		A1 * WA +
		A2 * WA +
		A3 * WA +
		A4 * WA +
		A5 * WA +
		A6 * WA +
		A7 * WA +
		B0 * WB +
		B1 * WB +
		B2 * WB +
		B3 * WB +
		B4 * WB +
		B5 * WB +
		B6 * WB +
		B7 * WB;

	return color;
}
#endif

#ifdef BLOOM_MERGE
sampler2D bloomUp1;
float4 bloomUp1_TexelSize;
#define IMG_0 bloomUp1
#define PARAM_0 bloomUp1_TexelSize
v2f_s7 vert_bloom_merge(appdata_img v)
{
	v2f_s7 o;
	const float Start = 2.0 / 6.0;
	const float Scale = 0.66 / 2.0;
	o.pos = UnityObjectToClipPos(v.vertex);
	o.uv01.xy = v.texcoord.xy;
	o.uv01.zw = v.texcoord.xy + Circle(Start, 6.0, 0.0) * Scale * PARAM_0.xy;
	o.uv23.xy = v.texcoord.xy + Circle(Start, 6.0, 1.0) * Scale * PARAM_0.xy;
	o.uv23.zw = v.texcoord.xy + Circle(Start, 6.0, 2.0) * Scale * PARAM_0.xy;
	o.uv45.xy = v.texcoord.xy + Circle(Start, 6.0, 3.0) * Scale * PARAM_0.xy;
	o.uv45.zw = v.texcoord.xy + Circle(Start, 6.0, 4.0) * Scale * PARAM_0.xy;
	o.uv6 = v.texcoord.xy + Circle(Start, 6.0, 5.0) * Scale * PARAM_0.xy;
	return o;
}

half4 frag_bloom_merge(v2f_s7 i) : SV_Target
{
	const float W = 1.0 / 7.0;
	half4 N0 = tex2D(IMG_0, i.uv01.xy);
	half4 N1 = tex2D(IMG_0, i.uv01.zw);
	half4 N2 = tex2D(IMG_0, i.uv23.xy);
	half4 N3 = tex2D(IMG_0, i.uv23.zw);
	half4 N4 = tex2D(IMG_0, i.uv45.xy);
	half4 N5 = tex2D(IMG_0, i.uv45.zw);
	half4 N6 = tex2D(IMG_0, i.uv6);
	half4 color = 0;
	color.rgb = (N0 * W) +
		(N1 * W) +
		(N2 * W) +
		(N3 * W) +
		(N4 * W) +
		(N5 * W) +
		(N6 * W);	
	return color;
}
#endif

#ifdef FINAL_OUTPUT

#if defined(HDR_ON)
sampler2D bloomMerge;
half4 _HDRParams;
half3 ACESFilm(half3 x)
{
	half a = 2.51f;
	half b = 0.03f;
	half c = 2.43f;
	half d = 0.59f;
	half e = 0.14f;
	return saturate((x*(a*x + b)) / (x*(c*x + d) + e));
}
#elif defined(BLOOM_ON)
sampler2D bloomMerge;
half4 _BloomParams;
#endif

#if defined(LUT_ON)
sampler3D colorLUT;
#endif

half4 frag_final_merge(v2f i) : SV_Target
{
	half3 color = tex2D(IMG_0, i.uv).rgb;
	color *= color;

#if defined(HDR_ON)
	color += max(tex2D(bloomMerge, i.uv).rgb, 0) * _HDRParams.y;
	color = ACESFilm(_HDRParams.z * color);
#elif defined(BLOOM_ON)
	color += max(tex2D(bloomMerge, i.uv).rgb, 0) * _BloomParams.y;
#endif
	color = sqrt(color);
#if defined(LUT_ON)
	const half chartDim = 32.0;
	const half3 scale = half3(chartDim - 1.0, chartDim - 1.0, chartDim - 1.0) / chartDim;
	const half3 bias = half3(0.5, 0.5, 0.5) / chartDim;
	color = tex3D(colorLUT, color * scale + bias).rgb;
#endif

	//¶Ô±È¶È
	color.rgb = lerp(GetLuminance(color.rgb), color.rgb, _Saturation);

	//ÑÕÉ«»ìºÏ
	//color.rgb *= _ColorMix.rgb;

	//ÍÊÉ«
	if (IsFading)
	{
		fixed grey = dot(color.rgb, fixed3(0.299, 0.587, 0.114));
		color.rgb = fixed3(grey, grey, grey);
	}
			
	//ÕÚÕÖ
	if (IsZheZhao == 1)
	{
		fixed4 zz = tex2D(Zhezhao, i.uv);
		color = zz * color;
	}

	return half4(color, 1);
}

#endif

#if defined(IMG_0) && defined(IMG_1)  && defined(PARAM_0) && defined(PARAM_1)
half4 frag_dof(v2f i) : SV_Target
{
	half4 ori = tex2D(IMG_0, i.uv);
	half4 blur = tex2D(IMG_1, i.uv);
	float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
	depth = Linear01Depth(depth);
	//half4 final = (depth <= _focalDistance) ? ori : lerp(ori, blur, clamp((depth - _focalDistance) * _farBlurScale, 0, 1));
	//final = (depth > _focalDistance) ? final :  lerp(ori, blur, clamp((_focalDistance - depth) * _nearBlurScale, 0, 1));
	half temp = max(saturate((depth - _focalDistance) * _farBlurScale), saturate((_focalDistance - depth) * _nearBlurScale));
	half4 final = lerp(ori, blur, temp);
	return final;
}
#endif

#if defined(IMG_0)
half4 fragFade(v2f i ) : SV_TARGET
{
	half4 color = tex2D(IMG_0, i.uv.xy);
			
	//color *= _ColorMix;

	if (IsFading)
	{
		half grey = dot(color.rgb, fixed3(0.299, 0.587, 0.114));
		color.rgb = fixed3(grey, grey, grey);
	}
			
	if (IsZheZhao == 1)
	{
		half4 zz = tex2D(Zhezhao, i.uv.xy);
		color = zz * color;
	}
	return color;
}
#endif


//fxaa=====================================================
#if defined(IMG_0) && defined(PARAM_0)
float _ContrastThreshold, _RelativeThreshold;
float _SubpixelBlending;

struct VertexData 
{
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
};

struct Interpolators 
{
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
};

Interpolators VertexProgram(VertexData v) 
{
	Interpolators i;
	i.pos = UnityObjectToClipPos(v.vertex);
	i.uv = v.uv;
	return i;
}

float4 Sample(float2 uv) 
{
	return tex2Dlod(IMG_0, float4(uv, 0, 0));
}

float SampleLuminance(float2 uv) 
{
	return Sample(uv).g;
}

float SampleLuminance(float2 uv, float uOffset, float vOffset)
{
	uv += PARAM_0 * float2(uOffset, vOffset);
	return SampleLuminance(uv);
}

struct LuminanceData 
{
	float m, n, e, s, w;
	float ne, nw, se, sw;
	float highest, lowest, contrast;
};

LuminanceData SampleLuminanceNeighborhood(float2 uv) 
{
	LuminanceData l;
	l.m = SampleLuminance(uv);
	l.n = SampleLuminance(uv, 0, 1);
	l.e = SampleLuminance(uv, 1, 0);
	l.s = SampleLuminance(uv, 0, -1);
	l.w = SampleLuminance(uv, -1, 0);

	l.ne = SampleLuminance(uv, 1, 1);
	l.nw = SampleLuminance(uv, -1, 1);
	l.se = SampleLuminance(uv, 1, -1);
	l.sw = SampleLuminance(uv, -1, -1);

	l.highest = max(max(max(max(l.n, l.e), l.s), l.w), l.m);
	l.lowest = min(min(min(min(l.n, l.e), l.s), l.w), l.m);
	l.contrast = l.highest - l.lowest;
	return l;
}

bool ShouldSkipPixel(LuminanceData l) 
{
	float threshold =
		max(_ContrastThreshold, _RelativeThreshold * l.highest);
	return l.contrast < threshold;
}

float DeterminePixelBlendFactor(LuminanceData l)
{
	float filter = 2 * (l.n + l.e + l.s + l.w);
	filter += l.ne + l.nw + l.se + l.sw;
	filter *= 1.0 / 12;
	filter = abs(filter - l.m);
	filter = saturate(filter / l.contrast);

	float blendFactor = smoothstep(0, 1, filter);
	return blendFactor * blendFactor * _SubpixelBlending;
}

struct EdgeData 
{
	bool isHorizontal;
	float pixelStep;
	float oppositeLuminance, gradient;
};

EdgeData DetermineEdge(LuminanceData l) 
{
	EdgeData e;
	float horizontal =
		abs(l.n + l.s - 2 * l.m) * 2 +
		abs(l.ne + l.se - 2 * l.e) +
		abs(l.nw + l.sw - 2 * l.w);
	float vertical =
		abs(l.e + l.w - 2 * l.m) * 2 +
		abs(l.ne + l.nw - 2 * l.n) +
		abs(l.se + l.sw - 2 * l.s);
	e.isHorizontal = horizontal >= vertical;

	float pLuminance = e.isHorizontal ? l.n : l.e;
	float nLuminance = e.isHorizontal ? l.s : l.w;
	float pGradient = abs(pLuminance - l.m);
	float nGradient = abs(nLuminance - l.m);

	e.pixelStep =
		e.isHorizontal ? PARAM_0.y : PARAM_0.x;

	if (pGradient < nGradient) 
	{
		e.pixelStep = -e.pixelStep;
		e.oppositeLuminance = nLuminance;
		e.gradient = nGradient;
	}
	else 
	{
		e.oppositeLuminance = pLuminance;
		e.gradient = pGradient;
	}

	return e;
}

#define EDGE_STEP_COUNT 4
#define EDGE_STEPS 1, 1.5, 2, 4
#define EDGE_GUESS 12

static const float edgeSteps[EDGE_STEP_COUNT] = { EDGE_STEPS };

float DetermineEdgeBlendFactor(LuminanceData l, EdgeData e, float2 uv) 
{
	float2 uvEdge = uv;
	float2 edgeStep;
	if (e.isHorizontal) 
	{
		uvEdge.y += e.pixelStep * 0.5;
		edgeStep = float2(PARAM_0.x, 0);
	}
	else 
	{
		uvEdge.x += e.pixelStep * 0.5;
		edgeStep = float2(0, PARAM_0.y);
	}

	float edgeLuminance = (l.m + e.oppositeLuminance) * 0.5;
	float gradientThreshold = e.gradient * 0.25;

	float2 puv = uvEdge + edgeStep * edgeSteps[0];
	float pLuminanceDelta = SampleLuminance(puv) - edgeLuminance;
	bool pAtEnd = abs(pLuminanceDelta) >= gradientThreshold;

	UNITY_UNROLL
		for (int i = 1; i < EDGE_STEP_COUNT && !pAtEnd; i++) 
		{
			puv += edgeStep * edgeSteps[i];
			pLuminanceDelta = SampleLuminance(puv) - edgeLuminance;
			pAtEnd = abs(pLuminanceDelta) >= gradientThreshold;
		}
	if (!pAtEnd) 
	{
		puv += edgeStep * EDGE_GUESS;
	}

	float2 nuv = uvEdge - edgeStep * edgeSteps[0];
	float nLuminanceDelta = SampleLuminance(nuv) - edgeLuminance;
	bool nAtEnd = abs(nLuminanceDelta) >= gradientThreshold;

	UNITY_UNROLL
		for (int i = 1; i < EDGE_STEP_COUNT && !nAtEnd; i++) 
		{
			nuv -= edgeStep * edgeSteps[i];
			nLuminanceDelta = SampleLuminance(nuv) - edgeLuminance;
			nAtEnd = abs(nLuminanceDelta) >= gradientThreshold;
		}
	if (!nAtEnd) 
	{
		nuv -= edgeStep * EDGE_GUESS;
	}

	float pDistance, nDistance;
	if (e.isHorizontal) 
	{
		pDistance = puv.x - uv.x;
		nDistance = uv.x - nuv.x;
	}
	else 
	{
		pDistance = puv.y - uv.y;
		nDistance = uv.y - nuv.y;
	}

	float shortestDistance;
	bool deltaSign;
	if (pDistance <= nDistance) 
	{
		shortestDistance = pDistance;
		deltaSign = pLuminanceDelta >= 0;
	}
	else
	{
		shortestDistance = nDistance;
		deltaSign = nLuminanceDelta >= 0;
	}

	if (deltaSign == (l.m - edgeLuminance >= 0))
	{
		return 0;
	}
	return 0.5 - shortestDistance / (pDistance + nDistance);
}

float4 ApplyFXAA(float2 uv) 
{
	LuminanceData l = SampleLuminanceNeighborhood(uv);
	if (ShouldSkipPixel(l)) 
	{
		return Sample(uv);
	}

	float pixelBlend = DeterminePixelBlendFactor(l);
	EdgeData e = DetermineEdge(l);
	float edgeBlend = DetermineEdgeBlendFactor(l, e, uv);
	float finalBlend = max(pixelBlend, edgeBlend);

	if (e.isHorizontal) 
	{
		uv.y += e.pixelStep * finalBlend;
	}
	else 
	{
		uv.x += e.pixelStep * finalBlend;
	}
	return float4(Sample(uv).rgb, l.m);
}
#endif



#if defined(IMG_0) && defined(PARAM_0)
half4 fragRadialBlur (v2f i) : SV_target
{
	// fixed2 dir = 0.5-i.uv;
	fixed2 dir = PARAM_0.zw - i.uv;
	fixed dist = length(dir);
	dir /= dist;
	dir *= PARAM_0.x;
	fixed4 sum = tex2D(IMG_0, i.uv - dir * 0.01);
	sum += tex2D(IMG_0, i.uv - dir*0.02);
	sum += tex2D(IMG_0, i.uv - dir*0.03);
	sum += tex2D(IMG_0, i.uv - dir*0.05);
	sum += tex2D(IMG_0, i.uv - dir*0.08);
	sum += tex2D(IMG_0, i.uv + dir*0.01);
	sum += tex2D(IMG_0, i.uv + dir*0.02);
	sum += tex2D(IMG_0, i.uv + dir*0.03);
	sum += tex2D(IMG_0, i.uv + dir*0.05);
	sum += tex2D(IMG_0, i.uv + dir*0.08);
	sum *= 0.1;
	return sum;
}
#endif

#if defined(IMG_0) && defined(IMG_1) && defined(PARAM_0)
half4 fragCombine (v2f i) : SV_target
{
	// fixed2 dir = 0.5-i.texcoord;
	//fixed dist = length(0.5-i.uv);
	//dist *= dist;
	fixed2 dir = PARAM_0.zw - i.uv;
	fixed dist = length(dir);
	dist *= dist;
	fixed4  col = tex2D(IMG_0, i.uv);
	fixed4  blur = tex2D(IMG_1, i.uv);
	col=lerp(col, blur,saturate(PARAM_0.y * dist));
	return col;
}
#endif


half2 _ScaledTextureSize;
half2 _ShatterPos;
half _ShatterOffsetScale;

#if defined(IMG_0) && defined(IMG_1) && defined(PARAM_0)
    half4 fragShatter (v2f i) : SV_target
    {
        half2 bumpUV = (i.uv * PARAM_0.zw - _ShatterPos) / _ScaledTextureSize + 0.5; 
        half2 bumpOffset = UnpackNormal(tex2D(IMG_1, bumpUV)).rg;
        half2 offsetUV = bumpOffset * _ShatterOffsetScale + i.uv;   
        fixed4 col = tex2D(IMG_0, offsetUV);  
        return col;
    }
#endif

//======================================================


