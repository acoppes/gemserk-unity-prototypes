Shader "Unlit/FogShader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { 
			"Queue"="Transparent"
			"RenderType"="Transparent" 		
			"IgnoreProjector"="True" 	
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				float2 texcoord  : TEXCOORD0;
			};

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				return OUT;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f IN) : SV_Target
			{
//				fixed4 c0 = tex2D (_MainTex, IN.texcoord + float2(-1, -1));
//				fixed4 c1 = tex2D (_MainTex, IN.texcoord + float2(-1,  0));
//				fixed4 c2 = tex2D (_MainTex, IN.texcoord + float2(-1,  1));
//				fixed4 c3 = tex2D (_MainTex, IN.texcoord + float2(0,  -1));
				fixed4 c4 = tex2D (_MainTex, IN.texcoord);
//				fixed4 c5 = tex2D (_MainTex, IN.texcoord + float2(0,  1));
//				fixed4 c6 = tex2D (_MainTex, IN.texcoord + float2(1,  -1));
//				fixed4 c7 = tex2D (_MainTex, IN.texcoord + float2(1,  0));
//				fixed4 c8 = tex2D (_MainTex, IN.texcoord + float2(1,  1));
				
				fixed4 c = fixed4(0, 0, 0, 0);
				// c.a = 1.f - (c4.r + c3.r * 0.2f + c5.r * 0.2f);
				c.a = 1.f - c4.r;
				// c.rgb = fixed3(0, 0, 0);
				return c;
			}
			ENDCG
		}
	}
}
