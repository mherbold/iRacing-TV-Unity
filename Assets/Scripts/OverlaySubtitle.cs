
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
	[NonSerialized] public VerticalLayoutGroup panel_VerticalLayoutGroup;
	[NonSerialized] public TextMeshProUGUI text_Text;

	[NonSerialized] public long indexSettings;
	[NonSerialized] public long indexLiveData;

	public void Awake()
	{
		maxSizeContainer_RectTransform = maxSizeContainer.GetComponent<RectTransform>();
		panel_VerticalLayoutGroup = panel.GetComponent<VerticalLayoutGroup>();
		text_Text = text.GetComponent<TextMeshProUGUI>();
	}

	public void Update()
	{
		enable.SetActive( LiveData.Instance.liveDataControlPanel.masterOn && LiveData.Instance.liveDataControlPanel.subtitlesOn && ipc.isConnected && LiveData.Instance.isConnected );

		if ( indexSettings != IPC.indexSettings )
		{
			indexSettings = IPC.indexSettings;

			maxSizeContainer_RectTransform.transform.localPosition = new Vector2( Settings.overlay.subtitlePosition.x, -Settings.overlay.subtitlePosition.y );

			maxSizeContainer_RectTransform.sizeDelta = Settings.overlay.subtitleMaxSize;

			panel_VerticalLayoutGroup.padding = new RectOffset( Settings.overlay.subtitleTextPadding.x, Settings.overlay.subtitleTextPadding.x, Settings.overlay.subtitleTextPadding.y, Settings.overlay.subtitleTextPadding.y );
		}

		if ( indexLiveData != IPC.indexLiveData )
		{
			indexLiveData = IPC.indexLiveData;

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
}
