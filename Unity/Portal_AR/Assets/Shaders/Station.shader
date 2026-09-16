Shader "Custom/Station"
{
    Properties
    {
        _BaseMap ("Albedo (RGB)", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
        _Opacity ("Opacity", Range(0,1)) = 1.0 // Transparency control
        _EmissionMap ("Emission (RGB)", 2D) = "black" {} // Emission Texture
        [HDR] _EmissionColor ("Emission Color", Color) = (0,0,0,1) // Emission Strength
        [Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp("Stencil Comp", Int) = 3
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha // Enables transparency
            ZWrite Off                      // Fixes depth sorting issues

            Stencil
            {
                Ref 1
                Comp [_StencilComp]
                Pass Replace
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 viewDirWS : TEXCOORD2;
                float4 positionCS : SV_POSITION;
                float3 tangentWS : TEXCOORD3;
                float3 bitangentWS : TEXCOORD4;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);
            TEXTURE2D(_EmissionMap);
            SAMPLER(sampler_EmissionMap);

            float _Metallic;
            float _Smoothness;
            float _Opacity;
            float4 _EmissionColor; // HDR Emission Strength

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs posInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                VertexNormalInputs normalInputs = GetVertexNormalInputs(IN.normalOS);

                OUT.positionCS = posInputs.positionCS;
                OUT.uv = IN.uv;
                OUT.normalWS = normalInputs.normalWS;
                OUT.viewDirWS = GetWorldSpaceViewDir(posInputs.positionWS);
                
                // Calculate tangent and bitangent
                OUT.tangentWS = TransformObjectToWorldDir(IN.tangentOS.xyz);
                OUT.bitangentWS = cross(OUT.normalWS, OUT.tangentWS) * IN.tangentOS.w; // Fixes TBN matrix issue
                
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                // Sample the base texture with transparency
                half4 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                float alpha = albedo.a * _Opacity; // Use texture alpha and opacity control

                // Create TBN matrix for correct normal mapping
                float3 normalTS = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, IN.uv));
                float3x3 TBN = float3x3(IN.tangentWS, IN.bitangentWS, IN.normalWS);
                float3 normalWS = normalize(mul(normalTS, TBN));

                // Lighting Calculation
                Light mainLight = GetMainLight();
                float3 lightDir = normalize(mainLight.direction);
                float diff = max(dot(normalWS, lightDir), 0.0);
                float3 lighting = mainLight.color * diff;

                // **Fresnel Effect** for glass reflections
                float fresnel = pow(1.0 - max(dot(normalWS, normalize(IN.viewDirWS)), 0.0), 5.0);
                float3 reflection = fresnel * 0.5; // Adjust reflection intensity

                // **Emission**
                float3 emission = SAMPLE_TEXTURE2D(_EmissionMap, sampler_EmissionMap, IN.uv).rgb * _EmissionColor.rgb;

                // Final transparent material color with emission
                float3 finalColor = albedo.rgb * lighting + reflection + emission;
                return half4(finalColor, alpha); // Ensures transparency & emission
            }
            ENDHLSL
        }
    }
}
