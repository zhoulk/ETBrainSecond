Shader "Custom/DiffuseBump"
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white" {}
		_BumpTex("BumpTex", 2D) = "bump" {}
		_Tooniness("Tooniness", Range(0.1,20)) = 4
		_Ramp("Ramp Texture", 2D) = "white" {}
		_Outline("Outline", Range(0,1)) = 0.4
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        // #pragma surface surf Standard fullforwardshadows finalcolor:final
		#pragma surface surf Toon

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _BumpTex;
		float _Tooniness;
		sampler2D _Ramp;
		float _Outline;

        struct Input
        {
            float2 uv_MainTex;
			float2 uv_BumpTex;
			float3 viewDir;
        };

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

		half4 LightingToon(SurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten)
		{
			float difLight = max(0, dot(s.Normal, lightDir));
			float dif_hLambert = difLight * 0.5 + 0.5;

			float rimLight = max(0, dot(s.Normal, viewDir));
			float rim_hLambert = rimLight * 0.5 + 0.5;

			float3 ramp = tex2D(_Ramp, float2(rim_hLambert, dif_hLambert)).rgb;

			float4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * ramp;
			c.a = s.Alpha;
			return c;
		}

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Normal = UnpackNormal(tex2D(_BumpTex, IN.uv_BumpTex));

			half edge = saturate(dot(o.Normal, normalize(IN.viewDir)));
			edge = edge < _Outline ? edge / 4 : 1;

			o.Albedo = (floor(c.rgb * _Tooniness) / _Tooniness) * edge;

            // Metallic and smoothness come from slider variables
            /*o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;*/
            o.Alpha = c.a;
        }

		void final(Input IN, SurfaceOutput o, inout fixed4 color) {
			color = floor(color * _Tooniness) / _Tooniness;
		}

        ENDCG
    }
    FallBack "Diffuse"
}
