Shader "Custom/AnimateSprite"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}

        _CellAmount ("Cell Amount", float) = 0.0
        _Speed ("Speed", Range(0.01, 32)) = 12
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

        struct Input
        {
            float2 uv_MainTex;
        };

        half _CellAmount;
		half _Speed;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float2 spriteUV = IN.uv_MainTex;
			
			float cellUVPercent = 1 / _CellAmount;

			float cellIndex = fmod(_Time.y * _Speed, _CellAmount);
			cellIndex = ceil(cellIndex);

			spriteUV.x = cellIndex * cellUVPercent + spriteUV.x * cellUVPercent;

			//spriteUV.x += timeVal;
			//spriteUV.x *= cellUVPercent;
			
			// Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, spriteUV);
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            //o.Metallic = _Metallic;
            //o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
