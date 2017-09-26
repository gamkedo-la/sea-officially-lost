Shader "Custom/ImageEffectRipple"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_CircleProperties ("CircleProperties", Vector) = (0, 0, 0, 0)
		_EffectPower ("Power", float) = 0.1
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

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float4 _CircleProperties;
			float _EffectPower;

			fixed4 frag (v2f i) : SV_Target
			{
			    float2 pos = _CircleProperties.xy;
				float radius = _CircleProperties.z;
				float width = _CircleProperties.w;
				float2 uvOffset;
				float dist = distance(i.uv, pos);

				uvOffset = normalize(i.uv - pos) * _EffectPower; // Get direction and power of effect
				uvOffset *= (dist < radius); // Eval to zero outside the radius
				uvOffset *= sin((radius - dist)*width); // Make a wave pattern
				uvOffset *= (1.41421356-dist); // Fade towards the edges and corners (sqrt(2) is screen corner)
				i.uv = i.uv - uvOffset;

				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
