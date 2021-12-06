Shader "Custom/My Trsnsparent" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
    _AlphaTex("Alpha Texture", 2D) = "white" {}
}

SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
    LOD 200

CGPROGRAM
#pragma surface surf Lambert alpha:fade

sampler2D _MainTex;
sampler2D _AlphaTex;
fixed4 _Color;

struct Input {
    float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
    fixed4 main_color = tex2D(_MainTex, IN.uv_MainTex);
    fixed4 alpha_color = tex2D(_AlphaTex, IN.uv_MainTex);
    o.Albedo = main_color.rgb;
    o.Alpha =  main_color.a * max( 1-alpha_color.g, 0);
}
ENDCG
}

Fallback "Legacy Shaders/Transparent/VertexLit"
}
