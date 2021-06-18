// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SpecialUI"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_TextureSample3("Texture Sample 3", 2D) = "white" {}
		_TextureSample4("Texture Sample 4", 2D) = "white" {}
		[HDR]_Color0("Color 0", Color) = (0,0,0,0)
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_Float2("Float 2", Float) = 0
		_Vector0("Vector 0", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
		
		Stencil
		{
			Ref [_Stencil]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
			CompFront [_StencilComp]
			PassFront [_StencilOp]
			FailFront Keep
			ZFailFront Keep
			CompBack Always
			PassBack Keep
			FailBack Keep
			ZFailBack Keep
		}


		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		
		Pass
		{
			Name "Default"
		CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_CLIP_RECT
			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			
			#include "UnityShaderVariables.cginc"

			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform fixed4 _TextureSampleAdd;
			uniform float4 _ClipRect;
			uniform sampler2D _MainTex;
			uniform sampler2D _TextureSample1;
			uniform float4 _TextureSample1_ST;
			uniform sampler2D _specialTex;
			uniform float4 _specialTex_ST;
			uniform sampler2D _TextureSample2;
			uniform float _Float2;
			uniform float2 _Vector0;
			uniform float _specialAmount;
			uniform sampler2D _TextureSample3;
			uniform float4 _Color0;
			uniform sampler2D _TextureSample4;
			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID( IN );
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				OUT.worldPosition = IN.vertex;
				
				
				OUT.worldPosition.xyz +=  float3( 0, 0, 0 ) ;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN  ) : SV_Target
			{
				float2 uv_TextureSample1 = IN.texcoord.xy * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
				float4 tex2DNode2 = tex2D( _TextureSample1, uv_TextureSample1 );
				float2 uv_specialTex = IN.texcoord.xy * _specialTex_ST.xy + _specialTex_ST.zw;
				float4 tex2DNode1 = tex2D( _specialTex, uv_specialTex );
				float4 lerpResult4 = lerp( tex2DNode2 , tex2DNode1 , tex2DNode1.a);
				float2 uv0115 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float mulTime118 = _Time.y * 0.5;
				float2 panner116 = ( (1.0 + (sin( mulTime118 ) - -1.0) * (2.0 - 1.0) / (1.0 - -1.0)) * float2( 0.01,0.01 ) + uv0115);
				float2 uv079 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner80 = ( 1.0 * _Time.y * float2( 0,-1 ) + uv079);
				float simplePerlin2D94 = snoise( panner80*_Float2 );
				simplePerlin2D94 = simplePerlin2D94*0.5 + 0.5;
				float2 lerpResult119 = lerp( uv0115 , panner116 , saturate( (_Vector0.x + (simplePerlin2D94 - 0.0) * (_Vector0.y - _Vector0.x) / (1.0 - 0.0)) ));
				float cos88 = cos( 1.0 * _Time.y );
				float sin88 = sin( 1.0 * _Time.y );
				float2 rotator88 = mul( saturate( lerpResult119 ) - float2( 0.5,0.5 ) , float2x2( cos88 , -sin88 , sin88 , cos88 )) + float2( 0.5,0.5 );
				float4 tex2DNode74 = tex2D( _TextureSample2, rotator88 );
				float4 lerpResult100 = lerp( tex2DNode2 , tex2DNode74 , tex2DNode74.a);
				float4 lerpResult75 = lerp( tex2DNode2 , lerpResult100 , tex2DNode74.a);
				float4 lerpResult77 = lerp( lerpResult75 , tex2DNode1 , tex2DNode1.a);
				float4 lerpResult76 = lerp( lerpResult4 , lerpResult77 , saturate( (0.0 + (_specialAmount - 0.99) * (1.0 - 0.0) / (1.0 - 0.99)) ));
				float4 appendResult11 = (float4(0.0 , (0.0 + (_specialAmount - 0.0) * (-1.0 - 0.0) / (1.0 - 0.0)) , 0.0 , 0.0));
				float2 uv08 = IN.texcoord.xy * float2( 1,1 ) + appendResult11.xy;
				float mulTime68 = _Time.y * -0.6;
				float4 appendResult64 = (float4(mulTime68 , 1.13 , 0.0 , 0.0));
				float2 uv026 = IN.texcoord.xy * float2( 1,1 ) + ( (0.0 + (_specialAmount - 0.0) * (-1.0 - 0.0) / (1.0 - 0.0)) + appendResult64 ).xy;
				float4 appendResult46 = (float4(0.0 , _specialAmount , 0.0 , 0.0));
				float2 uv039 = IN.texcoord.xy * float2( 1,1 ) + appendResult46.xy;
				float4 lerpResult47 = lerp( float4( 1,1,1,0 ) , (float4( 1,1,1,0 ) + (tex2D( _TextureSample3, uv026 ) - float4( 0,0,0,0 )) * (float4( 0,0,0,0 ) - float4( 1,1,1,0 )) / (float4( 1,1,1,0 ) - float4( 0,0,0,0 ))) , saturate( ( (0.0 + (_specialAmount - 0.1) * (1.0 - 0.0) / (0.4 - 0.1)) + ceil( ( ( 1.0 - uv039.y ) + -0.2 ) ) ) ));
				float4 lerpResult5 = lerp( lerpResult76 , ( lerpResult4 * float4( 0.1509434,0.1509434,0.1509434,1 ) ) , saturate( ( ceil( ( uv08.y * 1.0 ) ) * lerpResult47 ) ));
				float4 appendResult67 = (float4(mulTime68 , 0.123 , 0.0 , 0.0));
				float2 uv033 = IN.texcoord.xy * float2( 1,1 ) + ( (0.0 + (_specialAmount - 0.0) * (-1.0 - 0.0) / (1.0 - 0.0)) + appendResult67 ).xy;
				float4 tex2DNode30 = tex2D( _TextureSample4, uv033 );
				float4 lerpResult55 = lerp( float4( 0,0,0,0 ) , _Color0 , tex2DNode30.a);
				float4 lerpResult32 = lerp( float4( 0,0,0,0 ) , tex2DNode30 , saturate( pow( ( tex2DNode1.a + tex2DNode2.b ) , 0.1 ) ));
				float4 lerpResult31 = lerp( lerpResult5 , lerpResult55 , lerpResult32);
				
				half4 color = lerpResult31;
				
				#ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17800
1920;36;1920;983;6589.737;1538.069;5.52498;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;79;-4924,129.5758;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;118;-3960.141,-286.4094;Inherit;False;1;0;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;80;-4668,113.5758;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;85;-5079.809,606.2953;Inherit;False;Property;_Float2;Float 2;6;0;Create;True;0;0;False;0;0;5.47;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;117;-3711.813,-273.7104;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-2486.69,639.2589;Inherit;False;Global;_specialAmount;_specialAmount;1;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;94;-4300,81.57579;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;103;-4661.025,543.1133;Inherit;False;Property;_Vector0;Vector 0;7;0;Create;True;0;0;False;0;0,0;-1.1,1.57;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DynamicAppendNode;46;-2330.679,769.0909;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TFHCRemapNode;121;-3508.315,-253.5119;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;115;-3610.039,-447.0549;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;101;-3948,48.03002;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-2;False;4;FLOAT;1.86;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-1285.482,966.8492;Inherit;False;Constant;_Float0;Float 0;8;0;Create;True;0;0;False;0;1.13;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;68;-1396.272,883.9165;Inherit;False;1;0;FLOAT;-0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;116;-3263.205,-288.4395;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0.01,0.01;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;102;-3174.679,-42.92192;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;39;-2189.972,1124.796;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;42;-1954.308,1281.402;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;29;-1253.875,577.7185;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;64;-1024.941,853.6878;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;119;-2816.799,-261.4934;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;41;-1676.678,1280.345;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;-0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;16;-1256.143,328.5066;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;120;-2608.527,-344.6891;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-860.826,836.7756;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CeilOpNode;40;-1374.792,1316.703;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;48;-1854.114,837.4769;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0.1;False;2;FLOAT;0.4;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;88;-2394.624,-389.8753;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;26;-724.5219,768.8346;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;11;-899.6533,329.3945;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;74;-2123.624,-508.6241;Inherit;True;Property;_TextureSample2;Texture Sample 2;5;0;Create;True;0;0;False;0;-1;None;1067199a75e33b34692db529a3b7bd2e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-1036.388,54.23504;Inherit;True;Property;_TextureSample1;Texture Sample 1;0;0;Create;True;0;0;False;0;-1;None;25599fa04a0a66446bf188b3d358f2e9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-662.1974,281.1997;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;51;-1127.333,1271.27;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-767.7429,1498.091;Inherit;False;Constant;_Float1;Float 1;7;0;Create;True;0;0;False;0;0.123;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;19;-473.711,773.8201;Inherit;True;Property;_TextureSample3;Texture Sample 3;2;0;Create;True;0;0;False;0;-1;None;887f2af9340cd2844bdfe7cdbb1dfef1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;50;-822.1354,1265.928;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;100;-1254.239,-523.3123;Inherit;False;3;0;COLOR;0.5283019,0.5283019,0.5283019,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-732.2701,-266.0917;Inherit;True;Global;_specialTex;_specialTex;1;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;36;-638.6879,1066.807;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-358.4056,327.8355;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;67;-373.0597,1341.937;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TFHCRemapNode;22;-150.8679,799.7794;Inherit;True;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;3;COLOR;1,1,1,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CeilOpNode;10;-91.06534,326.9051;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;71;73.22729,110.7895;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;47;58.48209,551.4114;Inherit;False;3;0;COLOR;1,1,1,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;35;-216.8419,1120.631;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;75;-1027.62,-487.0884;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;60;-1743.075,256.5558;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0.99;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;33;72.37146,1094.585;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;61;-1269.481,82.05042;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;77;-214.6974,-317.7589;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;73;216.8901,113.2715;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;4;-209.8483,-567.0405;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;246.9783,330.8388;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;72;644.8555,250.6477;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;338.6139,-50.2551;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.1509434,0.1509434,0.1509434,1;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;56;931.5882,512.2552;Inherit;False;Property;_Color0;Color 0;4;1;[HDR];Create;True;0;0;False;0;0,0,0,0;4.594794,1.243297,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;76;299.0251,-296.7657;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;23;442.2703,250.3525;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;30;602.2487,773.6473;Inherit;True;Property;_TextureSample4;Texture Sample 4;3;0;Create;True;0;0;False;0;-1;None;11eabf0ded1de6b42b1bb6d4582c24e4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;5;792.5045,-52.20653;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;32;875.754,245.2043;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;55;1314.265,471.2261;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;31;1642.287,-19.39456;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;89;-3070.763,-559.0051;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;2281.512,-33.3707;Float;False;True;-1;2;ASEMaterialInspector;0;4;SpecialUI;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;True;2;False;-1;True;True;True;True;True;0;True;-9;True;True;0;True;-5;255;True;-8;255;True;-7;0;True;-4;0;True;-6;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;0;True;-11;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;0
WireConnection;80;0;79;0
WireConnection;117;0;118;0
WireConnection;94;0;80;0
WireConnection;94;1;85;0
WireConnection;46;1;12;0
WireConnection;121;0;117;0
WireConnection;101;0;94;0
WireConnection;101;3;103;1
WireConnection;101;4;103;2
WireConnection;116;0;115;0
WireConnection;116;1;121;0
WireConnection;102;0;101;0
WireConnection;39;1;46;0
WireConnection;42;0;39;2
WireConnection;29;0;12;0
WireConnection;64;0;68;0
WireConnection;64;1;65;0
WireConnection;119;0;115;0
WireConnection;119;1;116;0
WireConnection;119;2;102;0
WireConnection;41;0;42;0
WireConnection;16;0;12;0
WireConnection;120;0;119;0
WireConnection;28;0;29;0
WireConnection;28;1;64;0
WireConnection;40;0;41;0
WireConnection;48;0;12;0
WireConnection;88;0;120;0
WireConnection;26;1;28;0
WireConnection;11;1;16;0
WireConnection;74;1;88;0
WireConnection;8;1;11;0
WireConnection;51;0;48;0
WireConnection;51;1;40;0
WireConnection;19;1;26;0
WireConnection;50;0;51;0
WireConnection;100;0;2;0
WireConnection;100;1;74;0
WireConnection;100;2;74;4
WireConnection;36;0;12;0
WireConnection;9;0;8;2
WireConnection;67;0;68;0
WireConnection;67;1;66;0
WireConnection;22;0;19;0
WireConnection;10;0;9;0
WireConnection;71;0;1;4
WireConnection;71;1;2;3
WireConnection;47;1;22;0
WireConnection;47;2;50;0
WireConnection;35;0;36;0
WireConnection;35;1;67;0
WireConnection;75;0;2;0
WireConnection;75;1;100;0
WireConnection;75;2;74;4
WireConnection;60;0;12;0
WireConnection;33;1;35;0
WireConnection;61;0;60;0
WireConnection;77;0;75;0
WireConnection;77;1;1;0
WireConnection;77;2;1;4
WireConnection;73;0;71;0
WireConnection;4;0;2;0
WireConnection;4;1;1;0
WireConnection;4;2;1;4
WireConnection;24;0;10;0
WireConnection;24;1;47;0
WireConnection;72;0;73;0
WireConnection;17;0;4;0
WireConnection;76;0;4;0
WireConnection;76;1;77;0
WireConnection;76;2;61;0
WireConnection;23;0;24;0
WireConnection;30;1;33;0
WireConnection;5;0;76;0
WireConnection;5;1;17;0
WireConnection;5;2;23;0
WireConnection;32;1;30;0
WireConnection;32;2;72;0
WireConnection;55;1;56;0
WireConnection;55;2;30;4
WireConnection;31;0;5;0
WireConnection;31;1;55;0
WireConnection;31;2;32;0
WireConnection;0;0;31;0
ASEEND*/
//CHKSM=EE7C90B0B1F7C5200F018CF212AF9B1B6E9A2F6D