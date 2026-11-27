Shader "Custom/GodRay"
{
    Properties
    {
        _Color ("Ray Color", Color) = (1, 0.95, 0.7, 1)
        _Intensity ("Intensity", Range(0, 5)) = 1.5
        _MainTex ("Streak Texture (R)", 2D) = "white" {}
        _Mask ("Shape Mask (R)", 2D) = "white" {}

        _ScrollSpeed ("Scroll Speed", Float) = 0.05
        _LengthFade ("Fade Along Length", Range(0.1, 5)) = 1.5
        _EdgeSoftness ("Side Edge Softness", Range(0.01, 0.5)) = 0.35
        _ViewFade ("View Angle Fade", Range(0, 4)) = 1.0
        _FlickerAmount ("Flicker Amount", Range(0, 1)) = 0.15
        _FlickerSpeed ("Flicker Speed", Float) = 0.8
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }

        Blend SrcAlpha One      // additive
        ZWrite Off
        Cull Off
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex; float4 _MainTex_ST;
            sampler2D _Mask;    float4 _Mask_ST;
            fixed4 _Color;
            float _Intensity, _ScrollSpeed, _LengthFade, _EdgeSoftness;
            float _ViewFade, _FlickerAmount, _FlickerSpeed;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos       : SV_POSITION;
                float2 uv        : TEXCOORD0;
                float3 worldPos  : TEXCOORD1;
                float3 worldNorm : TEXCOORD2;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos       = UnityObjectToClipPos(v.vertex);
                o.uv        = v.uv;
                o.worldPos  = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNorm = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Streaks, scrolling sideways, two layers at different speeds for variation
                float2 uvA = TRANSFORM_TEX(i.uv, _MainTex) + float2(_Time.y * _ScrollSpeed, 0);
                float2 uvB = TRANSFORM_TEX(i.uv, _MainTex) * 1.7 + float2(-_Time.y * _ScrollSpeed * 0.6, 0);
                float streaks = tex2D(_MainTex, uvA).r * tex2D(_MainTex, uvB).r * 2.0;

                // Optional shape mask
                float mask = tex2D(_Mask, TRANSFORM_TEX(i.uv, _Mask)).r;

                // Fade along the ray: uv.y = 1 at the light source, 0 at the far end
                // (swap to i.uv.y if your quad is the other way round)
                float lengthFade = pow(saturate(i.uv.y), _LengthFade);

                // Soft sides so the quad edges never show
                float sides = smoothstep(0, _EdgeSoftness, i.uv.x) *
                              smoothstep(0, _EdgeSoftness, 1 - i.uv.x);

                // Fade out when viewed edge-on so you don't see a flat card
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float viewFade = pow(saturate(abs(dot(normalize(i.worldNorm), viewDir))), _ViewFade);

                // Gentle flicker
                float flicker = 1.0 - _FlickerAmount * (0.5 + 0.5 * sin(_Time.y * _FlickerSpeed + i.worldPos.x * 0.5));

                float a = streaks * mask * lengthFade * sides * viewFade * flicker * _Intensity;

                return fixed4(_Color.rgb, saturate(a * _Color.a));
            }
            ENDCG
        }
    }
}