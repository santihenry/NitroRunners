// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TurboFloor"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Speed("Speed", Range( 1 , 5)) = 1
		_Pixels("Pixels", Range( 20 , 50)) = 23.53677
		[HDR]_Color0("Color 0", Color) = (0,0,0,0)
		_Stripes("Stripes", Range( 0 , 50)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color0;
		uniform sampler2D _TextureSample0;
		uniform float _Speed;
		uniform float _Pixels;
		uniform float _Stripes;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = ( _Color0 * ( tex2D( _TextureSample0, ( round( ( ( (i.uv_texcoord*float2( 1,2 ) + ( ( _Time.y * _Speed ) * float2( 0,-1 ) )) * _Pixels ) + 0.5 ) ) / _Pixels ) ) * frac( ( i.uv_texcoord.x * _Stripes ) ) ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
0;0;2560;1059;2395.681;509.5796;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;18;-2201.076,-585.076;Inherit;False;1819.569;829.0378;Comment;9;6;14;10;15;5;13;17;11;7;Movimiento;0,0.3935943,1,1;0;0
Node;AmplifyShaderEditor.TimeNode;17;-2042.222,-492.3316;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;11;-2156.822,-123.9315;Inherit;False;Property;_Speed;Speed;1;0;Create;True;0;0;False;0;1;1;1;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-1707.222,-344.3316;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;5;-1754.547,-67.27695;Inherit;True;Constant;_Vector0;Vector 0;1;0;Create;True;0;0;False;0;0,-1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;25;-2202.99,299.3791;Inherit;False;1507.804;411;PIXELES;6;19;20;22;21;23;24;;1,0,0,1;0;0
Node;AmplifyShaderEditor.Vector2Node;14;-1432.222,-332.3316;Inherit;False;Constant;_Vector1;Vector 1;2;0;Create;True;0;0;False;0;1,2;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexCoordVertexDataNode;15;-1308.222,-538.3314;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-1449.022,-101.0315;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-2152.99,452.0973;Inherit;False;Property;_Pixels;Pixels;2;0;Create;True;0;0;False;0;23.53677;0;20;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;6;-1060.345,-222.7769;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;1,0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-1768.185,594.379;Float;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-1796.185,349.3791;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;35;-2201.004,827.3946;Inherit;False;1389.51;495.0001;Comment;5;28;31;29;30;32;Cilindros;0,1,0.1204233,1;0;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;28;-2151.004,879.4548;Inherit;True;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;21;-1487.185,352.3791;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-1813.494,1206.395;Inherit;False;Property;_Stripes;Stripes;4;0;Create;True;0;0;False;0;1;0;0;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;23;-1171.185,353.3791;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;29;-1733.494,877.3946;Inherit;True;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleDivideOpNode;24;-930.1853,359.3791;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-1349.494,880.3946;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;32;-1009.494,878.3946;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-746.5075,-196.7827;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;-1;efbc4a4390282de46a1d48a4c328de5a;efbc4a4390282de46a1d48a4c328de5a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-339.3959,-197.9063;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;27;-341.6613,-591.3647;Float;False;Property;_Color0;Color 0;3;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;12.51953,-585.8583;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;490.6674,-652.284;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;TurboFloor;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;13;0;17;2
WireConnection;13;1;11;0
WireConnection;10;0;13;0
WireConnection;10;1;5;0
WireConnection;6;0;15;0
WireConnection;6;1;14;0
WireConnection;6;2;10;0
WireConnection;20;0;6;0
WireConnection;20;1;19;0
WireConnection;21;0;20;0
WireConnection;21;1;22;0
WireConnection;23;0;21;0
WireConnection;29;0;28;0
WireConnection;24;0;23;0
WireConnection;24;1;19;0
WireConnection;30;0;29;0
WireConnection;30;1;31;0
WireConnection;32;0;30;0
WireConnection;7;1;24;0
WireConnection;33;0;7;0
WireConnection;33;1;32;0
WireConnection;26;0;27;0
WireConnection;26;1;33;0
WireConnection;0;0;26;0
ASEEND*/
//CHKSM=9618FDC1E4E8BF7A8D401244860CD0517EF11BAF