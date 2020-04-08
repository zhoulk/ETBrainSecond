Shader "Custom/ScrollingUVs"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
       
		_ScrollXSpeed("X Scroll Speed", Range(0,10)) = 2
		_ScrollYSpeed("Y Scroll Speed", Range(0,10)) = 2
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
		float _ScrollXSpeed;
		float _ScrollYSpeed;

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
			fixed2 scrollUV = IN.uv_MainTex;

			fixed xScrollValue = _ScrollXSpeed * _Time;
			fixed yScrollValue = _ScrollYSpeed * _Time;

			scrollUV += fixed2(xScrollValue, yScrollValue);
		/*	scrollUV.x = fmod(scrollUV.x, 1);
			scrollUV.y = fmod(scrollUV.y, 1);*/
			scrollUV = fmod(scrollUV, fixed2(1, 1));

            // Albedo comes from a texture tinted by color
			half4  c = tex2D(_MainTex, scrollUV);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
