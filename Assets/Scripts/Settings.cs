
using System;
using System.IO;
using System.Xml.Serialization;

using UnityEngine;

using TMPro;

public class Settings : MonoBehaviour
{
	public const string SettingsFileName = "Settings.xml";
	public const int MaxNumFonts = 4;

	public static string settingsFilePath = Program.documentsFolder + SettingsFileName;

	public static bool dataIsValid = false;
	public static SettingsData data = new();

	public static readonly TMP_FontAsset[] fontAssets = new TMP_FontAsset[ MaxNumFonts ];

	public void Awake()
    {
		Load();
		Save();
    }

	public void OnDestroy()
	{
		Save();
	}

	public static void Load()
	{
		if ( File.Exists( settingsFilePath ) )
		{
			try
			{
				var xmlSerializer = new XmlSerializer( typeof( SettingsData ) );

				var fileStream = new FileStream( settingsFilePath, FileMode.Open );

				var deserializedObject = xmlSerializer.Deserialize( fileStream );

				if ( deserializedObject != null )
				{
					data = (SettingsData) deserializedObject;

					dataIsValid = true;
				}

				fileStream.Close();
			}
			catch ( Exception exception )
			{
				Debug.Log( exception.Message );
			}

			if ( data == null )
			{
				data = new SettingsData();
			}
		}

		for ( var fontIndex = 0; fontIndex < MaxNumFonts; fontIndex++ )
		{
			var fontFileName = data.fontFileNames[ fontIndex ];

			if ( fontFileName != string.Empty )
			{
				if ( !File.Exists( fontFileName ) )
				{
					fontFileName = Program.documentsFolder + fontFileName;
				}

				if ( File.Exists( fontFileName ) )
				{
					var font = new Font( fontFileName );

					fontAssets[ fontIndex ] = TMP_FontAsset.CreateFontAsset( font );
				}
			}
		}
	}

	public static void Save()
	{
		if ( !dataIsValid )
		{
			return;
		}

		var xmlSerializer = new XmlSerializer( typeof( SettingsData ) );

		var streamWriter = new StreamWriter( settingsFilePath );

		xmlSerializer.Serialize( streamWriter, data );

		streamWriter.Close();
	}

	public static TMP_FontAsset GetFontAsset( int fontIndex )
	{
		return fontAssets[ fontIndex ];
	}
}
