
using System;

using UnityEngine;

[Serializable]
public class SettingsOverlay
{
	[NonSerialized] public const int MaxNumFonts = 4;
	[NonSerialized] public const int MaxNumAnimations = 2;

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
	public Vector2 introLeftPosition = new( 781, 517 );
	public float introLeftScale = 1.7f;
	public Vector2 introRightPosition = new( 1495, 517 );
	public float introRightScale = 1.7f;
	public int introLeftInAnimationNumber = 1;
	public int introRightInAnimationNumber = 1;
	public int introLeftOutAnimationNumber = 1;
	public int introRightOutAnimationNumber = 1;
	public float introInTime = 1;
	public float introHoldTime = 1;
	public float introOutTime = 1;

	public bool startLightsEnabled = true;
	public Vector2 startLightsPosition = new( 903, 130 );

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
