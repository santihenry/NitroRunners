// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>
#if UNITY_POST_PROCESSING_STACK_V2
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess( typeof( ASESampleShadersSimpleSimpleBlurOFFPPSRenderer ), PostProcessEvent.AfterStack, "ASESampleShadersSimpleSimpleBlurOFF", true )]
public sealed class ASESampleShadersSimpleSimpleBlurOFFPPSSettings : PostProcessEffectSettings
{
	[Tooltip( "MainSample" )]
	public TextureParameter _MainSample = new TextureParameter {  };
	[Tooltip( "Toggle Blur" )]
	public FloatParameter _ToggleBlur = new FloatParameter { value = 0f };
	[Tooltip( "Blur Size" )]
	public FloatParameter _BlurSize = new FloatParameter { value = 0f };
}

public sealed class ASESampleShadersSimpleSimpleBlurOFFPPSRenderer : PostProcessEffectRenderer<ASESampleShadersSimpleSimpleBlurOFFPPSSettings>
{
	public override void Render( PostProcessRenderContext context )
	{
		var sheet = context.propertySheets.Get( Shader.Find( "ASESampleShaders/Simple/SimpleBlurOFF" ) );
		if(settings._MainSample.value != null) sheet.properties.SetTexture( "_MainSample", settings._MainSample );
		sheet.properties.SetFloat( "_ToggleBlur", settings._ToggleBlur );
		sheet.properties.SetFloat( "_BlurSize", settings._BlurSize );
		context.command.BlitFullscreenTriangle( context.source, context.destination, sheet, 0 );
	}
}
#endif
