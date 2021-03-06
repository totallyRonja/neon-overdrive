﻿// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/ImageGrid" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_GridTex ("Grid (RGB)", 2D) = "white" {}

		_Repetition ("Rate of lights", Range(3.14, 20)) = 12
		_Intensity ("Emission intensity", Range(0, 1)) = 0.5
		_Speed ("Wandering Speed", Range(1, 10)) = 3
		_GridScale ("Image Distortion Scale (UV)", Vector) = (1, 1, 0, 0)
		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _GridTex;

		uniform float4 origin0 = float4(0, 0, 0, 0);
		uniform float4 origin1 = float4(0, 0, 0, 0);
		uniform float distortionPoint0 = 0;
		uniform float distortionPoint1 = 0;

		struct Input {
			float2 uv_MainTex;
			float2 uv_GridTex;
			float3 worldPos;
		};

		half _Repetition;
		half _Intensity;
		half _Speed;

		float4 _GridScale;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			float3 toPoint0 = origin0 - IN.worldPos;
			float3 toPoint1 = origin1 - IN.worldPos;

			float3 point1offset = normalize(toPoint0) * distortionPoint0 / length(toPoint0);
			float3 point2offset = normalize(toPoint1) * distortionPoint1 / length(toPoint1);

			float2 image_uv = IN.uv_MainTex + (point1offset + point2offset) * _GridScale.xy;
			float2 grid_uv = IN.uv_GridTex + point1offset + point2offset;

			float lineIntensity = sin(clamp(fmod(_Time.y * _Speed + grid_uv.y, _Repetition), 0, 3.14)) * _Intensity;

			// Albedo comes from a texture tinted by color
			fixed4 image = tex2D (_MainTex, image_uv);
			fixed4 c = lerp(tex2D(_GridTex, grid_uv), image, max(max(image.r,image.g), image.b));
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Emission = c.rgb * lineIntensity * (1-image.b);
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
