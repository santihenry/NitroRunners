// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "  "
{
	Properties
	{
		_MainTexture("Main Texture", 2D) = "white" {}
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Tilingrosa("Tiling rosa", Float) = 0
		_Tilingazul("Tiling azul", Float) = 0
		_Speed("Speed", Float) = 0
		_SpeedAZUL("SpeedAZUL", Float) = 0
		[HDR]_PrimerColor("Primer Color", Color) = (1,1,1,1)
		[HDR]_Color0("Color 0", Color) = (1,1,1,1)
		[HDR]_SegundoColor("Segundo Color", Color) = (1,1,1,1)
		_Shielpower("Shiel power", Float) = 0
		_Float5("Float 5", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float4 screenPosition1;
			float2 uv_texcoord;
		};

		uniform float _Shielpower;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Float5;
		uniform sampler2D _MainTexture;
		uniform float _Speed;
		uniform float _Tilingrosa;
		uniform float4 _PrimerColor;
		uniform sampler2D _TextureSample0;
		uniform float _SpeedAZUL;
		uniform float _Tilingazul;
		uniform float4 _Color0;
		uniform float4 _SegundoColor;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 vertexPos1 = ase_vertex3Pos;
			float4 ase_screenPos1 = ComputeScreenPos( UnityObjectToClipPos( vertexPos1 ) );
			o.screenPosition1 = ase_screenPos1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV21 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode21 = ( 0.0 + 0.88 * pow( 1.0 - fresnelNdotV21, (0.0 + (_Shielpower - 0.0) * (1.0 - 0.0) / (1.0 - 0.0)) ) );
			float temp_output_109_0 = ( fresnelNode21 * 1.0 );
			float4 ase_screenPos1 = i.screenPosition1;
			float4 ase_screenPosNorm1 = ase_screenPos1 / ase_screenPos1.w;
			ase_screenPosNorm1.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm1.z : ase_screenPosNorm1.z * 0.5 + 0.5;
			float screenDepth1 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm1.xy ));
			float distanceDepth1 = saturate( abs( ( screenDepth1 - LinearEyeDepth( ase_screenPosNorm1.z ) ) / ( _Float5 ) ) );
			float mulTime99 = _Time.y * _Speed;
			float4 appendResult98 = (float4(_Tilingrosa , _Tilingrosa , 0.0 , 0.0));
			float2 uv_TexCoord100 = i.uv_texcoord * appendResult98.xy;
			float2 panner112 = ( mulTime99 * float2( 1,0 ) + uv_TexCoord100);
			float3 temp_cast_1 = (tex2D( _MainTexture, panner112 ).r).xxx;
			float grayscale101 = Luminance(temp_cast_1);
			float mulTime119 = _Time.y * _SpeedAZUL;
			float4 appendResult117 = (float4(_Tilingazul , _Tilingazul , 0.0 , 0.0));
			float2 uv_TexCoord121 = i.uv_texcoord * appendResult117.xy;
			float2 panner122 = ( mulTime119 * float2( -1,0 ) + uv_TexCoord121);
			float3 temp_cast_3 = (tex2D( _TextureSample0, panner122 ).g).xxx;
			float grayscale125 = Luminance(temp_cast_3);
			o.Emission = saturate( ( ( ( temp_output_109_0 + ( 1.0 - distanceDepth1 ) ) * ( ( grayscale101 * _PrimerColor ) + ( grayscale125 * _Color0 ) ) ) * _SegundoColor ) ).rgb;
			o.Alpha = temp_output_109_0;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
