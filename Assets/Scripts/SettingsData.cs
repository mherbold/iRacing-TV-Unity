
using System;

using UnityEngine;

using TMPro;

[Serializable]
public class SettingsData
{
	public Vector2Int overlayPosition = new( 0, 0 );
	public Vector2Int overlaySize = new( 1920, 1080 );

	public string[] fontFileNames = new string[ Settings.MaxNumFonts ];

	public Vector2 leaderboardFirstPlacePosition = new( 44, 244 );
	public float leaderboardPlaceSpacing = 41;
	public int leaderboardPlaceCount = 20;

	public int numberOfCheckpoints = 100;

	public float carHeatMaximumDistance = 100;

	public bool showVoiceOfOverlay = true;
	public Vector2 voiceOfOverlayPosition = new( 1920, 41 );

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

	public class TextSettingsData
	{
		public int fontIndex = 0;
		public int fontSize = 0;

		public TextAlignmentOptions alignment = TextAlignmentOptions.TopLeft;

		public Vector2 position = Vector2.zero;
		public Vector2 size = Vector2.zero;
		public Color tintColor = Color.white;
	}

	public SerializableDictionary<string, TextSettingsData> textSettingsDataDictionary = new();

	public enum ImageType
	{
		Custom,
		SeriesLogo,
		CarNumber,
		Car
	}

	public class ImageSettingsData
	{
		public ImageType imageType = ImageType.Custom;

		public string fileName = string.Empty;

		public Vector2 position;
		public Vector2 size;
		public Color tintColor;
	}

	public SerializableDictionary<string, ImageSettingsData> imageSettingsDataDictionary = new();

	public SerializableDictionary<string, string> translationDictionary = new();

	public SettingsData()
	{
		for ( var fontIndex = 0; fontIndex < Settings.MaxNumFonts; fontIndex++ )
		{
			fontFileNames[ fontIndex ] = string.Empty;
		}
	}

	public TextSettingsData GetTextSettingsData( string id )
	{
		if ( !textSettingsDataDictionary.ContainsKey( id ) )
		{
			textSettingsDataDictionary[ id ] = new TextSettingsData();
		}

		return textSettingsDataDictionary[ id ];
	}

	public ImageSettingsData GetImageSettingsData( string id )
	{
		if ( !imageSettingsDataDictionary.ContainsKey( id ) )
		{
			imageSettingsDataDictionary[ id ] = new ImageSettingsData();
		}

		return imageSettingsDataDictionary[ id ];
	}

	public string GetTranslation( string id, string defaultTranslation )
	{
		if ( !translationDictionary.ContainsKey( id ) )
		{
			translationDictionary[ id ] = defaultTranslation;
		}

		return translationDictionary[ id ];
	}
}
