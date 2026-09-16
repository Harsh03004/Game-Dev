Shader "Unlit/Portal"
{
    Properties
    {
        _MainTex ("Base Map", 2D) = "white" {}  // Albedo texture
        _MetallicMap ("Metallic Map", 2D) = "black" {} // Metallic texture
        _NormalMap ("Normal Map", 2D) = "bump" {} // Normal texture
        _Metallic ("Metallic", Range(0,1)) = 0.5  // Metallic intensity
        _Smoothness ("Smoothness", Range(0,1)) = 0.5  // Smoothness intensity
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD2;
                float3 tangentNormal : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _MetallicMap;
            sampler2D _NormalMap;
            float _Metallic;
            float _Smoothness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                // Compute normal in world space
                o.normal = UnityObjectToWorldNormal(v.normal);

                // Sample normal map in tangent space
                o.tangentNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));

                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Sample base color texture
                fixed4 albedo = tex2D(_MainTex, i.uv);

                // Sample metallic map and apply metallic & smoothness
                float metallic = tex2D(_MetallicMap, i.uv).r * _Metallic;
                float smoothness = tex2D(_MetallicMap, i.uv).g * _Smoothness;

                // Sample normal map
                float3 normalTangent = UnpackNormal(tex2D(_NormalMap, i.uv));
                float3 normalWorld = normalize(mul(normalTangent, (float3x3)unity_WorldToObject));

                // Basic lighting
                float3 lightDir = normalize(float3(0.0, 10.0, 10.0));
                float l = max(dot(lightDir, normalWorld), 0.0);

                // Metallic reflection effect
                float3 reflectColor = lerp(albedo.rgb, float3(1,1,1), metallic);
                float3 finalColor = (reflectColor * l) + float3(smoothness, smoothness, smoothness) * 0.3;

                return fixed4(finalColor, 1.0);
            }
            ENDCG
        }
    }
}
