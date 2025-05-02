Shader "Custom/MultiplyBlend"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Opacity ("Opacity", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent" 
            "RenderType"="Transparent"
            "PreviewType"="Plane"
        }
        
        Blend DstColor Zero
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Opacity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                fixed4 texColor = tex2D(_MainTex, uv);
                
                // 멀티플라이 효과와 농도 조절
                float darkness = 0.2;
                fixed3 darkColor = fixed3(darkness, darkness, darkness);
                fixed4 finalColor;
                
                // _Opacity로 어두운 정도를 직접 조절
                fixed3 multiplicativeColor = lerp(fixed3(1,1,1), darkColor, _Opacity);
                finalColor.rgb = texColor.rgb * multiplicativeColor;
                finalColor.a = 1.0;
                
                return finalColor;
            }
            ENDCG
        }
    }
}