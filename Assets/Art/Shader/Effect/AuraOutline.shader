
Shader "YanJia/Effect/AuraOutline" 
{
	Properties
    {
		_Color("Aura Color", Color) = (0,0,1,1)
		_ColorR("Rim Color", Color) = (0,1,1,1)
		_Outline("Outline width", Range(.002, 0.8)) = 0.3
		_OutlineZ("Outline Z", Range(-1, 0)) = -0.1
		_NoiseTex("Noise Texture", 2D) = "white" { }
        _NoiseCutOff("Noise Cutoff", Range(0,1)) = 0
		_Scale("Noise Scale", Range(0.0, 5)) = 1
		_SpeedX("Speed X", Range(-10, 10)) = 0
		_SpeedY("Speed Y", Range(-10, 10)) = 3.0
		_Opacity("Noise Opacity", Range(0.01, 10.0)) = 10
		_Brightness("Brightness", Range(0.5, 5)) = 2
		_Edge("Rim Edge", Range(0.0, 1)) = 0.1
		_RimPower("Rim Power", Range(0.01, 1.0)) = 1
	}


    SubShader
    {
        Tags{ "RenderType" = "Transparent" }
        Pass
        {
            Tags { "LightMode"="ForwardBase"}
            Cull Front
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
            half _Scale, _Opacity, _Edge;
            fixed4 _Color, _ColorR;
            half _Brightness, _SpeedX, _SpeedY;
            half _Outline;
            half _OutlineZ;
            half _RimPower;
            fixed _NoiseCutOff;

            struct a2v 
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f 
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 worldPos : TEXCOORD3; 
            };



            v2f vert(a2v v) 
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                float4 pos = mul( UNITY_MATRIX_MV, v.vertex);
                float3 normal = mul( (float3x3)UNITY_MATRIX_IT_MV, v.normal);
                normal.z = -0.5;
                pos = pos + float4(normalize(normal),0) * _Outline;
                o.pos = mul(UNITY_MATRIX_P, pos);
                o.normalDir = normalize(UnityObjectToWorldNormal(v.normal));
                o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }


            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv =  float2(i.worldPos.x * _NoiseTex_ST.x * _Scale - (_Time.x * _SpeedX), i.worldPos.y * _NoiseTex_ST.y * _Scale - (_Time.x * _SpeedY));
                fixed4 noise = tex2D(_NoiseTex, uv);
                fixed rim = pow(saturate(dot(i.viewDir,- i.normalDir)), _RimPower);
                rim -= noise.r + _NoiseCutOff;
                fixed texturedRim = saturate(rim * _Opacity);
                fixed extraRim = (saturate((_Edge + rim) * _Opacity) - texturedRim) * _Brightness;
                fixed4 col = (_Color * texturedRim) + (_ColorR * extraRim);
                return col;
            }
            ENDCG
        }
    }
}