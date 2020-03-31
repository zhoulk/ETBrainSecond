Shader "Myshader/CameraShader" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Brightness("Brightness", Float) = 1
	}
		SubShader{
			Pass {
				ZTest Always Cull Off ZWrite Off

				CGPROGRAM
				#pragma vertex vert  
				#pragma fragment frag  

				#include "UnityCG.cginc"  

				sampler2D _MainTex;
				half _Brightness;

				struct v2f {
					float4 pos : SV_POSITION;
					half2 uv: TEXCOORD0;
				};

				v2f vert(appdata_img v) {
					v2f o;

					o.pos = UnityObjectToClipPos(v.vertex);

					o.uv = v.texcoord;

					return o;
				}

				fixed4 frag(v2f i) : SV_Target {
					float2 uv = i.uv;

					fixed4 renderTex = tex2D(_MainTex, uv);

					fixed3 finalColor = renderTex.rgb * _Brightness;

					return fixed4(finalColor, renderTex.a);
				}

				ENDCG
			}
		}
			Fallback Off
}