153;94;1559;822;4712.577;64.70422;1.887203;True;False
Node;AmplifyShaderEditor.CommentaryNode;115;-3034.921,-99.35725;Inherit;False;2145.905;851.8096;Patron Rosa;11;94;98;97;100;114;99;112;7;101;102;103;;0.9245283,0.4666252,0.8537742,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;127;-3045.337,774.1534;Inherit;False;2145.905;851.8094;PatronCeleste;11;116;117;118;119;121;122;123;124;125;134;132;;0.2877358,0.9860509,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;94;-3004.555,-1.963473;Float;False;Property;_Tilingrosa;Tiling rosa;2;0;Create;True;0;0;False;0;0;2.482353;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;116;-2992.491,881.5388;Float;False;Property;_Tilingazul;Tiling azul;3;0;Create;True;0;0;False;0;0;2.482353;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;98;-2650.722,-25.35719;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;118;-2995.337,1201.302;Float;False;Property;_SpeedAZUL;SpeedAZUL;5;0;Create;True;0;0;False;0;0;0.14;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;117;-2661.138,848.1535;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-2984.921,327.7911;Float;False;Property;_Speed;Speed;4;0;Create;True;0;0;False;0;0;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;119;-2531.138,1234.153;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;114;-2487.827,197.0243;Inherit;False;Constant;_Vector0;Vector 0;6;0;Create;True;0;0;False;0;1,0;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;99;-2520.722,360.6428;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;100;-2425.923,-76.19425;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;121;-2424.141,824.1534;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;134;-2554.194,1007.082;Inherit;False;Constant;_Vector1;Vector 1;10;0;Create;True;0;0;False;0;-1,0;2,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;112;-2086.47,203.6545;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;104;-2483.495,-1165.135;Inherit;True;Property;_Shielpower;Shiel power;9;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;122;-2165.954,1077.165;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PosVertexDataNode;137;-2249.506,-654.2144;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;142;-2153.805,-1451.967;Inherit;True;Constant;_Float0;Float 0;11;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;123;-1857.829,1049.772;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;-1;None;69dc29d658644a548b184956107f9bb0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;136;-2317.544,-308.389;Inherit;False;Property;_Float5;Float 5;10;0;Create;True;0;0;False;0;0;1.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-1847.413,176.2609;Inherit;True;Property;_MainTexture;Main Texture;0;0;Create;True;0;0;False;0;-1;None;69dc29d658644a548b184956107f9bb0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;105;-2107.42,-1153.472;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;102;-1548.206,545.4523;Inherit;False;Property;_PrimerColor;Primer Color;6;1;[HDR];Create;True;0;0;False;0;1,1,1,1;7.70914,8.069035,0,0.1568628;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;1;-1845.601,-506.5663;Inherit;False;True;True;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCGrayscale;101;-1498.83,199.3928;Inherit;True;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;124;-1558.623,1418.963;Inherit;False;Property;_Color0;Color 0;7;1;[HDR];Create;True;0;0;False;0;1,1,1,1;9.089006,1.01643,0,0.2784314;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;110;-1870.071,-882.7686;Float;True;Constant;_Float2;Float 2;6;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;21;-1785.185,-1240.715;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0.88;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCGrayscale;125;-1509.246,1072.903;Inherit;True;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;103;-1124.016,252.8122;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;140;-1486.805,-582.7808;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;-1393.071,-939.7687;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;132;-1159.245,870.98;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;130;-537.0717,-92.62207;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;138;-956.1134,-732.4237;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;108;-185.9814,163.4371;Inherit;False;Property;_SegundoColor;Segundo Color;8;1;[HDR];Create;True;0;0;False;0;1,1,1,1;0.1270444,1.516593,0.3558485,0.2352941;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;144;-321.906,-402.3444;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;141;28.86853,-417.6698;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;145;-3846.351,378.7885;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;143;290.419,-529.2495;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;536.6622,-594.3218;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;  ;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;98;0;94;0
WireConnection;98;1;94;0
WireConnection;117;0;116;0
WireConnection;117;1;116;0
WireConnection;119;0;118;0
WireConnection;99;0;97;0
WireConnection;100;0;98;0
WireConnection;121;0;117;0
WireConnection;112;0;100;0
WireConnection;112;2;114;0
WireConnection;112;1;99;0
WireConnection;122;0;121;0
WireConnection;122;2;134;0
WireConnection;122;1;119;0
WireConnection;123;1;122;0
WireConnection;7;1;112;0
WireConnection;105;0;104;0
WireConnection;1;1;137;0
WireConnection;1;0;136;0
WireConnection;101;0;7;1
WireConnection;21;1;142;0
WireConnection;21;3;105;0
WireConnection;125;0;123;2
WireConnection;103;0;101;0
WireConnection;103;1;102;0
WireConnection;140;0;1;0
WireConnection;109;0;21;0
WireConnection;109;1;110;0
WireConnection;132;0;125;0
WireConnection;132;1;124;0
WireConnection;130;0;103;0
WireConnection;130;1;132;0
WireConnection;138;0;109;0
WireConnection;138;1;140;0
WireConnection;144;0;138;0
WireConnection;144;1;130;0
WireConnection;141;0;144;0
WireConnection;141;1;108;0
WireConnection;143;0;141;0
WireConnection;0;2;143;0
WireConnection;0;9;109;0
ASEEND*/
//CHKSM=E936369C7B7E38C9E2F866215DCD8FC7A703B088