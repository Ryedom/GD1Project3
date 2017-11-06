// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Skybox/Starry Sky"
{
    Properties
    {
        _SkyColor1 ("Sky Color 1", Color) = (0, 0, 0, 0)
        _SkyColor2 ("Sky Color 2", Color) = (0.5, 0.5, 0.5, 0)
        _SkyColor2 ("Sky Color 2", Color) = (1, 1, 1, 0)
        _StarColor ("Star Color", Color) = (1, 1, 1, 0)
        _twinkleOffset ("Twinkle Offset", Float) = 0
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    struct appdata
    {
        float4 position : POSITION;
        float3 texcoord : TEXCOORD0;
    };
    
    struct v2f
    {
        float4 position : SV_POSITION;
        float3 texcoord : TEXCOORD0;
    };
    
    half4 _SkyColor1;
    half4 _SkyColor2;
    half4 _SkyColor3;
    half4 _StarColor;
    float _twinkleOffset;

    #define PI 3.141592653589
    
    v2f vert (appdata v)
    {
        v2f o;
        o.position = UnityObjectToClipPos (v.position);
        o.texcoord = v.texcoord;
        return o;
    }
    
    half4 frag (v2f i) : COLOR
    {
        float3 normCoord = normalize(i.texcoord);
        float upAngle = dot(float3(0.0f,1.0f,0.0f),normCoord);
        float rightAngle = dot(float3(1.0f,0.0f,0.0f),normCoord);
        float forwardAngle = dot(float3(0.25f,0.50f,0.75f),normCoord);
        float star = 0.5f * sin(upAngle * 50.0f) * cos(rightAngle * 75.0f) * -sin(forwardAngle * 50.0f);
        #if SHADER_API_MOBILE
        star = round(star + 0.0175f);
        #else
        star = clamp(pow(star + 0.52f,200.0f),0.0f,1.0f);
        float twinkleAngle = dot(float3(0.75f,0.25f,0.50f),normCoord);
        star *= sin(twinkleAngle * 45.0f + _twinkleOffset) + 1.0f;
        #endif
        star *= clamp(normCoord.x * 3.0f + clamp(normCoord.y,0.0f,1.0f) * 1000.0f,0.0f,1.0f);
        return (lerp(lerp(_SkyColor1,_SkyColor3,normCoord.x + 0.5f),_SkyColor2,normCoord.y + 0.95f) + (_StarColor * star));
        //half4 outputSkyColor = (_SkyColor1 * (1.0f * normCoord.x) + _SkyColor3 * (normCoord.x)) * (normCoord.y) + _SkyColor2 * (1.0f - normCoord.y);
        //return (outputSkyColor + (_StarColor * star));
    }

    ENDCG

    SubShader
    {
        Tags { "RenderType"="Background" "Queue"="Background" }
        Pass
        {
            ZWrite Off
            Cull Off
            Fog { Mode Off }
            CGPROGRAM
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    } 
}