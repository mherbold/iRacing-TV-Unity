
using System;

using UnityEngine;

using TMPro;

public class OverlayVoiceOf : MonoBehaviour
{
	public GameObject enable;
	public GameObject animationRoot;
	public GameObject voiceOf;
	public GameObject driverName;
	public GameObject car;

	[NonSerialized] public Animator animationRoot_Animator;
	[NonSerialized] public TextMeshProUGUI voiceOf_Text;
	[NonSerialized] public TextMeshProUGUI driverName_Text;
	[NonSerialized] public ImageSettings car_ImageSettings;

	public void Awake()
	{
		animationRoot_Animator = animationRoot.GetComponent<Animator>();
		voiceOf_Text = voiceOf.GetComponent<TextMeshProUGUI>();
		driverName_Text = driverName.GetComponent<TextMeshProUGUI>();
		car_ImageSettings = car.GetComponent<ImageSettings>();
	}

	public void Start()
	{
		SettingsUpdated();
	}

	public void SettingsUpdated()
	{
		transform.localPosition = new Vector2( Settings.overlay.voiceOfOverlayPosition.x, -Settings.overlay.voiceOfOverlayPosition.y );

		enable.SetActive( Settings.overlay.voiceOfOverlayEnabled );
	}

	public void LiveDataUpdated()
	{
		animationRoot_Animator.SetBool( "Show", LiveData.Instance.liveDataVoiceOf.show );

		voiceOf_Text.text = LiveData.Instance.liveDataVoiceOf.voiceOfText;

		driverName_Text.text = LiveData.Instance.liveDataVoiceOf.driverNameText;

		if ( LiveData.Instance.liveDataVoiceOf.carIdx != -1 )
		{
			car_ImageSettings.carIdx = LiveData.Instance.liveDataVoiceOf.carIdx;
		}
	}
}
