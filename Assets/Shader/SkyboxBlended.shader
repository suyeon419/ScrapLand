Shader "Skybox/Blended"
{
    Properties
    {
        _FrontTex ("Front (+Z)", 2D) = "white" {}
        _BackTex ("Back (-Z)", 2D) = "white" {}
        _LeftTex ("Left (+X)", 2D) = "white" {}
        _RightTex ("Right (-X)", 2D) = "white" {}
        _UpTex ("Up (+Y)", 2D) = "white" {}
        _DownTex ("Down (-Y)", 2D) = "white" {}

        _FrontTex2 ("Front2 (+Z)", 2D) = "white" {}
        _BackTex2 ("Back2 (-Z)", 2D) = "white" {}
        _LeftTex2 ("Left2 (+X)", 2D) = "white" {}
        _RightTex2 ("Right2 (-X)", 2D) = "white" {}
        _UpTex2 ("Up2 (+Y)", 2D) = "white" {}
        _DownTex2 ("Down2 (-Y)", 2D) = "white" {}

        _Blend ("Blend", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "Queue" = "Background" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t { float4 vertex : POSITION; float3 texcoord : TEXCOORD0; };
            struct v2f { float4 vertex : SV_POSITION; float3 texcoord : TEXCOORD0; };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            sampler2D _FrontTex, _BackTex, _LeftTex, _RightTex, _UpTex, _DownTex;
            sampler2D _FrontTex2, _BackTex2, _LeftTex2, _RightTex2, _UpTex2, _DownTex2;
            float _Blend;

            fixed4 frag (v2f i) : SV_Target
            {
                float3 absCoord = abs(i.texcoord);
                float maxCoord = max(max(absCoord.x, absCoord.y), absCoord.z);

                fixed4 col1, col2;
                if (absCoord.z >= absCoord.x && absCoord.z >= absCoord.y)
                {
                    if (i.texcoord.z > 0)
                    {
                        col1 = tex2D(_FrontTex, i.texcoord.xy * 0.5 + 0.5);
                        col2 = tex2D(_FrontTex2, i.texcoord.xy * 0.5 + 0.5);
                    }
                    else
                    {
                        col1 = tex2D(_BackTex, float2(1-i.texcoord.x, i.texcoord.y) * 0.5 + 0.5);
                        col2 = tex2D(_BackTex2, float2(1-i.texcoord.x, i.texcoord.y) * 0.5 + 0.5);
                    }
                }
                else if (absCoord.y >= absCoord.x)
                {
                    if (i.texcoord.y > 0)
                    {
                        col1 = tex2D(_UpTex, float2(i.texcoord.x, 1-i.texcoord.z) * 0.5 + 0.5);
                        col2 = tex2D(_UpTex2, float2(i.texcoord.x, 1-i.texcoord.z) * 0.5 + 0.5);
                    }
                    else
                    {
                        col1 = tex2D(_DownTex, float2(i.texcoord.x, i.texcoord.z) * 0.5 + 0.5);
                        col2 = tex2D(_DownTex2, float2(i.texcoord.x, i.texcoord.z) * 0.5 + 0.5);
                    }
                }
                else
                {
                    if (i.texcoord.x > 0)
                    {
                        col1 = tex2D(_RightTex, float2(i.texcoord.z, i.texcoord.y) * 0.5 + 0.5);
                        col2 = tex2D(_RightTex2, float2(i.texcoord.z, i.texcoord.y) * 0.5 + 0.5);
                    }
                    else
                    {
                        col1 = tex2D(_LeftTex, float2(1-i.texcoord.z, i.texcoord.y) * 0.5 + 0.5);
                        col2 = tex2D(_LeftTex2, float2(1-i.texcoord.z, i.texcoord.y) * 0.5 + 0.5);
                    }
                }
                return lerp(col1, col2, _Blend);
            }
            ENDCG
        }
    }
    Fallback "RenderFX/Skybox"
}
