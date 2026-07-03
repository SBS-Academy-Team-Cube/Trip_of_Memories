Shader "TripOfMemories/Character/XRaySilhouette"
{
    Properties
    {
        [HDR] _XRayColor ("X-Ray Color", Color) = (0.12, 0.85, 1.0, 0.38)
        [HDR] _RimColor ("Rim Color", Color) = (0.65, 1.0, 1.0, 0.85)
        _RimPower ("Rim Power", Range(0.1, 8.0)) = 2.4
        _RimStrength ("Rim Strength", Range(0.0, 3.0)) = 1.25
        _Intensity ("Intensity", Range(0.0, 5.0)) = 1.0
        _StencilRef ("Stencil Ref", Range(0, 255)) = 11
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "HDRenderPipeline"
            "Queue" = "Transparent+90"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
        }

        Pass
        {
            Name "XRaySilhouette"
            Tags { "LightMode" = "SRPDefaultUnlit" }

            Cull Back
            ZWrite Off
            ZTest Greater
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGB

            Stencil
            {
                Ref [_StencilRef]
                Comp NotEqual
                Pass Keep
            }

            HLSLPROGRAM
            #pragma target 4.5
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariablesFunctions.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _XRayColor;
                float4 _RimColor;
                float _RimPower;
                float _RimStrength;
                float _Intensity;
                float _StencilRef;
            CBUFFER_END

            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionRWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                output.positionRWS = TransformObjectToWorld(input.positionOS);
                output.positionCS = TransformWorldToHClip(output.positionRWS);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                return output;
            }

            float4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float3 normalWS = normalize(input.normalWS);
                float3 viewDirWS = GetWorldSpaceNormalizeViewDir(input.positionRWS);
                float rim = pow(saturate(1.0 - abs(dot(normalWS, viewDirWS))), _RimPower);

                float4 color = _XRayColor;
                color.rgb = color.rgb * _Intensity + _RimColor.rgb * rim * _RimStrength;
                color.a = saturate(color.a + _RimColor.a * rim * _RimStrength);
                return color;
            }
            ENDHLSL
        }
    }

    FallBack Off
}
