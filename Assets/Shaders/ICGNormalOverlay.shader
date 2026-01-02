Shader "ICG/NormalOverlay"
{
    Properties
    {
        _Opacity ("Opacity", Range(0,1)) = 0.6
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _Opacity;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 nView : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                float3 nWorld = UnityObjectToWorldNormal(v.normal);
                float3 nView = mul((float3x3)UNITY_MATRIX_V, nWorld);
                o.nView = normalize(nView);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 n = normalize(i.nView);
                fixed3 col = n * 0.5 + 0.5;
                return fixed4(col, _Opacity);
            }
            ENDCG
        }
    }
}
