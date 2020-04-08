Shader "Custom/SimpleAlpha"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_TransVal("Transparency Value", Range(0,1)) = 0.5
		_R("Red", Range(0,1)) = 0
		_G("Green", Range(0,1)) = 0
		_B("Blue", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Transparent" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		float _TransVal;
		half _R;
		half _G;
		half _B;

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

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            //o.Metallic = _Metallic;
            //o.Smoothness = _Glossiness;
			if (_R == 1) {
				o.Alpha = c.r;
			}
			else if (_G == 1) {
				o.Alpha = c.g;
			}
			else if (_B == 1) {
				o.Alpha = c.b;
			}
			else {
				o.Alpha = c.a;
			}
			
			o.Alpha = o.Alpha * _TransVal;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
