Shader "BYCW/FirrstUnlitShader"
{
    Properties
    {
        _MainTex ("FIrstTexture", 2D) = "white" {}

        _Color ("Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        LOD 100 //level of details

        Blend SrcAlpha OneMinusSrcAlpha

        // ZWrite off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert

            #pragma fragment frag

            #include "UnityCG.cginc"    //这些类库在 D:\unity\unityEditor\2022.3.17f1c1\Editor\Data\CGIncludes 下面

            

            struct appdata
            {
                float4 vertex : POSITION;

                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;

                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;

            float4 _MainTex_ST;

            fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                //根据Tilling和offset进行纹理坐标变换
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture

                fixed4 col = tex2D(_MainTex, i.uv);

                return col * _Color;
            }
            ENDCG
        }
    }
}
