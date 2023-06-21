
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
		SettingsUpdated();
	}

	public void SettingsUpdated()
	{
		if ( id == string.Empty )
		{
			enabled = false;
		}
		else
		{
			settings = Settings.overlay.GetTextSettingsData( id );

			if ( settings.fontIndex == SettingsText.FontIndex.None )
			{
				enabled = false;
			}
			else
			{
				enabled = true;

				text.font = Fonts.GetFontAsset( settings.fontIndex );
				text.fontSize = settings.fontSize;
				text.color = settings.tintColor;
				text.alignment = settings.alignment;

				rectTransform.localPosition = new Vector2( settings.position.x, -settings.position.y );
				rectTransform.sizeDelta = settings.size;
			}
		}
	}
}
