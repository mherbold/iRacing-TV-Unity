
using System;

using UnityEngine;

[Serializable]
public class SettingsOverlay
{
	[NonSerialized] public const int MaxNumFonts = 4;

	public Vector2Int overlayPosition = new( 0, 0 );
	public Vector2Int overlaySize = new( 1920, 1080 );

	public string[] fontPaths = new string[ MaxNumFonts ];

	public bool raceStatusOverlayEnabled = true;
	public Vector2 raceStatusOverlayPosition = new( 44, 9 );

	public bool leaderboardOverlayEnabled = true;
	public Vector2 leaderboardOverlayPosition = new( 44, 244 );

	public bool voiceOfOverlayEnabled = true;
	public Vector2 voiceOfOverlayPosition = new( 1920, 41 );

	public bool subtitleOverlayEnabled = true;
	public Vector2 subtitleOverlayPosition = new( 1089, 918 );
	public Vector2 subtitleOverlayMaxSize = new( 1250, 190 );
	public Color subtitleOverlayBackgroundColor = new( 0, 0, 0, 0.9f );
	public Vector2Int subtitleTextPadding = new( 12, 6 );

	public SerializableDictionary<string, SettingsText> textSettingsDataDictionary = new();
	public SerializableDictionary<string, SettingsImage> imageSettingsDataDictionary = new();

	public SettingsOverlay()
	{
		for ( var fontIndex = 0; fontIndex < MaxNumFonts; fontIndex++ )
		{
			fontPaths[ fontIndex ] = string.Empty;
		}
	}

	public SettingsText GetTextSettingsData( string id )
	{
		if ( !textSettingsDataDictionary.ContainsKey( id ) )
		{
			textSettingsDataDictionary[ id ] = new SettingsText();
		}

		return textSettingsDataDictionary[ id ];
	}

	public SettingsImage GetImageSettings( string id )
	{
		if ( !imageSettingsDataDictionary.ContainsKey( id ) )
		{
			imageSettingsDataDictionary[ id ] = new SettingsImage();
		}

		return imageSettingsDataDictionary[ id ];
	}
}
