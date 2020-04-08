Shader "Custom/BasicDiffuse"
{
	Properties
	{
		/*_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0*/

		_EmissiveColor("Emissive Color", Color) = (1,1,1,1)
		_AmbientColor("Ambient Color", Color) = (1,1,1,1)
		_MySliderValue("This is a slider", Range(0,10)) = 2.5
		_RampTex("Ramp Texture", 2D) = "white"{}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Standard fullforwardshadows
		#pragma surface surf BasicDiffuse

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		float4 _EmissiveColor;
		float4 _AmbientColor;
		float _MySliderValue;
		sampler2D _RampTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            //fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			float4 c;
			c = pow((_EmissiveColor + _AmbientColor), _MySliderValue);
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            //o.Metallic = _Metallic;
            //o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }

		//inline float4 LightingBasicDiffuse(SurfaceOutput s, fixed3 lightDir, fixed atten) 
		//{
		//	/*float difLight = max(0, dot(s.Normal, lightDir));
		//	float4 col;
		//	col.rgb = s.Albedo * _LightColor0.rgb * (difLight * atten * 2);
		//	col.a = s.Alpha;
		//	return col;*/

		//	/*float difLight = max(0, dot(s.Normal, lightDir));
		//	float hfDIfLight = difLight * 0.5 + 0.5;
		//	float4 col;
		//	col.rgb = s.Albedo * _LightColor0.rgb * (hfDIfLight * atten * 2);
		//	col.a = s.Alpha;
		//	return col;*/

		//	float difLight = max(0, dot(s.Normal, lightDir));
		//	float hfDIfLight = difLight * 0.5 + 0.5;
		//	float3 ramp = tex2D(_RampTex, float2(hfDIfLight, 0)).rgb;

		//	float4 col;
		//	col.rgb = s.Albedo * _LightColor0.rgb * (ramp);
		//	col.a = s.Alpha;
		//	return col;
		//}

		inline float4 LightingBasicDiffuse(SurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten)
		{
			/*float difLight = max(0, dot(s.Normal, lightDir));
			float4 col;
			col.rgb = s.Albedo * _LightColor0.rgb * (difLight * atten * 2);
			col.a = s.Alpha;
			return col;*/

			/*float difLight = max(0, dot(s.Normal, lightDir));
			float hfDIfLight = difLight * 0.5 + 0.5;
			float4 col;
			col.rgb = s.Albedo * _LightColor0.rgb * (hfDIfLight * atten * 2);
			col.a = s.Alpha;
			return col;*/

			float difLight = max(0, dot(s.Normal, lightDir));
			float hfDIfLight = difLight * 0.5 + 0.5;

			float rimLight = max(0, dot(s.Normal, viewDir));
			float hfRimLLight = rimLight * 0.5 + 0.5;

			float3 ramp = tex2D(_RampTex, float2(hfDIfLight, hfRimLLight)).rgb;

			float4 col;
			col.rgb = s.Albedo * _LightColor0.rgb * (ramp);
			col.a = s.Alpha;
			return col;
		}

        ENDCG
    }
    FallBack "Diffuse"
}
