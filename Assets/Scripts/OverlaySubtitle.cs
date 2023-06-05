
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class OverlaySubtitle : MonoBehaviour
{
	public GameObject maxSizeContainer;
	public GameObject panel;
	public GameObject text;

	private RectTransform maxSizeContainer_RectTransform;
	private Image panel_Image;
	private VerticalLayoutGroup panel_VerticalLayoutGroup;
	private TextMeshProUGUI text_Text;

	public void Awake()
	{
		maxSizeContainer_RectTransform = maxSizeContainer.GetComponent<RectTransform>();
		panel_Image = panel.GetComponent<Image>();
		panel_VerticalLayoutGroup = panel.GetComponent<VerticalLayoutGroup>();
		text_Text = text.GetComponent<TextMeshProUGUI>();
	}

	public void Start()
	{
		transform.localPosition = new Vector2( Settings.data.subtitleOverlayPosition.x, -Settings.data.subtitleOverlayPosition.y );

		maxSizeContainer_RectTransform.sizeDelta = Settings.data.subtitleOverlayMaxSize;

		panel_Image.color = Settings.data.subtitleOverlayBackgroundColor;
		panel_VerticalLayoutGroup.padding = new RectOffset( Settings.data.subtitleTextPadding.x, Settings.data.subtitleTextPadding.x, Settings.data.subtitleTextPadding.y, Settings.data.subtitleTextPadding.y );

		if ( !Settings.data.showSubtitleOverlay )
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
