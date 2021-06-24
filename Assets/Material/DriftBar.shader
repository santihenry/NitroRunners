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
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_Float0("Float 0", Float) = 0
		_TextureSample3("Texture Sample 3", 2D) = "white" {}
		[HDR]_Color0("Color 0", Color) = (2.79544,1.448945,0,1)
		_driftBar("_driftBar", Range( 0 , 1)) = 0
		_Color1("Color 1", Color) = (0.1320755,0.1320755,0.1320755,1)

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
			uniform float _Float0;
			uniform sampler2D _TextureSample2;
			uniform float _driftBar;
			uniform sampler2D _TextureSample1;
			uniform float4 _Color1;
			uniform sampler2D _TextureSample3;
			uniform sampler2D _TextureSample0;
			uniform float4 _Color0;
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
				float2 uv075 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float cos76 = cos( 1.6 );
				float sin76 = sin( 1.6 );
				float2 rotator76 = mul( uv075 - float2( 0.5,0.5 ) , float2x2( cos76 , -sin76 , sin76 , cos76 )) + float2( 0.5,0.5 );
				float2 panner8 = ( -1.0 * _Time.y * float2( 1,0 ) + rotator76);
				float simplePerlin2D6 = snoise( panner8*_Float0 );
				simplePerlin2D6 = simplePerlin2D6*0.5 + 0.5;
				float2 uv084 = IN.texcoord.xy * float2( 1.36,1 ) + float2( 0.35,0 );
				float cos82 = cos( 1.6 );
				float sin82 = sin( 1.6 );
				float2 rotator82 = mul( ( 1.0 - uv084 ) - float2( 0.5,0.5 ) , float2x2( cos82 , -sin82 , sin82 , cos82 )) + float2( 0.5,0.5 );
				float4 tex2DNode3 = tex2D( _TextureSample2, rotator82 );
				float lerpResult15 = lerp( simplePerlin2D6 , 1.0 , (0.0 + (tex2DNode3.a - 0.0) * (1.0 - 0.0) / (1.0 - 0.0)));
				float lerpResult9 = lerp( 0.0 , 1.0 , tex2DNode3.a);
				float clampResult73 = clamp( (0.0 + (_driftBar - 0.7) * (0.5 - 0.0) / (0.71 - 0.7)) , 0.0 , 0.5 );
				Gradient gradient31 = NewGradient( 0, 2, 2, float4( 0.1921999, 0.589, 0, 0 ), float4( 0.611, 0.4958306, 0.01335519, 1 ), 0, 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
				float2 uv07 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				Gradient gradient34 = NewGradient( 0, 2, 2, float4( 1, 0.1487731, 0, 0 ), float4( 1, 0.5563877, 0, 1 ), 0, 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
				float4 lerpResult38 = lerp( SampleGradient( gradient31, uv07.y ) , SampleGradient( gradient34, uv07.y ) , saturate( (0.0 + (_driftBar - 0.7) * (1.0 - 0.0) / (0.71 - 0.7)) ));
				float4 tex2DNode2 = tex2D( _TextureSample1, rotator76 );
				float4 lerpResult25 = lerp( float4( 0,0,0,0 ) , lerpResult38 , tex2DNode2.a);
				float4 lerpResult41 = lerp( ( ( saturate( ( lerpResult15 * lerpResult9 ) ) * saturate( clampResult73 ) ) + lerpResult25 ) , float4( 0,0,0,0 ) , saturate( ceil( ( uv07.y + (0.0 + (_driftBar - 0.0) * (-1.0 - 0.0) / (1.0 - 0.0)) ) ) ));
				float4 appendResult52 = (float4(0.24 , (0.5 + (_driftBar - 0.0) * (-0.5 - 0.5) / (1.0 - 0.0)) , 0.0 , 0.0));
				float2 uv049 = IN.texcoord.xy * float2( 1,1 ) + appendResult52.xy;
				float cos78 = cos( 1.6 );
				float sin78 = sin( 1.6 );
				float2 rotator78 = mul( uv049 - float2( 0.5,0.5 ) , float2x2( cos78 , -sin78 , sin78 , cos78 )) + float2( 0.5,0.5 );
				float4 tex2DNode47 = tex2D( _TextureSample3, rotator78 );
				float4 lerpResult50 = lerp( float4( 0,0,0,0 ) , ( _Color1 * tex2DNode47.a ) , tex2DNode2.a);
				float4 lerpResult48 = lerp( lerpResult41 , lerpResult50 , tex2DNode47.a);
				float4 tex2DNode1 = tex2D( _TextureSample0, rotator76 );
				float lerpResult63 = lerp( 0.0 , simplePerlin2D6 , tex2DNode1.a);
				float4 lerpResult64 = lerp( tex2DNode1 , _Color0 , lerpResult63);
				float4 lerpResult58 = lerp( tex2DNode1 , lerpResult64 , saturate( (0.0 + (_driftBar - 0.9) * (1.0 - 0.0) / (0.91 - 0.9)) ));
				float4 lerpResult39 = lerp( lerpResult48 , lerpResult58 , tex2DNode1.a);
				
				half4 color = lerpResult39;
				
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
7;437;930;276;3305.539;85.89091;2.684892;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;84;-2452.192,261.7884;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1.36,1;False;1;FLOAT2;0.35,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;85;-2055.546,259.355;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;75;-2454.471,9.636248;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;83;-2049.865,385.5272;Inherit;False;Constant;_Float4;Float 4;7;0;Create;True;0;0;False;0;1.6;1.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-2296.359,164.1783;Inherit;False;Constant;_Float1;Float 1;7;0;Create;True;0;0;False;0;1.6;1.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;82;-1805.715,198.8849;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode;76;-2044.909,-58.96552;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-563.4877,304.5773;Inherit;False;Property;_Float0;Float 0;3;0;Create;True;0;0;False;0;0;8.73;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-821.5151,788.7576;Inherit;False;Property;_driftBar;_driftBar;6;0;Create;True;0;0;False;0;0;0.9201509;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-1416.395,211.989;Inherit;True;Property;_TextureSample2;Texture Sample 2;2;0;Create;True;0;0;False;0;-1;None;1bdbb7800d0ea5d44a07558de8189f2d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;8;-755.3489,447.6654;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,0;False;1;FLOAT;-1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;51;221.5521,1085.804;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.5;False;4;FLOAT;-0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;80;310.0037,994.7659;Inherit;False;Constant;_Float3;Float 3;8;0;Create;True;0;0;False;0;0.24;0.24;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;14;-826.9139,181.4249;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;6;-257.2271,290.807;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;8.31;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;9;-512.6938,-9.529844;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientNode;34;-677.5563,-382.1971;Inherit;False;0;2;2;1,0.1487731,0,0;1,0.5563877,0,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.GradientNode;31;-673.6857,-491.4313;Inherit;False;0;2;2;0.1921999,0.589,0,0;0.611,0.4958306,0.01335519,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.LerpOp;15;-21.10959,221.8932;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;67;382.4266,-319.6107;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0.7;False;2;FLOAT;0.71;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;70;77.06292,444.4749;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0.7;False;2;FLOAT;0.71;False;3;FLOAT;0;False;4;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;52;509.7736,1047.847;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-1647.154,500.2642;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;73;308.9023,290.3198;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientSampleNode;33;-78.36597,-621.3275;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;68;597.1213,-401.0693;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;49;675.056,942.6909;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;242.1028,3.817017;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientSampleNode;37;-46.19505,-378.01;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;46;-344.9994,765.2444;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;674.8724,1136.337;Inherit;False;Constant;_Float2;Float 2;7;0;Create;True;0;0;False;0;1.6;1.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;71;502.1497,160.5167;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;40;140.1632,674.3883;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;78;905.3234,959.5411;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;2;-1613.424,-145.5815;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;-1;None;48eddd0bfdb9a1344b2854be2b28caab;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;12;496.148,-11.89938;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;38;742.3563,-541.2312;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-1642.335,-388.7963;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;-1;None;73b486accb070cb458174ff9d56dc555;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;74;1267.177,416.7111;Inherit;False;Property;_Color1;Color 1;7;0;Create;True;0;0;False;0;0.1320755,0.1320755,0.1320755,1;0.162,0.162,0.162,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;667.2287,-36.30818;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0.72;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;25;765.1761,-208.8173;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;47;1112.7,831.8284;Inherit;True;Property;_TextureSample3;Texture Sample 3;4;0;Create;True;0;0;False;0;-1;None;ad3d7e4a0c2de2b489cd61cfef267b21;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CeilOpNode;44;566.043,579.3099;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;1038.665,-10.93338;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;63;1839.286,1005.567;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;43;1098.995,340.7836;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;1531.195,455.8441;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;65;1973.113,-221.5177;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0.9;False;2;FLOAT;0.91;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;54;962.9775,574.7703;Inherit;False;Property;_Color0;Color 0;5;1;[HDR];Create;True;0;0;False;0;2.79544,1.448945,0,1;2.79544,0.649087,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;41;1357.121,-18.8645;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;64;2244.088,986.0641;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;66;2245.953,-259.2393;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;50;1756.048,84.24624;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;48;2184.199,-34.53662;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;58;2493.029,-350.2501;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;39;2756.926,-172.3026;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;3155.015,-181.8232;Float;False;True;-1;2;ASEMaterialInspector;0;4;DriftBar;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;True;2;False;-1;True;True;True;True;True;0;True;-9;True;True;0;True;-5;255;True;-8;255;True;-7;0;True;-4;0;True;-6;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;0;True;-11;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;0
WireConnection;85;0;84;0
WireConnection;82;0;85;0
WireConnection;82;2;83;0
WireConnection;76;0;75;0
WireConnection;76;2;77;0
WireConnection;3;1;82;0
WireConnection;8;0;76;0
WireConnection;51;0;45;0
WireConnection;14;0;3;4
WireConnection;6;0;8;0
WireConnection;6;1;13;0
WireConnection;9;2;3;4
WireConnection;15;0;6;0
WireConnection;15;2;14;0
WireConnection;67;0;45;0
WireConnection;70;0;45;0
WireConnection;52;0;80;0
WireConnection;52;1;51;0
WireConnection;73;0;70;0
WireConnection;33;0;31;0
WireConnection;33;1;7;2
WireConnection;68;0;67;0
WireConnection;49;1;52;0
WireConnection;11;0;15;0
WireConnection;11;1;9;0
WireConnection;37;0;34;0
WireConnection;37;1;7;2
WireConnection;46;0;45;0
WireConnection;71;0;73;0
WireConnection;40;0;7;2
WireConnection;40;1;46;0
WireConnection;78;0;49;0
WireConnection;78;2;79;0
WireConnection;2;1;76;0
WireConnection;12;0;11;0
WireConnection;38;0;33;0
WireConnection;38;1;37;0
WireConnection;38;2;68;0
WireConnection;1;1;76;0
WireConnection;28;0;12;0
WireConnection;28;1;71;0
WireConnection;25;1;38;0
WireConnection;25;2;2;4
WireConnection;47;1;78;0
WireConnection;44;0;40;0
WireConnection;29;0;28;0
WireConnection;29;1;25;0
WireConnection;63;1;6;0
WireConnection;63;2;1;4
WireConnection;43;0;44;0
WireConnection;53;0;74;0
WireConnection;53;1;47;4
WireConnection;65;0;45;0
WireConnection;41;0;29;0
WireConnection;41;2;43;0
WireConnection;64;0;1;0
WireConnection;64;1;54;0
WireConnection;64;2;63;0
WireConnection;66;0;65;0
WireConnection;50;1;53;0
WireConnection;50;2;2;4
WireConnection;48;0;41;0
WireConnection;48;1;50;0
WireConnection;48;2;47;4
WireConnection;58;0;1;0
WireConnection;58;1;64;0
WireConnection;58;2;66;0
WireConnection;39;0;48;0
WireConnection;39;1;58;0
WireConnection;39;2;1;4
WireConnection;0;0;39;0
ASEEND*/
//CHKSM=4E9636206C2D4AAED2CE607E53D3A3C63ECBA29D