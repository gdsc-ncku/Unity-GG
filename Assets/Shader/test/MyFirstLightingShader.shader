Shader "Custom/MyFirstLightingShader"
{
    Properties
    {
		_Tint ("Tint", Color) = (1, 1, 1, 1)
		_MainTex ("Albedo", 2D) = "white" {}
		[Gamma] _Metallic ("Metallic", Range(0, 1)) = 0
		_Smoothness ("Smoothness", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Pass
		{
			Tags {
				"LightMode" = "ForwardBase"
			}

			CGPROGRAM

			#pragma target 3.0

			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

			//#include "UnityCG.cginc"
			#include "UnityPBSLighting.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST, _Tint;
			float _Smoothness, _Metallic;

			struct VertexData 
			{
				float4 position : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};
			
			struct Interpolators {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
			};

			Interpolators MyVertexProgram (VertexData v) {
				Interpolators i;
				i.position = UnityObjectToClipPos(v.position);
				i.worldPos = mul(unity_ObjectToWorld, v.position);
				i.normal = UnityObjectToWorldNormal(v.normal);
				i.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return i;
			}

			float4 MyFragmentProgram (Interpolators i) : SV_TARGET {
				i.normal = normalize(i.normal);
				float3 lightDir = _WorldSpaceLightPos0.xyz;
				float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

				float3 lightColor = _LightColor0.rgb;
				float3 albedo = tex2D(_MainTex, i.uv).rgb * _Tint.rgb;
				float3 specularTint;
				float oneMinusReflectivity;
				albedo = DiffuseAndSpecularFromMetallic(
					albedo, _Metallic, specularTint, oneMinusReflectivity
				);
				/*
					//���Ϯg�p��]Diffuse�^
					//�o�� albedo �O���誺��¦�C��A���u�P�k�V�q���I�n�M�w���Ϯg�G�סC
					//�����C��P�����C��ۭ��A�����F������C�������l���C
					float3 diffuse =
						albedo * lightColor * DotClamped(lightDir, i.normal);
					float3 halfVector = normalize(lightDir + viewDir);

					//_SpecularTint.rgb �O���誺�譱�Ϯg�C��A�Ψӽվ㰪�����C��C
					//���ƫ� _Smoothness �M�w�F�������W�Q�{�סC
					//DotClamped(halfVector, i.normal) �ϬM�F���u�P������V���@�P�ʡC
					float3 specular = specularTint * lightColor * pow(
						DotClamped(halfVector, i.normal),
						_Smoothness * 100
					);
				*/

				UnityLight light;
				light.color = lightColor;
				light.dir = lightDir;
				light.ndotl = DotClamped(i.normal, lightDir);

				UnityIndirect indirectLight;
				indirectLight.diffuse = 0;
				indirectLight.specular = 0;

				return UNITY_BRDF_PBS(
					albedo, specularTint,
					oneMinusReflectivity, _Smoothness,
					i.normal, viewDir,
					light, indirectLight
				);
			}

			ENDCG
		}
    }
}
