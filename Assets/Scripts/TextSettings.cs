
using UnityEngine;

using TMPro;

public class TextSettings : MonoBehaviour
{
	public string id;

	private RectTransform rectTransform;
	private TextMeshProUGUI text;

	public SettingsData.TextSettingsData textSettingsData;

	public void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		text = GetComponent<TextMeshProUGUI>();
	}

	public void Start()
	{
		if ( id != string.Empty )
		{
			textSettingsData = Settings.data.GetTextSettingsData( id );

			var fontAsset = Settings.GetFontAsset( textSettingsData.fontIndex );

			if ( fontAsset != null )
			{
				text.font = fontAsset;
			}

			text.fontSize = textSettingsData.fontSize;
			text.color = textSettingsData.tintColor;
			text.alignment = textSettingsData.alignment;

			rectTransform.localPosition = new Vector2( textSettingsData.position.x, -textSettingsData.position.y );
			rectTransform.sizeDelta = textSettingsData.size;
		}
	}
}
