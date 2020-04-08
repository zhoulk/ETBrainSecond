Shader "Unlit/LitSphere"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "bump" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

	   CGPROGRAM
			#pragma surface surf Unlit vertex:vert

			#pragma target 3.0

			sampler2D _MainTex;
			sampler2D _NormalMap;

			inline half4 LightingUnlit(SurfaceOutput s, fixed3 lightDir, fixed atten)
			{
				half4 c = half4(1, 1, 1, 1);
				c.rgb = s.Albedo;
				c.a = s.Alpha;
				return c;
			}

			struct Input {
				float2 uv_MainTex;
				float2 uv_NormalMap;
				float3 tan1;
				float3 tan2;
			};

			void vert(inout appdata_full v, out Input o)
			{
				UNITY_INITIALIZE_OUTPUT(Input, o);

				TANGENT_SPACE_ROTATION;
				o.tan1 = mul(rotation, UNITY_MATRIX_IT_MV[0].xyz);
				o.tan2 = mul(rotation, UNITY_MATRIX_IT_MV[1].xyz);
			}

			void surf(Input IN, inout SurfaceOutput o)
			{
				float3 normals = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));
				o.Normal = normals;

				float2 litSphereUV;
				litSphereUV.x = dot(IN.tan1, o.Normal);
				litSphereUV.y = dot(IN.tan2, o.Normal);

				half4 c = tex2D(_MainTex, litSphereUV*0.5 + 0.5);
				o.Albedo = c.rgb;
				o.Alpha = c.a;
			}
		ENDCG
    }
		FallBack "Diffuse"
}
