
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

	public bool startLightsEnabled = true;
	public Vector2 startLightsPosition = Vector2.zero;

	public bool raceStatusEnabled = true;
	public Vector2 raceStatusPosition = Vector2.zero;

	public bool leaderboardEnabled = true;
	public Vector2 leaderboardPosition = Vector2.zero;

	public bool trackMapEnabled = true;
	public Vector2 trackMapPosition = Vector2.zero;
	public Vector2 trackMapSize = Vector2.zero;
	public string trackMapTextureFilePath = string.Empty;
	public float trackMapLineThickness = 0;
	public Color trackMapLineColor = Color.white;

	public bool voiceOfEnabled = true;
	public Vector2 voiceOfPosition = Vector2.zero;

	public bool subtitleEnabled = true;
	public Vector2 subtitlePosition = Vector2.zero;
	public Vector2 subtitleMaxSize = Vector2.zero;
	public Vector2Int subtitleTextPadding = Vector2Int.zero;

	public bool introEnabled = true;
	public Vector2 introLeftPosition = Vector2.zero;
	public float introLeftScale = 1;
	public Vector2 introRightPosition = Vector2.zero;
	public float introRightScale = 1;
	public int introLeftInAnimationNumber = 1;
	public int introRightInAnimationNumber = 1;
	public int introLeftOutAnimationNumber = 1;
	public int introRightOutAnimationNumber = 1;
	public float introInTime = 1;
	public float introHoldTime = 1;
	public float introOutTime = 1;

	public bool hudEnabled = true;
	public Vector2 hudPosition = Vector2.zero;
	public Vector2 hudSpeechToTextPosition = Vector2.zero;
	public Vector2 hudSpeechToTextMaxSize = Vector2.zero;
	public Vector2Int hudSpeechToTextTextPadding = Vector2Int.zero;

	public SerializableDictionary<string, SettingsText> textSettingsDataDictionary = new();
	public SerializableDictionary<string, SettingsImage> imageSettingsDataDictionary = new();

	[NonSerialized] public SettingsText emptySettingsText = new();
	[NonSerialized] public SettingsImage emptySettingsImage = new();

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
			return emptySettingsText;
		}

		return textSettingsDataDictionary[ id ];
	}

	public SettingsImage GetImageSettings( string id )
	{
		if ( !imageSettingsDataDictionary.ContainsKey( id ) )
		{
			return emptySettingsImage;
		}

		return imageSettingsDataDictionary[ id ];
	}
}
