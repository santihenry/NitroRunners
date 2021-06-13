// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Scaner"
{
	Properties
	{
		_lenght("lenght", Float) = 0
		_strenght("strenght", Float) = 0
		_Color("Color", Color) = (0,0,0,0)
		_waves("waves", Float) = 13.24
		_waveheight("wave height", Range( 0 , 1)) = 0.5324244
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow exclude_path:deferred 
		struct Input
		{
			float4 screenPos;
			float3 worldPos;
		};

		uniform float4 _Color;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _lenght;
		uniform float _strenght;
		uniform float _waveheight;
		uniform float _waves;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth12 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth12 = abs( ( screenDepth12 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _lenght ) );
			float temp_output_16_0 = ( 1.0 - pow( distanceDepth12 , _strenght ) );
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float mulTime44 = _Time.y * 2.0;
			float lerpResult36 = lerp( temp_output_16_0 , (temp_output_16_0*1.0 + _waveheight) , saturate( (0.0 + (sin( ( ( ase_vertex3Pos.z * _waves ) + mulTime44 ) ) - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) ));
			float temp_output_19_0 = saturate( lerpResult36 );
			float4 lerpResult17 = lerp( float4( 0,0,0,0 ) , _Color , temp_output_19_0);
			o.Emission = lerpResult17.rgb;
			o.Alpha = temp_output_19_0;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
237;414;998;334;926.8184;-367.3418;1;True;False
Node;AmplifyShaderEditor.PosVertexDataNode;32;-1254.004,426.9367;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;30;-1242.293,635.8083;Inherit;False;Property;_waves;waves;3;0;Create;True;0;0;False;0;13.24;25.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-980.5535,148.2881;Inherit;False;Property;_lenght;lenght;0;0;Create;True;0;0;False;0;0;2.23;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-923.6459,488.3948;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;44;-979.7485,815.2401;Inherit;False;1;0;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;42;-710.2363,577.5168;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-615.72,281.3499;Inherit;False;Property;_strenght;strenght;1;0;Create;True;0;0;False;0;0;0.75;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;12;-754.8861,118.4866;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;27;-499.9759,595.4688;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;14;-403.7772,128.7502;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;16;-188.0154,169.5471;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;40;-334.8207,625.1448;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-409.1087,444.3076;Inherit;False;Property;_waveheight;wave height;4;0;Create;True;0;0;False;0;0.5324244;0.4971304;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;37;-54.7488,571.6022;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;24;-80.03211,416.4644;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;36;289.6278,406.6771;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;19;484.498,404.9985;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;18;30.85999,-94.53045;Inherit;False;Property;_Color;Color;2;0;Create;True;0;0;False;0;0,0,0,0;0,0.9600265,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;17;652.306,113.499;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1038.872,86.22039;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Scaner;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.12;0,0,0,0;VertexScale;True;False;Cylindrical;False;Relative;0;;2;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;33;0;32;3
WireConnection;33;1;30;0
WireConnection;42;0;33;0
WireConnection;42;1;44;0
WireConnection;12;0;10;0
WireConnection;27;0;42;0
WireConnection;14;0;12;0
WireConnection;14;1;15;0
WireConnection;16;0;14;0
WireConnection;40;0;27;0
WireConnection;37;0;40;0
WireConnection;24;0;16;0
WireConnection;24;2;41;0
WireConnection;36;0;16;0
WireConnection;36;1;24;0
WireConnection;36;2;37;0
WireConnection;19;0;36;0
WireConnection;17;1;18;0
WireConnection;17;2;19;0
WireConnection;0;2;17;0
WireConnection;0;9;19;0
ASEEND*/
//CHKSM=CA6B9C27017D96458428899D1F023A78ABC0B3FF