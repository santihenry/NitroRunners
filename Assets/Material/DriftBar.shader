// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DriftBar"
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
		_Float6("Float 6", Float) = 0
		_Float7("Float 6", Float) = 0
		[HDR]_Color2("Color 0", Color) = (2.79544,1.448945,0,1)
		_driftBar("_driftBar", Range( 0 , 1)) = 0
		_driftframe2("drift frame 2", 2D) = "white" {}
		_fillobject2("fill object 2", 2D) = "white" {}
		_fillobject2inverse("fill object 2 inverse", 2D) = "white" {}
		_fire2inverse("fire 2 inverse", 2D) = "white" {}
		_fire2("fire 2", 2D) = "white" {}
		_Float2("Float 2", Float) = 0
		_Float5("Float 5", Float) = -0.19
		_Float4("Float 4", Float) = 0
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
			uniform float _Float6;
			uniform sampler2D _fire2inverse;
			uniform float4 _fire2inverse_ST;
			uniform float _Float7;
			uniform sampler2D _fire2;
			uniform float4 _fire2_ST;
			uniform float _driftBar;
			uniform sampler2D _fillobject2;
			uniform float4 _fillobject2_ST;
			uniform float _Float2;
			uniform float _Float5;
			uniform float _Float4;
			uniform sampler2D _fillobject2inverse;
			uniform float4 _fillobject2inverse_ST;
			uniform sampler2D _driftframe2;
			uniform float4 _driftframe2_ST;
			uniform float4 _Color2;
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
			
			struct Gradient
			{
				int type;
				int colorsLength;
				int alphasLength;
				float4 colors[8];
				float2 alphas[8];
			};
			
			Gradient NewGradient(int type, int colorsLength, int alphasLength, 
			float4 colors0, float4 colors1, float4 colors2, float4 colors3, float4 colors4, float4 colors5, float4 colors6, float4 colors7,
			float2 alphas0, float2 alphas1, float2 alphas2, float2 alphas3, float2 alphas4, float2 alphas5, float2 alphas6, float2 alphas7)
			{
				Gradient g;
				g.type = type;
				g.colorsLength = colorsLength;
				g.alphasLength = alphasLength;
				g.colors[ 0 ] = colors0;
				g.colors[ 1 ] = colors1;
				g.colors[ 2 ] = colors2;
				g.colors[ 3 ] = colors3;
				g.colors[ 4 ] = colors4;
				g.colors[ 5 ] = colors5;
				g.colors[ 6 ] = colors6;
				g.colors[ 7 ] = colors7;
				g.alphas[ 0 ] = alphas0;
				g.alphas[ 1 ] = alphas1;
				g.alphas[ 2 ] = alphas2;
				g.alphas[ 3 ] = alphas3;
				g.alphas[ 4 ] = alphas4;
				g.alphas[ 5 ] = alphas5;
				g.alphas[ 6 ] = alphas6;
				g.alphas[ 7 ] = alphas7;
				return g;
			}
			
			float4 SampleGradient( Gradient gradient, float time )
			{
				float3 color = gradient.colors[0].rgb;
				UNITY_UNROLL
				for (int c = 1; c < 8; c++)
				{
				float colorPos = saturate((time - gradient.colors[c-1].w) / (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, (float)gradient.colorsLength-1);
				color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
				}
				#ifndef UNITY_COLORSPACE_GAMMA
				color = half3(GammaToLinearSpaceExact(color.r), GammaToLinearSpaceExact(color.g), GammaToLinearSpaceExact(color.b));
				#endif
				float alpha = gradient.alphas[0].x;
				UNITY_UNROLL
				for (int a = 1; a < 8; a++)
				{
				float alphaPos = saturate((time - gradient.alphas[a-1].y) / (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, (float)gradient.alphasLength-1);
				alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
				}
				return float4(color, alpha);
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
				float2 uv07 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner128 = ( -1.0 * _Time.y * float2( 1,1 ) + uv07);
				float simplePerlin2D127 = snoise( panner128*_Float6 );
				simplePerlin2D127 = simplePerlin2D127*0.5 + 0.5;
				float2 uv_fire2inverse = IN.texcoord.xy * _fire2inverse_ST.xy + _fire2inverse_ST.zw;
				float4 tex2DNode106 = tex2D( _fire2inverse, uv_fire2inverse );
				float lerpResult129 = lerp( simplePerlin2D127 , 1.0 , tex2DNode106.r);
				float2 panner132 = ( -1.0 * _Time.y * float2( -1,1 ) + uv07);
				float simplePerlin2D134 = snoise( panner132*_Float7 );
				simplePerlin2D134 = simplePerlin2D134*0.5 + 0.5;
				float2 uv_fire2 = IN.texcoord.xy * _fire2_ST.xy + _fire2_ST.zw;
				float4 tex2DNode107 = tex2D( _fire2, uv_fire2 );
				float lerpResult135 = lerp( simplePerlin2D134 , 1.0 , tex2DNode107.r);
				float clampResult142 = clamp( (0.2 + (_driftBar - 0.7) * (2.0 - 0.2) / (1.0 - 0.7)) , 0.0 , 1.0 );
				Gradient gradient31 = NewGradient( 0, 2, 2, float4( 0.1921999, 0.589, 0, 0 ), float4( 0.1921999, 0.589, 0, 1 ), 0, 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
				Gradient gradient34 = NewGradient( 0, 3, 2, float4( 1, 0.5568628, 0, 0 ), float4( 1, 0.1487731, 0, 0.5000076 ), float4( 1, 0.5563877, 0, 1 ), 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
				float4 lerpResult38 = lerp( SampleGradient( gradient31, uv07.x ) , SampleGradient( gradient34, uv07.x ) , saturate( (0.0 + (_driftBar - 0.7) * (1.0 - 0.0) / (0.71 - 0.7)) ));
				float2 uv_fillobject2 = IN.texcoord.xy * _fillobject2_ST.xy + _fillobject2_ST.zw;
				float lerpResult112 = lerp( 0.0 , tex2D( _fillobject2, uv_fillobject2 ).a , saturate( ceil( ( uv07.x + (-1.0 + (_driftBar - 0.0) * (_Float2 - -1.0) / (1.0 - 0.0)) ) ) ));
				float2 uv_fillobject2inverse = IN.texcoord.xy * _fillobject2inverse_ST.xy + _fillobject2inverse_ST.zw;
				float lerpResult115 = lerp( 0.0 , saturate( ceil( ( ( uv07.x + (_Float5 + (_driftBar - 0.0) * (_Float4 - _Float5) / (1.0 - 0.0)) ) * -1.0 ) ) ) , tex2D( _fillobject2inverse, uv_fillobject2inverse ).a);
				float4 lerpResult25 = lerp( float4( 0,0,0,0 ) , lerpResult38 , ( lerpResult112 + lerpResult115 ));
				float2 uv_driftframe2 = IN.texcoord.xy * _driftframe2_ST.xy + _driftframe2_ST.zw;
				float4 tex2DNode86 = tex2D( _driftframe2, uv_driftframe2 );
				float2 panner146 = ( 0.6 * _Time.y * float2( 0,-1 ) + uv07);
				float simplePerlin2D145 = snoise( panner146*16.7 );
				simplePerlin2D145 = simplePerlin2D145*0.5 + 0.5;
				float4 lerpResult148 = lerp( tex2DNode86 , _Color2 , ( saturate( ( pow( simplePerlin2D145 , 1.5 ) * tex2DNode86.a ) ) * saturate( (0.0 + (saturate( (0.0 + (_driftBar - 0.9) * (1.0 - 0.0) / (1.0 - 0.9)) ) - 0.0) * (1.0 - 0.0) / (0.3 - 0.0)) ) ));
				float4 lerpResult89 = lerp( ( ( ( ( lerpResult129 * tex2DNode106.a ) + ( lerpResult135 * tex2DNode107.a ) ) * clampResult142 ) + lerpResult25 ) , lerpResult148 , tex2DNode86.a);
				
				half4 color = lerpResult89;
				
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
70;391;930;276;5636.579;-1043.713;1;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-5846.285,1832.635;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;122;-4407.349,2170.488;Inherit;False;Property;_Float5;Float 5;17;0;Create;True;0;0;False;0;-0.19;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;121;-4384.349,2253.488;Inherit;False;Property;_Float4;Float 4;18;0;Create;True;0;0;False;0;0;-0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-5132.046,1098.502;Inherit;False;Property;_driftBar;_driftBar;9;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;113;-3814.448,1531.026;Inherit;False;Constant;_Float1;Float 1;13;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;-3791.448,1614.026;Inherit;False;Property;_Float2;Float 2;16;0;Create;True;0;0;False;0;0;-0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;119;-3960.92,2074.156;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;120;-4094.839,2149.759;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;116;-3687.229,2135.371;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;108;-3607.02,1475.734;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;-2;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;147;190.8579,2012.958;Inherit;False;Constant;_Vector0;Vector 0;18;0;Create;True;0;0;False;0;0,-1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;133;-1456.928,2906.081;Inherit;False;Property;_Float7;Float 6;5;0;Create;True;0;0;False;0;0;8.73;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;146;424.8582,1848.794;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;0.6;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;126;-1381.493,2429.036;Inherit;False;Property;_Float6;Float 6;4;0;Create;True;0;0;False;0;0;8.73;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;124;-3418.906,2132.91;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;154;538.1936,2311.934;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;128;-1573.354,2572.124;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;-1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;132;-1648.789,3049.169;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-1,1;False;1;FLOAT;-1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;109;-3209.515,1442.027;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;145;589.1109,1830.562;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;16.7;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;134;-1150.667,2892.311;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;8.31;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;106;-2302.275,1999.95;Inherit;True;Property;_fire2inverse;fire 2 inverse;14;0;Create;True;0;0;False;0;-1;12994b4beb95d21499903b57c69b4abf;12994b4beb95d21499903b57c69b4abf;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CeilOpNode;117;-3219.08,2086.461;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;107;-2292.355,1777.829;Inherit;True;Property;_fire2;fire 2;15;0;Create;True;0;0;False;0;-1;250394c8fb5267f4a883aa40abfd4079;250394c8fb5267f4a883aa40abfd4079;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;155;763.186,2266.856;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0.9;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;127;-1075.232,2415.266;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;8.31;False;1;FLOAT;0
Node;AmplifyShaderEditor.CeilOpNode;110;-2824.354,1428.387;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;105;-3323.267,1777.295;Inherit;True;Property;_fillobject2inverse;fill object 2 inverse;13;0;Create;True;0;0;False;0;-1;edcba67041465484e91b96ac6908aa7f;edcba67041465484e91b96ac6908aa7f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;118;-2959.435,2087.96;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;111;-2564.709,1429.886;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;135;-851.3537,2803.011;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;159;963.6863,2273.474;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;87;-2711.861,1157.715;Inherit;True;Property;_fillobject2;fill object 2;12;0;Create;True;0;0;False;0;-1;c637a2f6b85870b4c98f0e5353282041;c637a2f6b85870b4c98f0e5353282041;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;86;-1811.091,865.5216;Inherit;True;Property;_driftframe2;drift frame 2;11;0;Create;True;0;0;False;0;-1;472e1fcf85efa514c8048ba663204a69;472e1fcf85efa514c8048ba663204a69;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;152;853.8742,1840.761;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;67;-3794.623,3204.544;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0.7;False;2;FLOAT;0.71;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientNode;34;-4707.133,2919.191;Inherit;False;0;3;2;1,0.5568628,0,0;1,0.1487731,0,0.5000076;1,0.5563877,0,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.LerpOp;129;-799.2781,2314.611;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientNode;31;-4703.262,2809.957;Inherit;False;0;2;2;0.1921999,0.589,0,0;0.1921999,0.589,0,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.GradientSampleNode;37;-4110.896,2981.913;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;130;-404.3326,2150.799;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;112;-2060.954,1224.311;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;68;-3542.674,3165.306;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;149;1183.55,1811.339;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;115;-2718.132,1773.902;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;136;-521.9702,2672.596;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientSampleNode;33;-4143.067,2738.595;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;141;-1242.114,1768.334;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0.7;False;2;FLOAT;1;False;3;FLOAT;0.2;False;4;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;158;1153.7,2223.083;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.3;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;38;-2900.757,2863.605;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;150;1441.071,1821.453;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;137;104.3842,2433.808;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;98;-1767.727,1395.811;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;157;1395.259,2229.423;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;142;-955.5237,1798.992;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;151;955.2111,1642.454;Inherit;False;Property;_Color2;Color 0;7;1;[HDR];Create;True;0;0;False;0;2.79544,1.448945,0,1;2.79544,0.649087,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;153;1705.253,2129.936;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;138;-699.0445,1755.701;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;25;-1142.068,1366.181;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;139;-289.1394,1428.509;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;144;-8142.518,-2399.506;Inherit;False;7214.34;2105.976;Comment;38;29;47;52;1;6;50;66;40;70;58;12;63;44;43;3;14;2;49;75;64;9;28;11;73;80;74;53;8;15;51;46;13;54;71;41;39;65;48;viejo;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;148;1780.776,1738.491;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;66;-3413.815,-2219.949;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;52;-2927.492,-766.134;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;54;-4696.791,-1385.94;Inherit;False;Property;_Color0;Color 0;8;1;[HDR];Create;True;0;0;False;0;2.79544,1.448945,0,1;2.79544,0.649087,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;65;-3686.655,-2182.228;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0.9;False;2;FLOAT;0.91;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-7302.103,-2349.506;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;-1;None;73b486accb070cb458174ff9d56dc555;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;89;1923.782,1262.27;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CeilOpNode;44;-5093.725,-1381.4;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;58;-3166.739,-2310.96;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;80;-3127.262,-819.2153;Inherit;False;Constant;_Float3;Float 3;8;0;Create;True;0;0;False;0;0.26;0.26;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;64;-1193.178,-827.9171;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;40;-5519.605,-1286.322;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-4128.573,-1504.866;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;50;-3903.719,-1876.464;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-4992.54,-1997.018;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0.72;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;12;-5163.621,-1972.609;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;70;-5582.706,-1516.235;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0.7;False;2;FLOAT;0.71;False;3;FLOAT;0;False;4;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;15;-5940.229,-1542.058;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;71;-5157.619,-1800.193;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;6;-6260.567,-1424.794;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;8.31;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;14;-6486.682,-1779.285;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;8;-6674.469,-1316.286;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,0;False;1;FLOAT;-1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;51;-3215.714,-728.1771;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.5;False;4;FLOAT;-0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-6482.608,-1459.374;Inherit;False;Property;_Float0;Float 0;3;0;Create;True;0;0;False;0;0;8.73;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;63;-1783.245,-547.5308;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;74;-4392.591,-1543.999;Inherit;False;Property;_Color1;Color 1;10;0;Create;True;0;0;False;0;0.1320755,0.1320755,0.1320755,1;0.162,0.162,0.162,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-5696.965,-1811.375;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;48;-3475.569,-1995.247;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;9;-6161.523,-1900.968;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;39;-2902.842,-2133.012;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;73;-5350.866,-1670.39;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;41;-4302.647,-1979.574;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;49;-2762.21,-871.2902;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;47;-2324.567,-982.1526;Inherit;True;Property;_TextureSample3;Texture Sample 3;6;0;Create;True;0;0;False;0;-1;None;ad3d7e4a0c2de2b489cd61cfef267b21;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;43;-4560.773,-1619.927;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;46;-3782.266,-1048.737;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-4636.994,-1935.396;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-7273.192,-2106.291;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;-1;None;48eddd0bfdb9a1344b2854be2b28caab;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-7188.974,-1866.86;Inherit;True;Property;_TextureSample2;Texture Sample 2;2;0;Create;True;0;0;False;0;-1;None;1bdbb7800d0ea5d44a07558de8189f2d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;75;-8092.518,-1948.902;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;3222.482,1362.646;Float;False;True;-1;2;ASEMaterialInspector;0;4;DriftBar;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;True;2;False;-1;True;True;True;True;True;0;True;-9;True;True;0;True;-5;255;True;-8;255;True;-7;0;True;-4;0;True;-6;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;0;True;-11;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;0
WireConnection;119;0;7;1
WireConnection;120;0;45;0
WireConnection;120;3;122;0
WireConnection;120;4;121;0
WireConnection;116;0;119;0
WireConnection;116;1;120;0
WireConnection;108;0;45;0
WireConnection;108;3;113;0
WireConnection;108;4;114;0
WireConnection;146;0;7;0
WireConnection;146;2;147;0
WireConnection;124;0;116;0
WireConnection;154;0;45;0
WireConnection;128;0;7;0
WireConnection;132;0;7;0
WireConnection;109;0;7;1
WireConnection;109;1;108;0
WireConnection;145;0;146;0
WireConnection;134;0;132;0
WireConnection;134;1;133;0
WireConnection;117;0;124;0
WireConnection;155;0;154;0
WireConnection;127;0;128;0
WireConnection;127;1;126;0
WireConnection;110;0;109;0
WireConnection;118;0;117;0
WireConnection;111;0;110;0
WireConnection;135;0;134;0
WireConnection;135;2;107;0
WireConnection;159;0;155;0
WireConnection;152;0;145;0
WireConnection;67;0;45;0
WireConnection;129;0;127;0
WireConnection;129;2;106;0
WireConnection;37;0;34;0
WireConnection;37;1;7;1
WireConnection;130;0;129;0
WireConnection;130;1;106;4
WireConnection;112;1;87;4
WireConnection;112;2;111;0
WireConnection;68;0;67;0
WireConnection;149;0;152;0
WireConnection;149;1;86;4
WireConnection;115;1;118;0
WireConnection;115;2;105;4
WireConnection;136;0;135;0
WireConnection;136;1;107;4
WireConnection;33;0;31;0
WireConnection;33;1;7;1
WireConnection;141;0;45;0
WireConnection;158;0;159;0
WireConnection;38;0;33;0
WireConnection;38;1;37;0
WireConnection;38;2;68;0
WireConnection;150;0;149;0
WireConnection;137;0;130;0
WireConnection;137;1;136;0
WireConnection;98;0;112;0
WireConnection;98;1;115;0
WireConnection;157;0;158;0
WireConnection;142;0;141;0
WireConnection;153;0;150;0
WireConnection;153;1;157;0
WireConnection;138;0;137;0
WireConnection;138;1;142;0
WireConnection;25;1;38;0
WireConnection;25;2;98;0
WireConnection;139;0;138;0
WireConnection;139;1;25;0
WireConnection;148;0;86;0
WireConnection;148;1;151;0
WireConnection;148;2;153;0
WireConnection;66;0;65;0
WireConnection;52;0;51;0
WireConnection;52;1;80;0
WireConnection;65;0;45;0
WireConnection;1;1;75;0
WireConnection;89;0;139;0
WireConnection;89;1;148;0
WireConnection;89;2;86;4
WireConnection;44;0;40;0
WireConnection;58;0;1;0
WireConnection;58;1;64;0
WireConnection;58;2;66;0
WireConnection;64;0;1;0
WireConnection;64;1;54;0
WireConnection;64;2;63;0
WireConnection;40;0;7;1
WireConnection;40;1;46;0
WireConnection;53;0;74;0
WireConnection;53;1;47;4
WireConnection;50;1;53;0
WireConnection;50;2;2;4
WireConnection;28;0;12;0
WireConnection;28;1;71;0
WireConnection;12;0;11;0
WireConnection;70;0;45;0
WireConnection;15;0;6;0
WireConnection;15;2;14;0
WireConnection;71;0;73;0
WireConnection;6;0;8;0
WireConnection;6;1;13;0
WireConnection;14;0;3;4
WireConnection;8;0;75;0
WireConnection;51;0;45;0
WireConnection;63;1;6;0
WireConnection;63;2;1;4
WireConnection;11;0;15;0
WireConnection;11;1;9;0
WireConnection;48;0;41;0
WireConnection;48;1;50;0
WireConnection;48;2;47;4
WireConnection;9;2;3;4
WireConnection;39;0;48;0
WireConnection;39;1;58;0
WireConnection;39;2;1;4
WireConnection;73;0;70;0
WireConnection;41;0;29;0
WireConnection;41;2;43;0
WireConnection;49;1;52;0
WireConnection;47;1;49;0
WireConnection;43;0;44;0
WireConnection;46;0;45;0
WireConnection;29;0;28;0
WireConnection;29;1;25;0
WireConnection;2;1;75;0
WireConnection;3;1;75;0
WireConnection;0;0;89;0
ASEEND*/
//CHKSM=6A3463107D92D6371B1288441EF8EE84A52A9028