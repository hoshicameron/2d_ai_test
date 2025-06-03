Shader "PetalsOfHope/Dissolve"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0
        _EdgeWidth ("Edge Width", Range(0, 0.2)) = 0.1
        _EdgeColor ("Edge Color", Color) = (1, 1, 0, 1)
        _EdgeGlow ("Edge Glow", Range(0, 10)) = 2
        _ScrollSpeed ("Scroll Speed", Vector) = (0, 0, 0, 0)
    }
    
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100
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
                float3 normal : NORMAL;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
            float4 _Color;
            float _DissolveAmount;
            float _EdgeWidth;
            float4 _EdgeColor;
            float _EdgeGlow;
            float2 _ScrollSpeed;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the noise texture with scrolling
                float2 noiseUV = i.uv + _Time.y * _ScrollSpeed;
                float noise = tex2D(_NoiseTex, noiseUV).r;
                
                // Calculate dissolve edge
                float dissolve = step(noise - _EdgeWidth, _DissolveAmount);
                float edge = smoothstep(0, _EdgeWidth, noise - (_DissolveAmount - _EdgeWidth)) * 
                            step(_DissolveAmount - _EdgeWidth, noise);
                
                // Sample the main texture
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                
                // Apply edge glow
                col.rgb = lerp(col.rgb, _EdgeColor.rgb, edge * _EdgeGlow);
                
                // Clip pixels based on dissolve
                clip(dissolve - 0.5);
                
                return col;
            }
            ENDCG
        }
    }
    
    FallBack "Diffuse"
}
