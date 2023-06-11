
using System;

using TMPro;

using UnityEngine;

public class Fonts : MonoBehaviour
{
	[NonSerialized] public static readonly string[] fontNames = new string[ SettingsOverlay.MaxNumFonts ];
	[NonSerialized] public static readonly TMP_FontAsset[] fontAssets = new TMP_FontAsset[ SettingsOverlay.MaxNumFonts ];

	public void Start()
	{
		OverlayUpdated();
	}

	public void OverlayUpdated()
	{
		for ( var fontIndex = 0; fontIndex < SettingsOverlay.MaxNumFonts; fontIndex++ )
		{
			var fontName = Settings.overlay.fontNames[ fontIndex ];

			if ( fontName != fontNames[ fontIndex ] )
			{
				if ( fontName != string.Empty )
				{
					var font = new Font( fontName );

					fontAssets[ fontIndex ] = TMP_FontAsset.CreateFontAsset( font );
				}
				else
				{
					fontAssets[ fontIndex ] = null;
				}

				fontNames[ fontIndex ] = fontName;
			}
		}
	}

	public static TMP_FontAsset GetFontAsset( SettingsText.FontIndex fontIndex )
	{
		return fontAssets[ (int) fontIndex ];
	}
}
