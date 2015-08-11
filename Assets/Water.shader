Shader "HeatDistort1" {

   Properties {
      _BumpAmt  ("Distortion", range (0,128)) = 10
      _MainTex ("Tint Color (RGB)", 2D) = "white" {}
      _BumpMap ("Normalmap", 2D) = "bump" {}
      
      _WaveAmplitude ("Wave Amplitude", Float) = 1
      _WaveFrequency ("Wave Frequency", Float) = 0.5
   }

   SubShader {
   
      Blend SrcAlpha OneMinusSrcAlpha 
      Tags { "Queue"="Transparent" "RenderType"="Transparent" }
      
         Pass {
      
      CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members t)
#pragma exclude_renderers d3d11 xbox360
// Upgrade NOTE: excluded shader from Xbox360; has structs without semantics (struct v2f members lightDir,normal)
#pragma exclude_renderers xbox360
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

struct appdata_t {
   float4 vertex : POSITION;
   float2 texcoord: TEXCOORD0;
};

struct v2f {
   float4 vertex : POSITION;
   float4 uvgrab : TEXCOORD0;
   float2 uvbump : TEXCOORD1;
   float2 uvmain : TEXCOORD2;
   float t;
};


sampler2D _GrabTexture : register(s0);
float4 _GrabTexture_TexelSize;
sampler2D _BumpMap : register(s1);
sampler2D _MainTex : register(s2);

float _WaveAmplitude;
float _WaveFrequency;

uniform float _BumpAmt;


v2f vert (appdata_t v)
{
   v2f o;
   o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
   o.t = _Time;
   #if UNITY_UV_STARTS_AT_TOP
   float scale = -1.0;
   #else
   float scale = 1.0;
   #endif
   o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
   o.uvgrab.zw = o.vertex.zw;
   o.uvbump = MultiplyUV( UNITY_MATRIX_TEXTURE1, v.texcoord );
   o.uvmain = MultiplyUV( UNITY_MATRIX_TEXTURE2, v.texcoord );
   return o;
}

half4 frag( v2f i ) : COLOR
{
   half2 bump = UnpackNormal(tex2D( _BumpMap, i.uvbump )).rg;

   float2 offset = bump * _BumpAmt * _GrabTexture_TexelSize.xy;
   i.uvgrab.xy = offset * i.uvgrab.z + i.uvgrab.xy  + _WaveAmplitude * float2(sin(_WaveFrequency * i.t), cos(_WaveFrequency * i.t));
   
   half4 col = tex2D( _GrabTexture, i.uvgrab.xy );
   half4 tint = tex2D( _MainTex, i.uvmain );
   return col * tint;
}

ENDCG
         }
      }

   Fallback "Diffuse"
}