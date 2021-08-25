
half _FogStart;
half _FogEnd;
half _FogMult;
half _FogExp;
half _TopFogHeight;
half _MiddleFogHeight;
half3 _TopFogColor;
half3 _MiddleFogColor;
half3 _DepthFogColor;
half _TopFogMult;
half _MiddleFogMult;
half _MiddleFogRange;
half _TopFogRange;

half4 GetCustomFog(half3 worldPos, half4 col)
{
	//col.rgb = half3(0,0,0);
	half3 offset = worldPos - _WorldSpaceCameraPos;
	half dist = length(offset);
	half linearFog = saturate((_FogEnd - abs(dist)) / (_FogEnd - _FogStart)); //线性雾
	linearFog = pow(linearFog, _FogExp);

	half height = worldPos.y;
	half topFog =  saturate((_TopFogHeight - height) * _TopFogRange);//顶部雾
	half MiddleFog = saturate(abs( _MiddleFogHeight - height) - _MiddleFogRange);//中部雾

	_DepthFogColor = lerp(col.rgb, _DepthFogColor, _FogMult);
	half3 finalColor = lerp(_DepthFogColor, col.rgb, linearFog);//混合线性雾
	_TopFogColor = lerp(finalColor, _TopFogColor, _TopFogMult);//混合顶部雾强度
	finalColor = lerp(_TopFogColor, finalColor,topFog *  linearFog);//混合顶部雾
	_MiddleFogColor = lerp(finalColor, _MiddleFogColor, _MiddleFogMult);//混合中部雾强度
	finalColor = lerp(_MiddleFogColor, finalColor, MiddleFog * linearFog);//混合中部雾
	return half4(finalColor, col.a);
}


//=====================================  OBJECT_PASS_FORWARD_BASE
#ifdef OBJECT_PASS_FORWARD_BASE

#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "AutoLight.cginc"

fixed4 _Color;
sampler2D _MainTex;
float4 _MainTex_ST;
fixed3 _EmissionColor;
half _Emmission;
half _DiffusePower;
fixed3 _SpecularColor;
half _SpecularPower;
half _Gloss;
fixed3 _FresnelColor;
half _FresnelPower;
half _FresnelScale;
fixed3 _FakeLightColor;
half4 _FakeLightDir;
sampler2D _MetalTex;
samplerCUBE _EnvTex;
float _Spread;
half _BloomPower;
			
struct a2v
{
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
	float3 normal : NORMAL;
#ifdef LIGHTMAP_ON
	float2 uvLM : TEXCOORD1;
#endif
};
			
struct v2f
{
	float4 pos : SV_POSITION;
	float4 uv : TEXCOORD0;
	float3 worldNormal : TEXCOORD1;
	float3 worldPos : TEXCOORD2;
	float3 reflect : TEXCOORD3;
#ifndef ENABLE_FAKE_LIGHTING
	UNITY_SHADOW_COORDS(4)
#endif
#ifdef ENABLE_FOG
	UNITY_FOG_COORDS(5)
#endif
#ifdef LIGHTMAP_ON
	float2 uvLM : TEXCOORD6;
#endif
};
			
v2f vert(a2v v) 
{
	v2f o;
	UNITY_INITIALIZE_OUTPUT(v2f, o);

	o.pos = UnityObjectToClipPos(v.vertex);
	o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
	//o.uv.z = dot(normalize(UNITY_MATRIX_IT_MV[0].xyz), normalize(v.normal));
	//o.uv.w = dot(normalize(UNITY_MATRIX_IT_MV[1].xyz), normalize(v.normal));

	o.uv.zw = o.uv.zw * 0.5 + 0.5;
	o.worldNormal = UnityObjectToWorldNormal(v.normal);
	o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
	fixed3 viewDir = UnityWorldSpaceViewDir(o.worldPos);
	o.reflect = reflect(-viewDir,o.worldNormal);
	//o.reflect.x = -o.reflect.x;
#ifndef ENABLE_FAKE_LIGHTING
	TRANSFER_SHADOW(o);
#endif

#ifdef ENABLE_FOG
	UNITY_TRANSFER_FOG(o, o.pos);
#endif
#ifdef LIGHTMAP_ON
	o.uvLM = v.uvLM.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif
	return o;
}
			
fixed4 frag(v2f i) : SV_Target
{
				
	fixed4 albedo = tex2D(_MainTex, i.uv.xy);
	fixed4 col = albedo * _Color; //不受光时的颜色
	fixed3 worldNormal = normalize(i.worldNormal);
	fixed metallic = 0;

	#if ENABLE_MATCAP
		metallic = tex2D(_MetalTex, i.uv.xy).r;
	#endif

#if LIGHTMAP_ON
	col.rgb = albedo.rgb * DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uvLM.xy));
