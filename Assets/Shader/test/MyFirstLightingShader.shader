Shader "Custom/MyFirstLightingShader"
{
    Properties
    {
		_MainTex ("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
        Pass
		{
			CGPROGRAM

			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;

			struct VertexData 
			{
				float4 position : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};
			
			struct Interpolators 
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
			};

			Interpolators MyVertexProgram (VertexData v) {
				Interpolators i;
				i.position = UnityObjectToClipPos(v.position);
				i.normal = UnityObjectToWorldNormal(v.normal);
				i.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return i;
			}

			float4 MyFragmentProgram (Interpolators i) : SV_TARGET 
			{
				i.normal = normalize(i.normal);
				return float4(i.normal * 0.5 + 0.5, 1);
			}

			ENDCG
		}
    }
}
