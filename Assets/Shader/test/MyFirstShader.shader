Shader "Unlit/MyFirstShader"
{
    Properties
    {
        _Tint ("Tint", Color) = (1, 1, 1, 1)
		_MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM //Begin CG Coding

            #pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

            //UnityCG.cginc is one of the shader include files that are bundled with Unity. It includes a few other essential files, and contains some generic functionality.
            #include "UnityCG.cginc"

            float4 _Tint;
			sampler2D _MainTex;
			float4 _MainTex_ST;

            struct Interpolators {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

            struct VertexData {
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
			};

			Interpolators MyVertexProgram (VertexData v) {
				Interpolators i;
				i.position = UnityObjectToClipPos(v.position);
				i.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return i;
			}

			float4 MyFragmentProgram (Interpolators i) : SV_TARGET {
				return tex2D(_MainTex, i.uv);
			}

			ENDCG //Ending CG Coding
        }
    }
}