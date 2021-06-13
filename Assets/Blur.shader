// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Blur"
{
	Properties
	{
		_smoothness("smoothness", Float) = 1
		_size("size", Float) = 1
		_vigneteTex("vigneteTex", 2D) = "white" {}
		_Amount("Amount", Float) = 0
		[Toggle]_Debug("Debug", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		Cull Off
		ZWrite Off
		ZTest Always
		
		Pass
		{
			CGPROGRAM

			

			#pragma vertex Vert
			#pragma fragment Frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			
		
			struct ASEAttributesDefault
			{
				float3 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				
			};

			struct ASEVaryingsDefault
			{
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float2 texcoordStereo : TEXCOORD1;
			#if STEREO_INSTANCING_ENABLED
				uint stereoTargetEyeIndex : SV_RenderTargetArrayIndex;
			#endif
				
			};

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_TexelSize;
			uniform half4 _MainTex_ST;
			
			uniform float _Debug;
			uniform float _Amount;
			uniform sampler2D _vigneteTex;
			uniform float4 _vigneteTex_ST;
			uniform float _size;
			uniform float _smoothness;
			uniform float Lerp;


			
			float2 TransformTriangleVertexToUV (float2 vertex)
			{
				float2 uv = (vertex + 1.0) * 0.5;
				return uv;
			}

			ASEVaryingsDefault Vert( ASEAttributesDefault v  )
			{
				ASEVaryingsDefault o;
				o.vertex = float4(v.vertex.xy, 0.0, 1.0);
				o.texcoord = TransformTriangleVertexToUV (v.vertex.xy);
#if UNITY_UV_STARTS_AT_TOP
				o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
#endif
				o.texcoordStereo = TransformStereoScreenSpaceTex (o.texcoord, 1.0);

				v.texcoord = o.texcoordStereo;
				float4 ase_ppsScreenPosVertexNorm = float4(o.texcoordStereo,0,1);

				

				return o;
			}

			float4 Frag (ASEVaryingsDefault i  ) : SV_Target
			{
				float4 ase_ppsScreenPosFragNorm = float4(i.texcoordStereo,0,1);

				float2 uv_MainTex = i.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode7 = tex2D( _MainTex, uv_MainTex );
				float temp_output_5_0_g1 = (0.0 + (_Amount - 0.0) * (0.01 - 0.0) / (1.0 - 0.0));
				float4 appendResult14_g1 = (float4(( temp_output_5_0_g1 * -1.0 ) , 0.0 , 0.0 , 0.0));
				float2 uv025_g1 = i.texcoord.xy * float2( 1,1 ) + appendResult14_g1.xy;
				float4 appendResult18_g1 = (float4(temp_output_5_0_g1 , 0.0 , 0.0 , 0.0));
				float2 uv032_g1 = i.texcoord.xy * float2( 1,1 ) + appendResult18_g1.xy;
				float4 appendResult26_g1 = (float4(( temp_output_5_0_g1 * 1.0 ) , 0.0 , 0.0 , 0.0));
				float2 uv038_g1 = i.texcoord.xy * float2( 1,1 ) + appendResult26_g1.xy;
				float4 appendResult31_g1 = (float4(( temp_output_5_0_g1 * -2.0 ) , 0.0 , 0.0 , 0.0));
				float2 uv044_g1 = i.texcoord.xy * float2( 1,1 ) + appendResult31_g1.xy;
				float4 appendResult28_g1 = (float4(( temp_output_5_0_g1 * 2.0 ) , 0.0 , 0.0 , 0.0));
				float2 uv035_g1 = i.texcoord.xy * float2( 1,1 ) + appendResult28_g1.xy;
				float4 appendResult21_g1 = (float4(0.0 , ( temp_output_5_0_g1 * -1.0 ) , 0.0 , 0.0));
				float2 uv029_g1 = i.texcoord.xy * float2( 1,1 ) + appendResult21_g1.xy;
				float4 appendResult15_g1 = (float4(0.0 , temp_output_5_0_g1 , 0.0 , 0.0));
				float2 uv034_g1 = i.texcoord.xy * float2( 1,1 ) + appendResult15_g1.xy;
				float4 appendResult30_g1 = (float4(0.0 , ( temp_output_5_0_g1 * 1.0 ) , 0.0 , 0.0));
				float2 uv039_g1 = i.texcoord.xy * float2( 1,1 ) + appendResult30_g1.xy;
				float4 appendResult33_g1 = (float4(0.0 , ( temp_output_5_0_g1 * -2.0 ) , 0.0 , 0.0));
				float2 uv037_g1 = i.texcoord.xy * float2( 1,1 ) + appendResult33_g1.xy;
				float4 appendResult27_g1 = (float4(0.0 , ( temp_output_5_0_g1 * 2.0 ) , 0.0 , 0.0));
				float2 uv040_g1 = i.texcoord.xy * float2( 1,1 ) + appendResult27_g1.xy;
				float4 temp_output_16_0 = ( ( ( ( ( tex2D( _MainTex, uv025_g1 ) + tex2D( _MainTex, uv032_g1 ) ) + tex2D( _MainTex, uv038_g1 ) ) + ( tex2D( _MainTex, uv044_g1 ) + tex2D( _MainTex, uv035_g1 ) ) ) + ( ( ( tex2D( _MainTex, uv029_g1 ) + tex2D( _MainTex, uv034_g1 ) ) + tex2D( _MainTex, uv039_g1 ).r ) + ( tex2D( _MainTex, uv037_g1 ) + tex2D( _MainTex, uv040_g1 ) ) ) ) / 9.0 );
				float2 uv_vigneteTex = i.texcoord.xy * _vigneteTex_ST.xy + _vigneteTex_ST.zw;
				float temp_output_6_0 = saturate( pow( ( tex2D( _vigneteTex, uv_vigneteTex ).a * _size ) , _smoothness ) );
				float4 lerpResult14 = lerp( tex2DNode7 , temp_output_16_0 , temp_output_6_0);
				float4 lerpResult10 = lerp( tex2DNode7 , lerpResult14 , Lerp);
				float4 color11 = IsGammaSpace() ? float4(0.9716981,0.0229174,0.0229174,0) : float4(0.9368213,0.001773793,0.001773793,0);
				float4 color12 = IsGammaSpace() ? float4(0.2128198,0.945098,0.2,0) : float4(0.03724059,0.8796223,0.03310476,0);
				float4 lerpResult8 = lerp( ( tex2DNode7 * color11 ) , ( temp_output_16_0 * color12 ) , temp_output_6_0);
				

				float4 color = (( _Debug )?( lerpResult8 ):( lerpResult10 ));
				
				return color;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17800
304;151;998;459;1066.775;608.7877;2.436741;True;True
Node;AmplifyShaderEditor.RangedFloatNode;1;-1665.277,-753.0551;Inherit;False;Property;_size;size;1;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-1707.954,-1069.324;Inherit;True;Property;_vigneteTex;vigneteTex;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-1392.373,-851.1307;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;29;-1314.334,332.5119;Inherit;False;0;0;_MainTex;Pass;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;17;-878.7834,266.3752;Inherit;False;Property;_Amount;Amount;3;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-1558.258,-583.8639;Inherit;True;Property;_smoothness;smoothness;0;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;16;-489.0885,331.7282;Inherit;False;Blur;-1;;1;1fbbf17292ce9ad4e978c7146365056a;0;3;1;FLOAT;0;False;59;FLOAT;0;False;60;SAMPLER2D;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;5;-1241.614,-645.5526;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1.04;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;22;-663.4312,-1039.406;Inherit;False;1015.031;746.7111;;5;13;11;9;8;12;only for debuggin;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;11;-435.822,-989.4062;Inherit;False;Constant;_Color1;Color 0;4;0;Create;True;0;0;False;0;0.9716981,0.0229174,0.0229174,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;-1069.532,-336.6752;Inherit;True;Property;_TextureSample12;Texture Sample 11;4;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;6;-922.2031,-595.1378;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;23;-860.6428,86.94236;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;12;-613.4312,-821.8185;Inherit;False;Constant;_Color2;Color 1;4;0;Create;True;0;0;False;0;0.2128198,0.945098,0.2,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-71.51254,-647.6492;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-24.17962,-867.6046;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;14;-689.3215,-37.07058;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-604.9203,110.9309;Inherit;False;Global;Lerp;Lerp;14;0;Create;True;0;0;False;0;0;-0.01999997;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;10;-317.4426,-147.1491;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;8;134.7813,-505.8567;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;31;309.983,-170.1744;Inherit;False;Property;_Debug;Debug;4;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;30;613.3701,-200.2547;Float;False;True;-1;2;ASEMaterialInspector;0;10;Blur;71afb715c17dc8441903f67851f3c9ec;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;True;2;False;-1;False;False;True;2;False;-1;True;7;False;-1;False;False;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;0
WireConnection;4;0;2;4
WireConnection;4;1;1;0
WireConnection;16;59;17;0
WireConnection;16;60;29;0
WireConnection;5;0;4;0
WireConnection;5;1;3;0
WireConnection;7;0;29;0
WireConnection;6;0;5;0
WireConnection;23;0;16;0
WireConnection;9;0;16;0
WireConnection;9;1;12;0
WireConnection;13;0;7;0
WireConnection;13;1;11;0
WireConnection;14;0;7;0
WireConnection;14;1;23;0
WireConnection;14;2;6;0
WireConnection;10;0;7;0
WireConnection;10;1;14;0
WireConnection;10;2;15;0
WireConnection;8;0;13;0
WireConnection;8;1;9;0
WireConnection;8;2;6;0
WireConnection;31;0;10;0
WireConnection;31;1;8;0
WireConnection;30;0;31;0
ASEEND*/
//CHKSM=9310E99BA3C218496F996933D3BED8E6FB6078B1