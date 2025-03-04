Shader "Hidden/FullScreen/GaussianBlur"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Spread("Standard Deviation (Spread)", Float) = 1.0
        _GridSize("Grid Size", Integer) = 5
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #pragma only_renderers gles gles3 glcore d3d11
        #define E 2.71828f

        sampler2D _MainTex;
        float4 _MainTex_TexelSize;
        float _Spread;
        int _GridSize;

        float GaussianWeight(int x)
        {
            float sigmaSqu = _Spread * _Spread;
            return (1.0 / sqrt(TWO_PI * sigmaSqu)) * pow(E, -(x * x) / (2.0 * sigmaSqu));
        }

        struct appdata
        {
            float4 positionOS : Position;
            float2 uv : TEXCOORD0;
        };

        struct v2f
        {
            float4 positionCS : SV_Position;
            float2 uv : TEXCOORD0;
        };

        v2f vert(appdata v)
        {
            v2f o;
            o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
            o.uv = v.uv;
            return o;
        }
        ENDHLSL

        // Pass 1: Horizontal Blur
        Pass
        {
            Name "HorizontalBlur"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag_horizontal

            float4 frag_horizontal(v2f i) : SV_Target
            {
                float3 col = float3(0.0f, 0.0f, 0.0f);
                float gridSum = 0.0f;
                int halfSize = _GridSize / 2;

                for (int x = -halfSize; x <= halfSize; ++x)
                {
                    float weight = GaussianWeight(x);
                    gridSum += weight;
                    float2 uv = i.uv + float2(_MainTex_TexelSize.x * x, 0.0f);
                    col += weight * tex2D(_MainTex, uv).rgb;
                }

                col /= gridSum;
                return float4(col, 1.0f);
            }
            ENDHLSL
        }

        // Pass 2: Vertical Blur
        Pass
        {
            Name "VerticalBlur"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag_vertical

            float4 frag_vertical(v2f i) : SV_Target
            {
                float3 col = float3(0.0f, 0.0f, 0.0f);
                float gridSum = 0.0f;
                int halfSize = _GridSize / 2;

                for (int y = -halfSize; y <= halfSize; ++y)
                {
                    float weight = GaussianWeight(y);
                    gridSum += weight;
                    float2 uv = i.uv + float2(0.0f, _MainTex_TexelSize.y * y);
                    col += weight * tex2D(_MainTex, uv).rgb;
                }

                col /= gridSum;
                return float4(col, 1.0f);
            }
            ENDHLSL
        }
    }
}
