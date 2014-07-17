﻿Shader "Custom/BloomFlare" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BlurTex ("Base (RGB)", 2D) = "white" {}
		_BloomColor ("BloomCol", Color) = (1.0, 1.0, 1.0, 1.0)
		_Amount ("Bloom Amount", Range(0,1)) = 0.5
		_Threshold ("Threshold", Range(0,1)) = 0.5
		_RadiusH("RadiusH", Range(0,1)) = 0.5
		_RadiusW("RadiusW", Range(0,1)) = 0.5
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"
	
	sampler2D _MainTex;
	sampler2D _BlurTex;
	float4 _MainTex_TexelSize;
	uniform float _Threshold;
	uniform float _Amount;
	uniform float _RadiusH;
	uniform float _RadiusW;
	uniform float4 _BloomColor;
	
	struct v2f {
		float4 pos : POSITION;
		float2 uv : TEXCOORD0;
	};
	
	v2f vert (appdata_img v)
	{
		v2f o;
		
		o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
		o.uv.xy = v.texcoord.xy;
		return o;
	}
	
	float BungieBloomCurve(float x)
	{
		x = pow( max(0.0, x * 3.1429 * 2.0f - 1.0f), 5.0f);
		return x;
	}
	
	fixed4 Downsample(v2f i) : COLOR
	{
		fixed2 InputSize = fixed2(1/_ScreenParams.x, _ScreenParams.y);
		
		fixed4 vSample = tex2D(_BlurTex, i.uv);
			
		vSample += tex2D(_BlurTex, i.uv+InputSize * fixed2(1.0f, 1.0f));
		vSample += tex2D(_BlurTex, i.uv+InputSize * fixed2(-1.0f, 1.0f));
		vSample += tex2D(_BlurTex, i.uv+InputSize * fixed2(1.0f, -1.0f));
		vSample += tex2D(_BlurTex, i.uv+InputSize * fixed2(-1.0f, -1.0f));
			
		vSample /= 4.0f;
			
		return vSample;
		
	}
	
	fixed4 vertical (v2f i) : COLOR
	{
		fixed4 sum = fixed4(0.0);
		
   		// blur in y (vertical)
   		// take nine samples, with the distance _Radius between them
   		sum += tex2D(_BlurTex, fixed2(i.uv.x, i.uv.y - 4.0 * _RadiusH)) * 0.05;
   		sum += tex2D(_BlurTex, fixed2(i.uv.x, i.uv.y - 3.0 * _RadiusH)) * 0.09;
   		sum += tex2D(_BlurTex, fixed2(i.uv.x, i.uv.y - 2.0 * _RadiusH)) * 0.12;
   		sum += tex2D(_BlurTex, fixed2(i.uv.x, i.uv.y - _RadiusH)) * 0.15;
   		sum += tex2D(_BlurTex, fixed2(i.uv.x, i.uv.y)) * 0.16;
   		sum += tex2D(_BlurTex, fixed2(i.uv.x, i.uv.y + _RadiusH)) * 0.15;
   		sum += tex2D(_BlurTex, fixed2(i.uv.x, i.uv.y + 2.0 * _RadiusH)) * 0.12;
   		sum += tex2D(_BlurTex, fixed2(i.uv.x, i.uv.y + 3.0 * _RadiusH)) * 0.09;
   		sum += tex2D(_BlurTex, fixed2(i.uv.x, i.uv.y + 4.0 * _RadiusH)) * 0.05;
   		
   		return sum;
   	}
   	
   	fixed4 horizontal (v2f i) : COLOR
	{
		fixed4 sum = fixed4(0.0);
		
   		// blur in x (horizontal)
   		// take nine samples, with the distance _Radius between them
   		sum += tex2D(_BlurTex, fixed2(i.uv.x - 4.0 * _RadiusW, i.uv.y)) * 0.05;
   		sum += tex2D(_BlurTex, fixed2(i.uv.x - 3.0 * _RadiusW, i.uv.y)) * 0.09;
   		sum += tex2D(_BlurTex, fixed2(i.uv.x - 2.0 * _RadiusW, i.uv.y)) * 0.12;
   		sum += tex2D(_BlurTex, fixed2(i.uv.x - _RadiusW, i.uv.y)) * 0.15;
   		sum += tex2D(_BlurTex, fixed2(i.uv.x, i.uv.y)) * 0.16;
   		sum += tex2D(_BlurTex, fixed2(i.uv.x + _RadiusW, i.uv.y)) * 0.15;
   		sum += tex2D(_BlurTex, fixed2(i.uv.x + 2.0 * _RadiusW, i.uv.y)) * 0.12;
   		sum += tex2D(_BlurTex, fixed2(i.uv.x + 3.0 * _RadiusW, i.uv.y)) * 0.09;
   		sum += tex2D(_BlurTex, fixed2(i.uv.x + 4.0 * _RadiusW, i.uv.y)) * 0.05;
   		
   		return sum;
   	}
   	
	fixed4 frag (v2f i) : COLOR
	{
		fixed4 col = tex2D(_BlurTex, i.uv);
		fixed4 main = tex2D(_MainTex, i.uv);
		
		col = max(0.0, (col - _Threshold) / (1.0f  - _Threshold)); //Get the bright areas that is brighter than Threshold.
		
		//float fIntensity = max(dot(col.rgb, float3(0.212671, 0.71516, 0.072169)), 0.0001f);
		//_Amount = BungieBloomCurve(fIntensity);
		
		col = saturate(col * _Amount);
		col = (col) * _BloomColor;
		return col + main;
	}
	ENDCG
	
	SubShader {
	Tags { "Queue"="Overlay" "RenderType"="Overlay"}
		Cull Off ZWrite Off Fog { Mode off } Lighting off
		
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment vertical
			#pragma target 3.0
			ENDCG
		}
		
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment horizontal
			#pragma target 3.0
			ENDCG
		}
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			ENDCG
		}
	
	}
}
