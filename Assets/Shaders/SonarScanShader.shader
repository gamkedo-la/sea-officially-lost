﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SonarScanShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ScanDistance("Scan Distance", float) = 0
		_ScanWidth("Scan Width", float) = 10
		_LeadSharp("Leading Edge Sharpness", float) = 10
		_LeadColor("Leading Edge Color", Color) = (1, 1, 1, 0)
		_MidColor("Mid Color", Color) = (1, 1, 1, 0)
		_TrailColor("Trail Color", Color) = (1, 1, 1, 0)
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct vIn
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 ray: TEXCOORD1;
			};

			float4 _MainTex_TexelSize;

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float2 uv_depth : TEXCOORD1;
				float4 interpolatedRay : TEXCOORD2;
			};

			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;
			float4 _WorldSpaceScannerPos;
			float _ScanDistance;
			float _ScanWidth;
			float _LeadSharp;
			float4 _LeadColor;
			float4 _MidColor;
			float4 _TrailColor;

//			float4 _MainTex_ST;
			
			float4 horzBars(float2 y)
			{
				return 1 - saturate(round(abs(frac(y.y * _ScreenParams.y/8) * 2)));
			}

			v2f vert(vIn v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv.xy;
				o.uv_depth = v.uv.xy;

				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					o.uv.y = 1 - o.uv.y;
				#endif				

				o.interpolatedRay = v.ray;

				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				half4 col = tex2D(_MainTex, i.uv);

				float rawDepth = DecodeFloatRG(tex2D(_CameraDepthTexture, i.uv));
				float linearDepth = Linear01Depth(rawDepth);
				float4 wsDir = linearDepth * i.interpolatedRay;
				float3 wsPos = _WorldSpaceCameraPos + wsDir;
				half4 scannerCol = half4(0, 0, 0, 0);
				//_CameraDepthTexture = EncodeFloatRG(tex2D(_CameraDepthTexture, i.uv));

				float dist = distance(wsPos, _WorldSpaceScannerPos);

				float diff = 1 - (_ScanDistance - dist) / _ScanWidth;

				int lead = saturate(diff * 10 - 8.8); //10 - 8 is one unit of width, 10 - 9 is zero units

				int scanMask = lerp(0, 1, !((int)(1 - diff) || (int)diff)) && linearDepth < 1;
				
				half4 edge = lerp(_MidColor, _LeadColor, lead);
				scannerCol = lerp(_TrailColor, edge, diff);// + horzBars(i.uv) * half4(1, 1, 1, 1);

				float distFade = saturate(1 - dist*0.005);

				return col + scannerCol * scanMask * diff * diff * distFade * distFade;
			}
			ENDCG
		}
	}
}
