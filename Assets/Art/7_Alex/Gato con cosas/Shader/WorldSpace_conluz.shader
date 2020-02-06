Shader "Danny/WOrldShader" {
	Properties{
		_Color("Main Color", Color) = (.5,.5,.5,1)
		_MainTex("Texture", 2D) = "white" {}
	}

		SubShader{
			Tags { "RenderType" = "Opaque" }
			Cull Off
			CGPROGRAM
			#pragma surface surf Standard

			sampler2D _MainTex;
			samplerCUBE _ToonShade;
			float4 _MainTex_ST;
			float4 _Color;

			struct Input {
				float3 worldNormal;
				float3 worldPos;
				float4 _Color;
			};
			struct v2f {
				float4 pos : POSITION;
				float2 texcoord : TEXCOORD0;
				float3 cubenormal : TEXCOORD1;
			};


			void surf(Input IN, inout SurfaceOutputStandard o) {

				if (abs(IN.worldNormal.y) > 0.5)
				{
					o.Albedo = tex2D(_MainTex, IN.worldPos.xz);
				}
				else if (abs(IN.worldNormal.x) > 0.5)
				{
					o.Albedo = tex2D(_MainTex, IN.worldPos.yz);
				}
				else
				{
					o.Albedo = tex2D(_MainTex, IN.worldPos.xy);
				}

			}

			ENDCG
	}
		SubShader{
	Tags { "RenderType" = "Opaque" "Queue" = "Transparent"}
	Pass {
		Name "BASE"
		SetTexture[_MainTex] {
			constantColor[_Color]
			Combine texture * constant
		}
		SetTexture[_ToonShade] {
			combine texture * previous DOUBLE, previous
		}
	}
			}
		FallBack "VertexLit"
}