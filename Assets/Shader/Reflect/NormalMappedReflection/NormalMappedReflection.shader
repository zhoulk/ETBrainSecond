Shader "Custom/NormalMappedReflection"
{
	Properties
	{
		_MainTex("Main Tex", 2D) = "" {}
		_ReflAmount("ReflAmount", Range(0,1)) = 1
		_CubeMap("CubeMap", CUBE) = ""{}
		_NormalMask("ReflMask", 2D) = "" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		samplerCUBE _CubeMap;
		sampler2D _NormalMask;
		half _ReflAmount;

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_NormalMap;
			float3 worldRefl;
			INTERNAL_DATA
		};

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

			float3 normals = UnpackNormal(tex2D(_NormalMask, IN.uv_NormalMap)).rgb;
			o.Normal = normals;

			float3 reflection = texCUBE(_CubeMap, WorldReflectionVector(IN, o.Normal)).rgb;

			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Emission = reflection * _ReflAmount;
			/*o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;*/
			o.Alpha = c.a;
		}
		ENDCG
	}
		FallBack "Diffuse"
}
