Shader "Hidden/UpscaleShader" {

    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _FogTex ("Fog Texture", 2D) = "white" {}
        _width ("Width", int) = 160
        _height ("Height", int) = 160
    }

    SubShader {

		ZWrite Off
        // Blend One OneMinusSrcAlpha

        Pass {
            CGPROGRAM

            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            uniform sampler2D _FogTex;

            int width;
            int height;

            float4 frag(v2f_img i) : COLOR {
                float4 c = tex2D(_FogTex, i.uv);
                // int2 tsize = textureSize(_FogTex);
                // float4 c2 = tex2D(_MainTex, i.uv);

                int u0 = floor(i.uv.x * width);
                int v0 = floor(i.uv.y * height);

                // float3 c3 = c.rgb * c.a;
                // c.a = 1;

                // return c2 + fixed4(c3.r, c3.g, c3.b, 0);
                
                return c;
            }

            ENDCG
        }
    }
}