#endif
#if ENABLE_DIFFUSE
	//使用真实光照数据，或者假光照光照数据
	#if ENABLE_FAKE_LIGHTING
			fixed3 worldLightDir = normalize(_FakeLightDir).xyz;
			fixed3 LightColor =_FakeLightColor;
	#else
			fixed3 worldLightDir =  normalize(UnityWorldSpaceLightDir(i.worldPos));
			fixed3 LightColor = _LightColor0.rgb;
	#endif
	
	//计算漫反射
	albedo *= _Color;
	fixed3 diffuse = LightColor * albedo.rgb * max(0, dot(worldNormal, worldLightDir));
	#if LIGHTMAP_ON
		col.rgb += diffuse * _DiffusePower * (1 - metallic);
	#else
		col.rgb = diffuse * _DiffusePower * (1 - metallic); //只包含diffuse
	#endif

	//计算高光反射
	fixed3 viewDir;
	#if ENABLE_SPECULAR
	viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
	fixed3 halfDir = normalize(worldLightDir + viewDir);
	fixed3 specular = LightColor * _SpecularColor * pow(max(0, dot(worldNormal, halfDir)), _Gloss);
	col.rgb += specular * _SpecularPower * (1 - metallic);//包含diffuse和specular
	#endif

	//计算菲尼尔反射
	#if ENABLE_FRESNEL
	viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
	fixed p = 1 - dot(viewDir, worldNormal);
	half fresnel = _FresnelScale + (1 - _FresnelScale) *p *p *p *p *p;
	col.rgb = lerp(col.rgb, _FresnelColor, saturate(fresnel * _FresnelPower));
	#endif

	#ifndef LIGHTMAP_ON
		//计算环境光光照衰减
		fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo.rgb;
		#ifndef ENABLE_FAKE_LIGHTING
			UNITY_LIGHT_ATTENUATION(atten, i, i.worldPos);
			col.rgb = col.rgb * atten + ambient;
		#else
			col.rgb = col.rgb + ambient;
		#endif
	#endif
#endif


	//计算金属反光
	#if ENABLE_MATCAP
	col.xyz += texCUBE(_EnvTex,i.reflect) * _Spread * metallic;
	#endif

	//添加自发光
	col.rgb += albedo.rgb * _Emmission * _EmissionColor * albedo.a;

	col.a = albedo.a * _BloomPower;

#ifdef ENABLE_FOG
	UNITY_APPLY_FOG(i.fogCoord, col);
#elif ENABLE_CUSTOM_FOG
	col = GetCustomFog(i.worldPos, col);
#endif
	return col;
}

#endif /* OBJECT_PASS_FORWARD_BASE */
//END=====================================  OBJECT_PASS_FORWARD_BASE

//=====================================  OBJECT_PASS_SHADOWCASTER
#ifdef OBJECT_PASS_SHADOWCASTER
#include "UnityCG.cginc"
struct v2f
{
    V2F_SHADOW_CASTER;
};

v2f vert(appdata_base v)
{
    v2f o;
	UNITY_INITIALIZE_OUTPUT(v2f, o);
    TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    SHADOW_CASTER_FRAGMENT(i)
}
#endif /* OBJECT_PASS_SHADOWCASTER */
//END=====================================  OBJECT_PASS_SHADOWCASTER


//=====================================  OBJECT_PASS_META
#ifdef OBJECT_PASS_META
#include "Lighting.cginc"
#include "UnityMetaPass.cginc"

fixed4 _Color;
sampler2D _MainTex;
float4 _MainTex_ST;

struct v2f
{
    float4 pos:SV_POSITION;
    float2 uv:TEXCOORD1;
    float3 worldPos:TEXCOORD0;
};

v2f vert_meta(appdata_full v)
{
    v2f o;
    UNITY_INITIALIZE_OUTPUT(v2f,o);

    o.pos = UnityMetaVertexPosition(v.vertex,v.texcoord1.xy,v.texcoord2.xy,unity_LightmapST,unity_DynamicLightmapST);
    o.uv = v.texcoord.xy;
    return o;
}

fixed4 frag_meta(v2f IN):SV_Target
{
	fixed4 albedo =  tex2D(_MainTex,IN.uv) * _Color;
    UnityMetaInput metaIN;
    UNITY_INITIALIZE_OUTPUT(UnityMetaInput,metaIN);
    metaIN.Albedo = albedo.rgb;
    metaIN.Emission = 0;
	return fixed4(1 , 1, 0, 1) ;
}

#endif /* OBJECT_PASS_META */
//END=====================================  OBJECT_PASS_META
