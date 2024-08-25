
using System;

using UnityEngine;

using TMPro;

public class OverlayTrainer : MonoBehaviour
{
	public IPC ipc;

	public GameObject enable;
	public GameObject message;
	public GameObject countdown;

	[NonSerialized] public TextMeshProUGUI message_Text;
	[NonSerialized] public TextMeshProUGUI countdown_Text;

	[NonSerialized] public long indexSettings;
	[NonSerialized] public long indexLiveData;

	public void Awake()
	{
		message_Text = message.GetComponent<TextMeshProUGUI>();
		countdown_Text = countdown.GetComponent<TextMeshProUGUI>();
	}

	public void Update()
	{
		enable.SetActive( LiveData.Instance.liveDataControlPanel.masterOn && Settings.overlay.trainerEnabled && ipc.isConnected && LiveData.Instance.isConnected );

		if ( indexSettings != IPC.indexSettings )
		{
			indexSettings = IPC.indexSettings;

			transform.localPosition = new Vector2( Settings.overlay.trainerPosition.x, -Settings.overlay.trainerPosition.y );
		}

		if ( indexLiveData != IPC.indexLiveData )
		{
			indexLiveData = IPC.indexLiveData;

			message_Text.text = LiveData.Instance.liveDataTrainer.message;
			countdown_Text.text = LiveData.Instance.liveDataTrainer.countdown;
		}
	}
}
