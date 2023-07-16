
using System;

using TMPro;

using UnityEngine;

public class Fonts : MonoBehaviour
{
	[NonSerialized] public static readonly string[] fontPaths = new string[ SettingsOverlay.MaxNumFonts ];
	[NonSerialized] public static readonly TMP_FontAsset[] fontAssets = new TMP_FontAsset[ SettingsOverlay.MaxNumFonts ];

	[NonSerialized] public long indexSettings;

	public void Update()
	{
		if ( indexSettings != IPC.indexSettings )
		{
			indexSettings = IPC.indexSettings;

			for ( var fontIndex = 0; fontIndex < SettingsOverlay.MaxNumFonts; fontIndex++ )
			{
				var fontPath = Settings.overlay.fontPaths[ fontIndex ];

				if ( fontPath != fontPaths[ fontIndex ] )
				{
					fontPaths[ fontIndex ] = fontPath;

					if ( fontPath != string.Empty )
					{
						var font = new Font( fontPath );

						fontAssets[ fontIndex ] = TMP_FontAsset.CreateFontAsset( font );
					}
					else
					{
						fontAssets[ fontIndex ] = null;
					}
				}
			}
		}
	}

	public static TMP_FontAsset GetFontAsset( SettingsText.FontIndex fontIndex )
	{
		return fontAssets[ (int) fontIndex ];
	}
}
