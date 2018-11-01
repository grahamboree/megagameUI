Shader "Archillect/Sphere" {
	Properties {
		_LineColor("Line Color", Color) = (1,1,1,1)
		_GridColor("Grid Color", Color) = (0,0,0,0)
		_LineWidth("Line Width", float) = 0.1
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform float4 _LineColor;
			uniform float4 _GridColor;
			uniform float _LineWidth;

			struct appdata {
				float4 vertex : POSITION;
				float3 barycentric : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float3 barycentric : TEXCOORD0;
				float3 world : TEXCOORD2;
			};

			v2f vert(appdata v) {
				v2f o;
				o.world = v.vertex;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.barycentric = v.barycentric;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target {
				float threshold = 0.08f;
				bool xyless = i.barycentric.x + i.barycentric.y < threshold;
				bool xzless = i.barycentric.x + i.barycentric.z < threshold;
				bool yzless = i.barycentric.y + i.barycentric.z < threshold;

				// Vertex dots (actually hexagons, but they're too small to notice)
				if (xyless || xzless || yzless) {
					return _LineColor;
				}

				// Edges
				float halfWidth = _LineWidth / 2.0f;
				if (i.barycentric.x < halfWidth || i.barycentric.y < halfWidth || i.barycentric.z < halfWidth) {
					return lerp(_GridColor, _LineColor, abs(length(i.world) - 1) / 0.2f);
				}

				// Faces
				return _GridColor;
			}
			ENDCG
		}
	}
}
