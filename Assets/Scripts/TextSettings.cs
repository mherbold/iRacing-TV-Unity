
using System;

using UnityEngine;

using TMPro;

public class TextSettings : MonoBehaviour
{
	public string id;

	[NonSerialized] public RectTransform rectTransform;
	[NonSerialized] public TextMeshProUGUI text;

	[NonSerialized] public SettingsText settings;

	public void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		text = GetComponent<TextMeshProUGUI>();
	}

	public void Start()
	{
		if ( id != string.Empty )
		{
			settings = Settings.overlay.GetTextSettingsData( id );

			var fontAsset = Fonts.GetFontAsset( settings.fontIndex );

			if ( fontAsset != null )
			{
				text.font = fontAsset;
			}

			text.fontSize = settings.fontSize;
			text.color = settings.tintColor;
			text.alignment = settings.alignment;

			rectTransform.localPosition = new Vector2( settings.position.x, -settings.position.y );
			rectTransform.sizeDelta = settings.size;
		}
	}
}
