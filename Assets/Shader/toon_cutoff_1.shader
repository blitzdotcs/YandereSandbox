Shader "Toon/Cutoff" {
Properties {
 _Color ("Color(RGBA)", Color) = (1,1,1,1)
 _MainTex ("Texture", 2D) = "white" {}
 _Shininess ("Shininess(0.0:)", Float) = 1
 _ShadowThreshold ("Shadow Threshold(0.0:1.0)", Float) = 0.5
 _ShadowColor ("Shadow Color(RGBA)", Color) = (0,0,0,0.5)
 _ShadowSharpness ("Shadow Sharpness(0.0:)", Float) = 100
}
	//DummyShaderTextExporter
	
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard fullforwardshadows
#pragma target 3.0
		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
		}
		ENDCG
	}
}