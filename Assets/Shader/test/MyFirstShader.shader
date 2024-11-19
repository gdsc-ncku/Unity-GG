 Shader "Unlit/MyFirstShader"
{
    SubShader
    {
        Pass
        {
            CGPROGRAM //Begin CG Coding

            #pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram

            //UnityCG.cginc is one of the shader include files that are bundled with Unity. It includes a few other essential files, and contains some generic functionality.
            #include "UnityCG.cginc"

            // 頂點著色器
            float4 MyVertexProgram (float4 position : POSITION) : SV_POSITION {
				return UnityObjectToClipPos(position);
			}

            // 片段著色器
            float4 MyFragmentProgram(float4 position : SV_POSITION) : SV_TARGET {
                return 0;
            }

			ENDCG //Ending CG Coding
        }
    }
}