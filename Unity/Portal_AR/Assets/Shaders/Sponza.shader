Shader "Unlit/SponzaShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [Enum(CompareFunction)] _StencilComp("Stencil Comp", Int) = 3
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Stencil
            {
                Ref 1
                Comp [_StencilComp]
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the base texture
                fixed4 texColor = tex2D(_MainTex, i.uv);
                
                // Lighting: Simple Lambert shading
                float3 lightDir = normalize(float3(0.5, 1.0, 0.5)); // Fake light direction
                float diff = max(dot(normalize(i.worldNormal), lightDir), 0.0);
                
                // Final color
                fixed4 finalColor = texColor * (0.5 + diff); // Ensure texture shows with light influence
                return finalColor;
            }
            ENDCG
        }
    }
}
