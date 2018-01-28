// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Planet" {
	Properties {
		_MainTex("Albedo (RGB), Roughness (A)", 2D) = "white" {}
		_NormalTex("Normal (RGB), Height (A)", 2D) = "white" {}
		_HeightTex("Height (RGB), Height (A)", 2D) = "white" {}
		_Height("Height Scale", Range(0,1)) = 0.1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:vert fullforwardshadows
		#pragma target 5.0

		sampler2D _MainTex;
		sampler2D _NormalTex;
		sampler2D _HeightTex;
		float _Height;

		struct Input {
			float2 uv_MainTex;
			float4 normal;
			float4 position;
		};
		
		void vert(inout appdata_full v) {
			float height = tex2Dlod(_HeightTex, v.texcoord).x * _Height;
			v.vertex.xyz += v.normal * height;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 norm = tex2D(_NormalTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Smoothness = 1.0 - c.a;
			o.Normal = UnpackNormal(norm);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
