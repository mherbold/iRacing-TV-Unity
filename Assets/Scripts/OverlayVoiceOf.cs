
using System;

using UnityEngine;

using TMPro;

public class OverlayVoiceOf : MonoBehaviour
{
	public IPC ipc;

	public GameObject enable;
	public GameObject layer1;
	public GameObject layer2;
	public GameObject layer3;
	public GameObject textLayer1;
	public GameObject textLayer2;

	[NonSerialized] public ImageSettings layer1_ImageSettings;
	[NonSerialized] public ImageSettings layer2_ImageSettings;
	[NonSerialized] public ImageSettings layer3_ImageSettings;
	[NonSerialized] public TextMeshProUGUI textLayer1_Text;
	[NonSerialized] public TextMeshProUGUI textLayer2_Text;

	[NonSerialized] public long indexSettings;
	[NonSerialized] public long indexLiveData;

	public void Awake()
	{
		layer1_ImageSettings = layer1.GetComponent<ImageSettings>();
		layer2_ImageSettings = layer2.GetComponent<ImageSettings>();
		layer3_ImageSettings = layer3.GetComponent<ImageSettings>();
		textLayer1_Text = textLayer1.GetComponent<TextMeshProUGUI>();
		textLayer2_Text = textLayer2.GetComponent<TextMeshProUGUI>();
	}

	public void Update()
	{
		enable.SetActive( LiveData.Instance.liveDataControlPanel.masterOn && LiveData.Instance.liveDataControlPanel.voiceOfOn && ipc.isConnected && LiveData.Instance.isConnected && LiveData.Instance.liveDataVoiceOf.show );

		if ( indexSettings != IPC.indexSettings )
		{
			indexSettings = IPC.indexSettings;

			transform.localPosition = new Vector2( Settings.overlay.voiceOfPosition.x, -Settings.overlay.voiceOfPosition.y );
		}

		if ( indexLiveData != IPC.indexLiveData )
		{
			indexLiveData = IPC.indexLiveData;

			textLayer1_Text.text = LiveData.Instance.liveDataVoiceOf.textLayer1;
			textLayer2_Text.text = LiveData.Instance.liveDataVoiceOf.textLayer2;

			if ( LiveData.Instance.liveDataVoiceOf.carIdx != -1 )
			{
				layer1_ImageSettings.carIdx = LiveData.Instance.liveDataVoiceOf.carIdx;
				layer2_ImageSettings.carIdx = LiveData.Instance.liveDataVoiceOf.carIdx;
				layer3_ImageSettings.carIdx = LiveData.Instance.liveDataVoiceOf.carIdx;
			}
		}
	}
}
