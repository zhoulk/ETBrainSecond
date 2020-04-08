Shader "Custom/PhotoShopLevel"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        
		_inBlack("Input Black", Range(0,255)) = 0
		_inGamma("Input Gamma", Range(0,2)) = 1.61
		_inWhite("Input White", Range(0,255)) = 255

		_outBlack("Out Black", Range(0,255)) = 0
		_outWhite("Out White", Range(0,255)) = 255
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		half _inBlack;
		half _inGamma;
		half _inWhite;
		half _outBlack;
		half _outWhite;

        struct Input
        {
            float2 uv_MainTex;
        };

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

		float GetPixelLevel(float pixelColor) {
			float pixelResult;
			pixelResult = (pixelColor * 255);
			pixelResult = max(0, pixelResult - _inBlack);
			pixelResult = saturate(pow(pixelResult / (_inWhite - _inBlack), _inGamma));
			pixelResult = (pixelResult * (_outWhite - _outBlack) + _outBlack) / 255;
			return pixelResult;
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = float3(GetPixelLevel(c.r), GetPixelLevel(c.g), GetPixelLevel(c.b));
            // Metallic and smoothness come from slider variables
            //o.Metallic = _Metallic;
            //o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
