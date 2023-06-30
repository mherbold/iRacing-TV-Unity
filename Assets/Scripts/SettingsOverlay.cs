
using System;

using UnityEngine;

[Serializable]
public class SettingsOverlay
{
	[NonSerialized] public const int MaxNumFonts = 4;

	public Vector2Int position = new( 0, 0 );
	public Vector2Int size = new( 1920, 1080 );

	public string[] fontPaths = new string[ MaxNumFonts ];

	public bool raceStatusEnabled = true;
	public Vector2 raceStatusPosition = new( 44, 9 );

	public bool leaderboardEnabled = true;
	public Vector2 leaderboardPosition = new( 44, 244 );

	public bool voiceOfEnabled = true;
	public Vector2 voiceOfPosition = new( 1920, 41 );

	public bool subtitleEnabled = true;
	public Vector2 subtitlePosition = new( 1089, 918 );
	public Vector2 subtitleMaxSize = new( 1250, 190 );
	public Color subtitleBackgroundColor = new( 0, 0, 0, 0.9f );
	public Vector2Int subtitleTextPadding = new( 12, 6 );

	public bool introEnabled = true;

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
