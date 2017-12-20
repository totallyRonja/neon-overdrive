Shader "Custom/Grid" {
	Properties {
		_MainTex ("Albedo", 2D) = "white" {}
		_Repetition ("Rate of lights", Range(3.14, 20)) = 12
		_Intensity ("Emission intensity", Range(0, 1)) = 0.5
		_Speed ("Wandering Speed", Range(1, 10)) = 3
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
		half _Repetition;
		half _Intensity;
		half _Speed;

		uniform float4 distortionPoint0 = float4(0, 0, 0, 0);
		uniform float4 distortionPoint1 = float4(0, 0, 0, 0);
		uniform float radius0 = 0;
		uniform float radius1 = 0;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
			
			float2 uv = IN.uv_MainTex;

			//calculate influences of the distortion points
			float3 toPoint0 = distortionPoint0 - IN.worldPos;
			float3 toPoint1 = distortionPoint1 - IN.worldPos;

			float3 point1offset = normalize(toPoint0) * radius0 / length(toPoint0);
			float3 point2offset = normalize(toPoint1) * radius1 / length(toPoint1);

			uv += (point1offset + point2offset);

			fixed3 color = tex2D (_MainTex, uv).rgb;
			o.Albedo = color;

			//moving lines
			float lineIntensity = sin(clamp(fmod(_Time.y * _Speed + uv.y, _Repetition), 0, 3.14)) * _Intensity;

			o.Emission = color * lineIntensity;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
