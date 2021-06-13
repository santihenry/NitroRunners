// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "electricity"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Vector0("Vector 0", Vector) = (0,5.73,0,0)
		_Float0("Float 0", Range( 0 , 1)) = 0
		_Float1("Float 1", Float) = 0
		_Vector1("Vector 1", Vector) = (0,-1,0,0)
		_Vector2("Vector 2", Vector) = (0,0,0,0)
		_Vector3("Vector 3", Vector) = (0,0,0,0)
		_Float2("Float 2", Float) = 0
		_Float4("Float 4", Float) = 0
		_Float3("Float 3", Float) = -5
		_Float5("Float 5", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float _Float5;
		uniform float _Float4;
		uniform float _Float3;
		uniform sampler2D _TextureSample0;
		uniform float _Float1;
		uniform float _Float0;
		uniform float2 _Vector1;
		uniform float2 _Vector2;
		uniform float2 _Vector0;
		uniform float2 _Vector3;
		uniform float _Float2;


		float2 voronoihash31( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi31( float2 v, float time, inout float2 id, float smoothness )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mr = 0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash31( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = g - f + o;
					float d = 0.5 * dot( r, r );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			return F2 - F1;
		}


		float2 voronoihash32( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi32( float2 v, float time, inout float2 id, float smoothness )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mr = 0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash32( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = g - f + o;
					float d = 0.5 * ( abs(r.x) + abs(r.y) );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			return F2 - F1;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 lerpResult74 = lerp( float3( 0,0,0 ) , ( ase_vertex3Pos * _Float5 ) , (0.0 + (sin( ( ( ase_vertex3Pos.y * _Float4 ) + ( _Time.y * _Float3 ) ) ) - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)));
			v.vertex.xyz += lerpResult74;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 color35 = IsGammaSpace() ? float4(0,13.80668,35.11095,0) : float4(0,322.2526,2511.735,0);
			float time31 = 4.71;
			float temp_output_4_0 = ( _Time.y * _Float0 );
			float2 uv_TexCoord7 = i.uv_texcoord * float2( 1,0.76 );
			float2 panner17 = ( temp_output_4_0 * _Vector1 + uv_TexCoord7);
			float2 coords31 = panner17 * _Float1;
			float2 id31 = 0;
			float fade31 = 0.5;
			float voroi31 = 0;
			float rest31 = 0;
			for( int it31 = 0; it31 <5; it31++ ){
			voroi31 += fade31 * voronoi31( coords31, time31, id31,0 );
			rest31 += fade31;
			coords31 *= 2;
			fade31 *= 0.5;
			}//Voronoi31
			voroi31 /= rest31;
			float time32 = 1.56;
			float2 panner5 = ( temp_output_4_0 * _Vector0 + uv_TexCoord7);
			float2 coords32 = panner5 * _Float1;
			float2 id32 = 0;
			float fade32 = 0.5;
			float voroi32 = 0;
			float rest32 = 0;
			for( int it32 = 0; it32 <8; it32++ ){
			voroi32 += fade32 * voronoi32( coords32, time32, id32,0 );
			rest32 += fade32;
			coords32 *= 2;
			fade32 *= 0.5;
			}//Voronoi32
			voroi32 /= rest32;
			float2 temp_cast_0 = ((_Vector3.x + (( (_Vector2.x + (voroi31 - 0.0) * (_Vector2.y - _Vector2.x) / (1.0 - 0.0)) + (_Vector2.x + (voroi32 - 0.0) * (_Vector2.y - _Vector2.x) / (1.0 - 0.0)) ) - 0.0) * (_Vector3.y - _Vector3.x) / (1.0 - 0.0))).xx;
			float4 temp_output_33_0 = (float4( 0,0,0,0 ) + (tex2D( _TextureSample0, temp_cast_0 ) - float4( 0,0,0,0 )) * (float4( 1,1,1,0 ) - float4( 0,0,0,0 )) / (float4( 0.3396226,0.3396226,0.3396226,0 ) - float4( 0,0,0,0 )));
			float4 lerpResult34 = lerp( float4( 0,0,0,0 ) , color35 , temp_output_33_0);
			o.Emission = lerpResult34.rgb;
			float2 temp_cast_2 = ((_Vector3.x + (( (_Vector2.x + (voroi31 - 0.0) * (_Vector2.y - _Vector2.x) / (1.0 - 0.0)) + (_Vector2.x + (voroi32 - 0.0) * (_Vector2.y - _Vector2.x) / (1.0 - 0.0)) ) - 0.0) * (_Vector3.y - _Vector3.x) / (1.0 - 0.0))).xx;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV55 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode55 = ( 0.0 + 1.18 * pow( 1.0 - fresnelNdotV55, 3.11 ) );
			float4 lerpResult57 = lerp( float4( 0,0,0,0 ) , temp_output_33_0 , pow( (1.0 + (fresnelNode55 - 0.0) * (0.0 - 1.0) / (1.0 - 0.0)) , _Float2 ));
			o.Alpha = saturate( lerpResult57 ).r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

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
				float3 worldNormal : TEXCOORD3;
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
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
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
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
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
153;367;930;502;1049.836;279.796;1;True;False
Node;AmplifyShaderEditor.SimpleTimeNode;2;-853.3173,136.0354;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-953.7229,258.9462;Inherit;False;Property;_Float0;Float 0;2;0;Create;True;0;0;False;0;0;0.125546;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;18;-652.7237,-239.607;Inherit;False;Property;_Vector1;Vector 1;4;0;Create;True;0;0;False;0;0,-1;2,-1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;6;-526.8894,262.706;Inherit;False;Property;_Vector0;Vector 0;1;0;Create;True;0;0;False;0;0,5.73;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-614.2309,136.2178;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-581.8343,-41.50882;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,0.76;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;17;-340.022,-296.7318;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-284.8398,320.343;Inherit;False;Property;_Float1;Float 1;3;0;Create;True;0;0;False;0;0;12.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;5;-333.7063,30.75381;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.VoronoiNode;32;20.24161,-58.86087;Inherit;True;0;2;2.03;2;8;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;1.56;False;2;FLOAT;1;False;3;FLOAT;0;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.VoronoiNode;31;-2.604108,-350.1267;Inherit;True;0;0;1;2;5;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;4.71;False;2;FLOAT;1;False;3;FLOAT;0;False;2;FLOAT;0;FLOAT;1
Node;AmplifyShaderEditor.Vector2Node;25;-3.165668,209.6328;Inherit;False;Property;_Vector2;Vector 2;5;0;Create;True;0;0;False;0;0,0;-4.9,35.81;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TFHCRemapNode;10;287.7083,52.6806;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-10;False;4;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;20;306.0798,-190.4486;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-10;False;4;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;65;2362.577,923.5212;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;72;2418.792,1089.778;Inherit;False;Property;_Float3;Float 3;9;0;Create;True;0;0;False;0;-5;-5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;62;2454.664,607.8461;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;30;746.7544,240.2093;Inherit;False;Property;_Vector3;Vector 3;6;0;Create;True;0;0;False;0;0,0;-2.13,-1.3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;15;569.2097,8.648727;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;73;2508.095,821.6381;Inherit;False;Property;_Float4;Float 4;8;0;Create;True;0;0;False;0;0;143.32;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;29;945.5753,14.79313;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;55;1732.157,283.59;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1.18;False;3;FLOAT;3.11;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;2795.921,765.252;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;9.81;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;2749.434,1008.529;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;60;2213.415,498.3206;Inherit;False;Property;_Float2;Float 2;7;0;Create;True;0;0;False;0;0;87.19;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;58;2137.634,258.4523;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;70;3088.176,914.2026;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;11;1254.622,1.810621;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;-1;None;d179c8744f837da49ab92aae04d1ae1c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinOpNode;64;3198.786,665.6745;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;33;1724.367,3.892382;Inherit;True;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0.3396226,0.3396226,0.3396226,0;False;3;COLOR;0,0,0,0;False;4;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;80;3085.664,535.9835;Inherit;False;Property;_Float5;Float 5;10;0;Create;True;0;0;False;0;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;59;2481.255,247.3002;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;22.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;78;3060.703,394.5368;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;57;2755.897,171.2446;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;3295.061,436.1388;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;81;3403.679,600.5037;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;35;1936.032,-172.6176;Inherit;False;Constant;_Color0;Color 0;6;1;[HDR];Create;True;0;0;False;0;0,13.80668,35.11095,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;61;2986.23,175.2618;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;34;2289.383,-104.7644;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;74;3518.333,428.649;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3795.02,124.5683;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;electricity;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;2;0
WireConnection;4;1;3;0
WireConnection;17;0;7;0
WireConnection;17;2;18;0
WireConnection;17;1;4;0
WireConnection;5;0;7;0
WireConnection;5;2;6;0
WireConnection;5;1;4;0
WireConnection;32;0;5;0
WireConnection;32;2;14;0
WireConnection;31;0;17;0
WireConnection;31;2;14;0
WireConnection;10;0;32;0
WireConnection;10;3;25;1
WireConnection;10;4;25;2
WireConnection;20;0;31;0
WireConnection;20;3;25;1
WireConnection;20;4;25;2
WireConnection;15;0;20;0
WireConnection;15;1;10;0
WireConnection;29;0;15;0
WireConnection;29;3;30;1
WireConnection;29;4;30;2
WireConnection;69;0;62;2
WireConnection;69;1;73;0
WireConnection;63;0;65;0
WireConnection;63;1;72;0
WireConnection;58;0;55;0
WireConnection;70;0;69;0
WireConnection;70;1;63;0
WireConnection;11;1;29;0
WireConnection;64;0;70;0
WireConnection;33;0;11;0
WireConnection;59;0;58;0
WireConnection;59;1;60;0
WireConnection;57;1;33;0
WireConnection;57;2;59;0
WireConnection;79;0;78;0
WireConnection;79;1;80;0
WireConnection;81;0;64;0
WireConnection;61;0;57;0
WireConnection;34;1;35;0
WireConnection;34;2;33;0
WireConnection;74;1;79;0
WireConnection;74;2;81;0
WireConnection;0;2;34;0
WireConnection;0;9;61;0
WireConnection;0;11;74;0
ASEEND*/
//CHKSM=C1734ED66F9FA918BD5C7C9DFBCCA4BFBE5C835A