
using System;

using UnityEngine;

using TMPro;

public class OverlayVoiceOf : MonoBehaviour
{
	public IPC ipc;

	public GameObject enable;
	public GameObject animationRoot;
	public GameObject voiceOf;
	public GameObject driverName;
	public GameObject car;

	[NonSerialized] public Animator animationRoot_Animator;
	[NonSerialized] public TextMeshProUGUI voiceOf_Text;
	[NonSerialized] public TextMeshProUGUI driverName_Text;
	[NonSerialized] public ImageSettings car_ImageSettings;

	[NonSerialized] public long indexSettings;
	[NonSerialized] public long indexLiveData;

	public void Awake()
	{
		animationRoot_Animator = animationRoot.GetComponent<Animator>();
		voiceOf_Text = voiceOf.GetComponent<TextMeshProUGUI>();
		driverName_Text = driverName.GetComponent<TextMeshProUGUI>();
		car_ImageSettings = car.GetComponent<ImageSettings>();
	}

	public void Update()
	{
		enable.SetActive( LiveData.Instance.liveDataControlPanel.masterOn && LiveData.Instance.liveDataControlPanel.voiceOfOn && ipc.isConnected && LiveData.Instance.isConnected );

		if ( indexSettings != IPC.indexSettings )
		{
			indexSettings = IPC.indexSettings;

			transform.localPosition = new Vector2( Settings.overlay.voiceOfPosition.x, -Settings.overlay.voiceOfPosition.y );
		}

		if ( indexLiveData != IPC.indexLiveData )
		{
			indexLiveData = IPC.indexLiveData;

			animationRoot_Animator.SetBool( "Show", LiveData.Instance.liveDataVoiceOf.show );

			voiceOf_Text.text = LiveData.Instance.liveDataVoiceOf.voiceOfText;

			driverName_Text.text = LiveData.Instance.liveDataVoiceOf.driverNameText;

			if ( LiveData.Instance.liveDataVoiceOf.carIdx != -1 )
			{
				car_ImageSettings.carIdx = LiveData.Instance.liveDataVoiceOf.carIdx;
			}
		}
	}
}
