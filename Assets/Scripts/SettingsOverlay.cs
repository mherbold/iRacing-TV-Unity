
using System;

using UnityEngine;

using TMPro;

[Serializable]
public class SettingsOverlay
{
	[NonSerialized] public const int MaxNumFonts = 4;

	public Vector2Int overlayPosition = new( 0, 0 );
	public Vector2Int overlaySize = new( 1920, 1080 );

	public string[] fontPaths = new string[ MaxNumFonts ];

	public bool leaderboardOverlayEnabled = true;
	public Vector2 leaderboardOverlayPosition = new( 44, 244 );
	public Vector2 leaderboardFirstPlacePosition = new( 0, 0 );
	public int leaderboardPlaceCount = 20;
	public Vector2 leaderboardPlaceSpacing = new( 0, 41 );

	public int numberOfCheckpoints = 100;

	public float CarLength = 4.91f;
	public float HeatFalloff = 20.0f;
	public float HeatBias = 0.5f;

	public bool showRaceStatusOverlay = true;
	public Vector2 raceStatusOverlayPosition = new( 44, 9 );

	public bool showVoiceOfOverlay = true;
	public Vector2 voiceOfOverlayPosition = new( 1920, 41 );

	public bool showSubtitleOverlay = true;
	public Vector2 subtitleOverlayPosition = new( 1089, 918 );
	public Vector2 subtitleOverlayMaxSize = new( 1250, 190 );
	public Color subtitleOverlayBackgroundColor = new( 0, 0, 0, 0.9f );
	public Vector2Int subtitleTextPadding = new( 12, 6 );

	public bool useClassColorsForDriverNames = true;
	public float classColorStrength = 0.5f;

	public bool telemetryShowLaps = false;
	public bool telemetryShowDistance = false;
	public bool telemetryShowTime = true;

	public bool telemetryIsBetweenCars = true;

	public Color telemetryTextColor = new( 0.690f, 0.710f, 0.694f, 1 );
	public Color pitTextColor = new( 0.875f, 0.816f, 0.137f, 1 );
	public Color outTextColor = new( 0.875f, 0.125f, 0.125f, 1 );

	public string carNumberColorOverrideA = string.Empty;
	public string carNumberColorOverrideB = string.Empty;
	public string carNumberColorOverrideC = string.Empty;
	public string carNumberPatternOverride = string.Empty;
	public string carNumberSlantOverride = string.Empty;

	public string customPaintsDirectory = Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ) + "\\iRacing\\paint";

	public SerializableDictionary<string, SettingsText> textSettingsDataDictionary = new();
	public SerializableDictionary<string, SettingsImage> imageSettingsDataDictionary = new();
	public SerializableDictionary<string, SettingsTranslation> translationDictionary = new();

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

	public string GetTranslation( string id, string defaultTranslation )
	{
		if ( !translationDictionary.ContainsKey( id ) )
		{
			translationDictionary[ id ] = new SettingsTranslation() { translation = defaultTranslation };
		}

		return translationDictionary[ id ].translation;
	}
}
