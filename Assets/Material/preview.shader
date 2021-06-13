// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "preview"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Float0("Float 0", Float) = 0
		_Float1("Float 0", Float) = 0
		_Float2("Float 2", Range( 0 , 1)) = 0
		_Color0("Color 0", Color) = (0.05076815,0.8867924,0,0)
		_Float3("Float 3", Float) = 0
		_Float4("Float 3", Float) = 0
		_Float5("Float 3", Float) = 0
		_Float6("Float 3", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color0;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _Float5;
		uniform float _Float6;
		uniform float _Float3;
		uniform float _Float4;
		uniform float _Float0;
		uniform float _Float1;
		uniform float _Float2;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Emission = _Color0.rgb;
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 tex2DNode34 = tex2D( _TextureSample0, uv_TextureSample0 );
			float lerpResult40 = lerp( 0.0 , (_Float3 + (tex2DNode34.r - 0.0) * (_Float4 - _Float3) / (1.0 - 0.0)) , saturate( (_Float0 + (tex2DNode34.r - 0.0) * (_Float1 - _Float0) / (1.0 - 0.0)) ));
			o.Alpha = saturate( ( ( saturate( (_Float5 + (tex2DNode34.r - 0.0) * (_Float6 - _Float5) / (1.0 - 0.0)) ) + saturate( lerpResult40 ) ) * _Float2 ) );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
121;241;930;466;-726.3618;665.6646;1.6;True;False
Node;AmplifyShaderEditor.RangedFloatNode;39;-420.188,-70.31353;Inherit;False;Property;_Float1;Float 0;2;0;Create;True;0;0;False;0;0;38.34;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-425.9631,-168.4908;Inherit;False;Property;_Float0;Float 0;1;0;Create;True;0;0;False;0;0;-0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;34;-675.7265,-641.5504;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;-1;None;0000000000000000f000000000000000;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;54;-192.5034,-580.3193;Inherit;False;Property;_Float3;Float 3;5;0;Create;True;0;0;False;0;0;2.12;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-153.1391,-501.5905;Inherit;False;Property;_Float4;Float 3;6;0;Create;True;0;0;False;0;0;-15.72;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;35;-223.8046,-401.3173;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.08;False;4;FLOAT;1.52;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;43;28.08279,-724.8152;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;-1.65;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;36;145.3013,-403.619;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-157.1451,-807.882;Inherit;False;Property;_Float6;Float 3;8;0;Create;True;0;0;False;0;0;3.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-145.8879,-912.9489;Inherit;False;Property;_Float5;Float 3;7;0;Create;True;0;0;False;0;0;-10.36;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;40;434.2654,-387.7214;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;49;50.47541,-1024.304;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-10.66;False;4;FLOAT;1.81;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;53;597.331,-770.3334;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;59;722.6631,-371.4038;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;1056.712,-97.6438;Inherit;False;Property;_Float2;Float 2;3;0;Create;True;0;0;False;0;0;0.03;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;52;929.7772,-389.5588;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;1394.696,-245.1176;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;46;1249.346,-587.584;Inherit;False;Property;_Color0;Color 0;4;0;Create;True;0;0;False;0;0.05076815,0.8867924,0,0;0.8773585,0.8773585,0.8773585,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;44;1615.991,-297.3073;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1884.19,-487.7574;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;preview;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;35;0;34;1
WireConnection;35;3;37;0
WireConnection;35;4;39;0
WireConnection;43;0;34;1
WireConnection;43;3;54;0
WireConnection;43;4;56;0
WireConnection;36;0;35;0
WireConnection;40;1;43;0
WireConnection;40;2;36;0
WireConnection;49;0;34;1
WireConnection;49;3;57;0
WireConnection;49;4;58;0
WireConnection;53;0;49;0
WireConnection;59;0;40;0
WireConnection;52;0;53;0
WireConnection;52;1;59;0
WireConnection;47;0;52;0
WireConnection;47;1;48;0
WireConnection;44;0;47;0
WireConnection;0;2;46;0
WireConnection;0;9;44;0
ASEEND*/
//CHKSM=C82DEC639CB5F6CA2EE08F6868CB46FAD3096454