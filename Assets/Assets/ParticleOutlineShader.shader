Shader "Custom/ParticleOutlineShader"
{
    Properties
    {
        _MainTex ("Particle Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness ("Outline Thickness", Range(0.0, 0.03)) = 0.005
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 outlinePos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _OutlineColor;
            float _OutlineThickness;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                float3 offset = float3(_OutlineThickness, _OutlineThickness, 0);
                o.outlinePos = UnityObjectToClipPos(v.vertex + float4(offset, 0));

                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.texcoord);
                half4 outlineCol = tex2D(_MainTex, i.outlinePos.xy);

                if (outlineCol.a > 0.1)
                {
                    return _OutlineColor;
                }

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
