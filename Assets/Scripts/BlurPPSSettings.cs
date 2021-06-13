// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>
#if UNITY_POST_PROCESSING_STACK_V2
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess( typeof( BlurPPSRenderer ), PostProcessEvent.AfterStack, "Blur", true )]
public sealed class BlurPPSSettings : PostProcessEffectSettings
{
	[Tooltip( "smoothness" )]
	public FloatParameter _smoothness = new FloatParameter { value = 1f };
	[Tooltip( "size" )]
	public FloatParameter _size = new FloatParameter { value = 1f };
	[Tooltip( "vigneteTex" )]
	public TextureParameter _vigneteTex = new TextureParameter {  };
	[Tooltip( "Amount" )]
	public FloatParameter _Amount = new FloatParameter { value = 0f };
	[Tooltip( "Debug" )]
	public FloatParameter _Debug = new FloatParameter { value = 0f };
}

public sealed class BlurPPSRenderer : PostProcessEffectRenderer<BlurPPSSettings>
{
	public override void Render( PostProcessRenderContext context )
	{
		var sheet = context.propertySheets.Get( Shader.Find( "Blur" ) );
		sheet.properties.SetFloat( "_smoothness", settings._smoothness );
		sheet.properties.SetFloat( "_size", settings._size );
		if(settings._vigneteTex.value != null) sheet.properties.SetTexture( "_vigneteTex", settings._vigneteTex );
		sheet.properties.SetFloat( "_Amount", settings._Amount );
		sheet.properties.SetFloat( "_Debug", settings._Debug );
		context.command.BlitFullscreenTriangle( context.source, context.destination, sheet, 0 );
	}
}
#endif
