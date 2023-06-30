
using System;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class OverlaySubtitle : MonoBehaviour
{
	public IPC ipc;

	public GameObject enable;
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
		SettingsUpdated();
	}

	public void Update()
	{
		enable.SetActive( Settings.overlay.subtitleEnabled && ipc.isConnected && LiveData.Instance.isConnected );
	}

	public void SettingsUpdated()
	{
		transform.localPosition = new Vector2( Settings.overlay.subtitlePosition.x, -Settings.overlay.subtitlePosition.y );

		maxSizeContainer_RectTransform.sizeDelta = Settings.overlay.subtitleMaxSize;

		panel_Image.color = Settings.overlay.subtitleBackgroundColor;
		panel_VerticalLayoutGroup.padding = new RectOffset( Settings.overlay.subtitleTextPadding.x, Settings.overlay.subtitleTextPadding.x, Settings.overlay.subtitleTextPadding.y, Settings.overlay.subtitleTextPadding.y );
	}

	public void LiveDataUpdated()
	{
		if ( LiveData.Instance.liveDataSubtitle.text == string.Empty )
		{
			panel.SetActive( false );
		}
		else
		{
			panel.SetActive( true );

			text_Text.text = LiveData.Instance.liveDataSubtitle.text;
		}
	}
}
