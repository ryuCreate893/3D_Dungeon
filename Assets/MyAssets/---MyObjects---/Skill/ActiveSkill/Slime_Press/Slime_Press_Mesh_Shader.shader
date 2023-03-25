Shader "Custom/Slime_Press_Mesh_Shader"
{
    Properties{
     _Color("Color", Color) = (1,0.3,0,0.4)
     _MainTex("Albedo (RGB)", 2D) = "white" {}
     _Rad("Radius", Range(0,3)) = 3
    }

        SubShader
     {
      Tags{
       "Queue" = "Transparent"
       "RenderType" = "Transparent"
      }

      LOD 200

      CGPROGRAM
       #pragma target 3.0
       #pragma surface surf Standard fullforwardshadows alpha

       fixed4 _Color;
       sampler2D _MainTex;
       float _Rad;

       struct Input {
        float2 uv_MainTex;
       };

       void surf(Input IN, inout SurfaceOutputStandard o) {
        fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
        float2 cp = float2(IN.uv_MainTex.x - 0.5, IN.uv_MainTex.y - 0.3);

        float r = distance(IN.uv_MainTex, cp);
        if (r > _Rad) {
            c.a = 0;
        }
        else {
            c.gb = frac((sqrt(cp.x * cp.x + cp.y * cp.y) * 2.5f) - (_Time.w * 0.5));
        }

       o.Albedo = c.rgb;
       o.Alpha = c.a;
      }
      ENDCG
     }
         FallBack "Transparent/Diffuse"
}