
using System;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class OverlaySubtitle : MonoBehaviour
{
	public GameObject maxSizeContainer;
	public GameObject panel;
	public GameObject text;

	[NonSerialized] public RectTransform maxSizeContainer_RectTransform;
	[NonSerialized] public Image panel_Image;
	[NonSerialized] public VerticalLayoutGroup panel_VerticalLayoutGroup;
	[NonSerialized] public TextMeshProUGUI text_Text;

	public void Awake()
	{
		maxSizeContainer_RectTransform = maxSizeContainer.GetComponent<RectTransform>();
		panel_Image = panel.GetComponent<Image>();
		panel_VerticalLayoutGroup = panel.GetComponent<VerticalLayoutGroup>();
		text_Text = text.GetComponent<TextMeshProUGUI>();
	}

	public void Start()
	{
		transform.localPosition = new Vector2( Settings.overlay.subtitleOverlayPosition.x, -Settings.overlay.subtitleOverlayPosition.y );

		maxSizeContainer_RectTransform.sizeDelta = Settings.overlay.subtitleOverlayMaxSize;

		panel_Image.color = Settings.overlay.subtitleOverlayBackgroundColor;
		panel_VerticalLayoutGroup.padding = new RectOffset( Settings.overlay.subtitleTextPadding.x, Settings.overlay.subtitleTextPadding.x, Settings.overlay.subtitleTextPadding.y, Settings.overlay.subtitleTextPadding.y );

		if ( !Settings.overlay.showSubtitleOverlay )
		{
			gameObject.SetActive( false );
		}
	}

	public void Update()
	{
		string text = ChatLogPlayback.Playback( IRSDK.normalizedSession.sessionNumber, IRSDK.normalizedData.sessionTime );

		if ( text == null )
		{
			panel.SetActive( false );
		}
		else
		{
			panel.SetActive( true );

			text_Text.text = text;
		}
	}
}
