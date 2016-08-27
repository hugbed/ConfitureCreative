Shader "Unlit/Fripouillé"
{
	Properties
	{
		_MainTex ("Albedo", 2D) = "white" {}
		_DisplacementMap ("Displacement map", 2D) = "white" {}
		_DisplacementFactor ("Displacement factor", Range(0.0, 100.0)) = 0
		_Wet ("Wet", Range (0.0, 1.0)) = 0.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Cull Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"
			
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _DisplacementMap;
			float _DisplacementFactor;
			float4 _MainTex_ST;
			float _Wet;
			
			v2f vert (appdata_base v)
			{
				v2f o;
				float4 tex = tex2Dlod(_DisplacementMap, float4(v.texcoord.xy, 0, 0));
				v.vertex += (tex.b - 0.5) / 10 * _DisplacementFactor * float4(v.normal.xyz, 0);

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				
				o.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = lerp(tex2D(_MainTex, i.uv), float4(0.2,0.2,0.7,1), clamp(_Wet, 0, 1) * 0.8);
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
