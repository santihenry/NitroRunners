// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "track main"
{
	Properties
	{
		_trackheight("track - height", 2D) = "white" {}
		_tracknormal("track - normal", 2D) = "white" {}
		_tile("tile", Vector) = (0,0,0,0)
		_offset("offset", Vector) = (0,0,0,0)
		_wavesize("wave size", Float) = 0
		[HDR]_wavecolor("wave color", Color) = (0,0,0,0)
		_MainColor("Main Color", Color) = (0,0,0,0)
		_GapColor("Gap Color", Color) = (0,0,0,0)
		_Float5("Float 5", Range( 0 , 1)) = 0
		_Float4("Float 4", Range( 0 , 1)) = 0
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_detailnormal("detail normal", 2D) = "white" {}
		_Vector2("Vector 2", Vector) = (0,0,0,0)
		_Color0("Color 0", Color) = (0,0,0,0)
		_Vector3("Vector 3", Vector) = (40,40,0,0)
		_Float6("Float 6", Range( 0 , 2)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _tracknormal;
		uniform float2 _tile;
		uniform float2 _offset;
		uniform sampler2D _detailnormal;
		uniform float2 _Vector3;
		uniform float2 _Vector2;
		uniform float _Float6;
		uniform sampler2D _TextureSample0;
		uniform float4 _GapColor;
		uniform float4 _MainColor;
		uniform sampler2D _trackheight;
		uniform float4 _Color0;
		uniform float _wavesize;
		uniform float4 _wavecolor;
		uniform float _Float4;
		uniform float _Float5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord18 = i.uv_texcoord * _tile + _offset;
			float2 uv_TexCoord119 = i.uv_texcoord * _Vector3 + _Vector2;
			float4 temp_cast_0 = (_Float6).xxxx;
			float4 tex2DNode101 = tex2D( _TextureSample0, uv_TexCoord119 );
			float4 lerpResult118 = lerp( pow( tex2D( _tracknormal, uv_TexCoord18 ) , 1.0 ) , pow( tex2D( _detailnormal, uv_TexCoord119 ) , temp_cast_0 ) , tex2DNode101);
			o.Normal = lerpResult118.rgb;
			float temp_output_17_0 = saturate( (0.0 + (( tex2D( _trackheight, uv_TexCoord18 ).r * 1.0 ) - 0.75) * (1.0 - 0.0) / (0.78 - 0.75)) );
			float4 lerpResult42 = lerp( _GapColor , _MainColor , temp_output_17_0);
			float4 lerpResult122 = lerp( lerpResult42 , _Color0 , tex2DNode101);
			o.Albedo = lerpResult122.rgb;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float temp_output_36_0 = saturate( (_wavesize + (sin( ( ( ase_vertex3Pos.x * 1.2 ) + _Time.y ) ) - 0.0) * (1.0 - _wavesize) / (1.0 - 0.0)) );
			float lerpResult10 = lerp( temp_output_36_0 , 0.0 , temp_output_17_0);
			float mulTime52 = _Time.y * 6.0;
			float4 lerpResult125 = lerp( ( lerpResult10 * ( _wavecolor * (0.5 + (sin( mulTime52 ) - -1.0) * (1.0 - 0.5) / (1.0 - -1.0)) ) ) , float4( 0,0,0,0 ) , tex2DNode101);
			o.Emission = lerpResult125.rgb;
			o.Metallic = 1.0;
			float lerpResult128 = lerp( _Float4 , _Float5 , tex2DNode101.r);
			o.Smoothness = lerpResult128;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
212;483;892;301;1793.28;814.2019;3.846984;True;False
Node;AmplifyShaderEditor.PosVertexDataNode;26;-1546.984,173.6688;Inherit;True;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;24;-1652.378,-7.869365;Inherit;False;Property;_offset;offset;3;0;Create;True;0;0;False;0;0,0;0.21,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;23;-1648.987,-150.2641;Inherit;False;Property;_tile;tile;2;0;Create;True;0;0;False;0;0,0;3,3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;34;-1186.191,615.9511;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1231.557,324.377;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;1.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-1450.253,-141.6769;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-795.4162,402.3368;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1184.819,-3.727359;Inherit;True;Property;_trackheight;track - height;0;0;Create;True;0;0;False;0;-1;441592737c3402a4682926502ed08f07;441592737c3402a4682926502ed08f07;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;37;-313.1123,703.8152;Inherit;False;Property;_wavesize;wave size;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-1028.809,651.5912;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;52;-387.0681,244.4973;Inherit;False;1;0;FLOAT;6;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;27;-564.8442,396.7502;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;120;-616.4269,-797.3599;Inherit;False;Property;_Vector2;Vector 2;17;0;Create;True;0;0;False;0;0,0;0.08,0.34;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TFHCRemapNode;3;-754.0977,691.7358;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0.75;False;2;FLOAT;0.78;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;127;-614.049,-920.1017;Inherit;False;Property;_Vector3;Vector 3;19;0;Create;True;0;0;False;0;40,40;40,30;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TFHCRemapNode;35;-296.8349,414.3623;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-68;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;51;-68.65814,280.8788;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;54;166.9485,262.1859;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0.5;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;17;-338.9167,802.3987;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;40;68.9053,77.76184;Inherit;False;Property;_wavecolor;wave color;5;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.3923031,0,0.09510379,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;36;78.27414,567.7019;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;119;-352.49,-877.6448;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;10,60;False;1;FLOAT2;2,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;131;9.150508,-471.1416;Inherit;False;Property;_Float6;Float 6;20;0;Create;True;0;0;False;0;0;1.638529;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;41;-659.5842,-541.8522;Inherit;False;Property;_MainColor;Main Color;6;0;Create;True;0;0;False;0;0,0,0,0;0.374,0.44,0.638,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;8;-649.577,-83.63296;Inherit;True;Property;_tracknormal;track - normal;1;0;Create;True;0;0;False;0;-1;441592737c3402a4682926502ed08f07;441592737c3402a4682926502ed08f07;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;43;-638.1519,-342.851;Inherit;False;Property;_GapColor;Gap Color;7;0;Create;True;0;0;False;0;0,0,0,0;0.27,0.08526316,0.08526316,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;10;693.309,585.7343;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;405.4294,165.9924;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;116;53.52468,-768.1093;Inherit;True;Property;_detailnormal;detail normal;16;0;Create;True;0;0;False;0;-1;21dd3a9238747f345a38c583e89fd490;21dd3a9238747f345a38c583e89fd490;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;123;1063.35,-478.1459;Inherit;False;Property;_Color0;Color 0;18;0;Create;True;0;0;False;0;0,0,0,0;0.8679245,0.3883664,0.1514774,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;130;474.4795,-476.7476;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;881.7973,430.9496;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;9;-172.3314,-80.226;Inherit;True;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;42;-253.5715,-316.0485;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;100;776.6426,173.1872;Inherit;False;Property;_Float4;Float 4;14;0;Create;True;0;0;False;0;0;0.6575815;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;101;57.83408,-988.3294;Inherit;True;Property;_TextureSample0;Texture Sample 0;15;0;Create;True;0;0;False;0;-1;None;fbde231e294ce95419603961327037ce;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;129;768.0369,105.9347;Inherit;False;Property;_Float5;Float 5;13;0;Create;True;0;0;False;0;0;0.2879132;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;58;-762.5682,1165.061;Inherit;True;Simplex3D;False;False;2;0;FLOAT3;1,1,0;False;1;FLOAT;3.84;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;61;466.0631,1137.614;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;204.6251,1316.776;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;752.5492,29.45439;Inherit;False;Constant;_Float0;Float 0;8;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;125;1312.5,220.7214;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;77;714.0508,992.4238;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;92;431.7766,1001.292;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;59;902.6122,944.6171;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;76;577.398,992.8237;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;122;1345.304,-249.3453;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;72;-177.7578,914.0477;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;90;276.5855,793.0695;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;68;-1460.9,873.907;Inherit;False;Property;_Vector0;Vector 0;9;0;Create;True;0;0;False;0;2,1;3.3,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;74;-434.1935,1000.803;Inherit;False;Property;_Float2;Float 2;10;0;Create;True;0;0;False;0;0;2.43;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-1177.642,856.904;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;93;-950.0874,965.2814;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;75;176.5406,1037.401;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;94;-1540.451,1129.607;Inherit;False;Property;_Vector1;Vector 1;12;0;Create;True;0;0;False;0;0,0;0,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;95;-1450.975,1035.338;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;128;1410.512,54.7207;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-1157.667,1261.881;Inherit;False;Property;_Float1;Float 1;8;0;Create;True;0;0;False;0;0;6.47;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;96;-1174.558,1044.925;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;118;559.3704,-175.5472;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;91;79.14565,843.4627;Inherit;False;Property;_Float3;Float 3;11;0;Create;True;0;0;False;0;0;-2.48;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1727.096,-87.17467;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;track main;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;True;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;28;0;26;1
WireConnection;18;0;23;0
WireConnection;18;1;24;0
WireConnection;29;0;28;0
WireConnection;29;1;34;0
WireConnection;1;1;18;0
WireConnection;2;0;1;1
WireConnection;27;0;29;0
WireConnection;3;0;2;0
WireConnection;35;0;27;0
WireConnection;35;3;37;0
WireConnection;51;0;52;0
WireConnection;54;0;51;0
WireConnection;17;0;3;0
WireConnection;36;0;35;0
WireConnection;119;0;127;0
WireConnection;119;1;120;0
WireConnection;8;1;18;0
WireConnection;10;0;36;0
WireConnection;10;2;17;0
WireConnection;50;0;40;0
WireConnection;50;1;54;0
WireConnection;116;1;119;0
WireConnection;130;0;116;0
WireConnection;130;1;131;0
WireConnection;38;0;10;0
WireConnection;38;1;50;0
WireConnection;9;0;8;0
WireConnection;42;0;43;0
WireConnection;42;1;41;0
WireConnection;42;2;17;0
WireConnection;101;1;119;0
WireConnection;58;0;93;0
WireConnection;58;1;65;0
WireConnection;61;0;73;0
WireConnection;73;0;58;0
WireConnection;73;1;75;0
WireConnection;125;0;38;0
WireConnection;125;2;101;0
WireConnection;77;0;76;0
WireConnection;92;0;90;0
WireConnection;59;1;77;0
WireConnection;59;2;36;0
WireConnection;76;0;92;0
WireConnection;76;1;61;0
WireConnection;122;0;42;0
WireConnection;122;1;123;0
WireConnection;122;2;101;0
WireConnection;72;0;36;0
WireConnection;72;4;74;0
WireConnection;90;0;36;0
WireConnection;90;3;91;0
WireConnection;66;0;26;0
WireConnection;66;1;68;0
WireConnection;93;0;66;0
WireConnection;93;1;96;0
WireConnection;75;0;72;0
WireConnection;128;0;100;0
WireConnection;128;1;129;0
WireConnection;128;2;101;0
WireConnection;96;0;94;0
WireConnection;96;1;95;0
WireConnection;118;0;9;0
WireConnection;118;1;130;0
WireConnection;118;2;101;0
WireConnection;0;0;122;0
WireConnection;0;1;118;0
WireConnection;0;2;125;0
WireConnection;0;3;49;0
WireConnection;0;4;128;0
ASEEND*/
//CHKSM=9638296F013B0EDEA1B78782A44EA2D359809443