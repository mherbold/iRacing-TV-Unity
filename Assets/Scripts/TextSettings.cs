
using System;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class TextSettings : MonoBehaviour
{
	public string id = string.Empty;
	public bool enableBorder = true;

	[NonSerialized] public RectTransform rectTransform;
	[NonSerialized] public TextMeshProUGUI text;

	[NonSerialized] public long indexSettings;
	[NonSerialized] public SettingsText settings;

	[NonSerialized] public Vector2? previousSize = null;
	[NonSerialized] public Vector2? previousPosition = null;
	[NonSerialized] public float showBorderTimer = 0;
	[NonSerialized] public GameObject border = null;
	[NonSerialized] public Image border_Image = null;
	[NonSerialized] public RectTransform border_RectTransform = null;

	[NonSerialized] public bool colorOverridden = false;

	public void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		text = GetComponent<TextMeshProUGUI>();
	}

	public void Update()
	{
		if ( enableBorder )
		{
			if ( border == null )
			{
				border = Instantiate( Border.border_GameObject );

				border.name = $"{transform.name} {Border.border_GameObject.name}";

				border.transform.SetParent( transform.parent );

				border.SetActive( true );

				border_Image = border.GetComponent<Image>();
				border_RectTransform = border.GetComponent<RectTransform>();
				border_RectTransform.pivot = rectTransform.pivot;
			}
		}

		if ( indexSettings != IPC.indexSettings )
		{
			indexSettings = IPC.indexSettings;

			if ( id == string.Empty )
			{
				enabled = false;
			}
			else
			{
				settings = Settings.overlay.GetTextSettingsData( id );

				if ( settings.fontIndex == SettingsText.FontIndex.None )
				{
					text.enabled = false;
				}
				else
				{
					text.enabled = true;

					text.font = Fonts.GetFontAsset( settings.fontIndex );
					text.fontSize = settings.fontSize;
					text.alignment = settings.alignment;

					if ( !colorOverridden )
					{
						text.color = settings.tintColor;
					}

					text.overflowMode = ( settings.allowOverflow ) ? TextOverflowModes.Overflow : TextOverflowModes.Ellipsis;

					rectTransform.localPosition = new Vector2( settings.position.x, -settings.position.y );
					rectTransform.sizeDelta = settings.size;

					if ( enableBorder )
					{
						border_RectTransform.localPosition = rectTransform.localPosition;
						border_RectTransform.sizeDelta = rectTransform.sizeDelta;

						if ( border_RectTransform.sizeDelta == Vector2.zero )
						{
							border_RectTransform.sizeDelta = new Vector2( 2, 2 );
						}
					}
				}

				if ( ( previousSize == null ) || ( previousPosition == null ) )
				{
					previousSize = settings.size;
					previousPosition = settings.position;
				}
				else if ( ( previousSize != settings.size ) || ( previousPosition != settings.position ) )
				{
					previousSize = settings.size;
					previousPosition = settings.position;

					showBorderTimer = 3.0f;
				}
			}
		}

		if ( settings != null )
		{
			if ( enableBorder && ( showBorderTimer > 0 ) )
			{
				showBorderTimer = Math.Max( 0, showBorderTimer - Time.deltaTime );

				border_Image.enabled = ( showBorderTimer > 0 );
			}
		}
	}

	public void SetColor( Color color )
	{
		colorOverridden = true;

		text.color = color;
	}
}
