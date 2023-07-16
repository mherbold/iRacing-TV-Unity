
using System;

using UnityEngine;

using TMPro;

public class OverlayRaceStatus : MonoBehaviour
{
	public IPC ipc;

	public GameObject enable;
	public GameObject sessionName;
	public GameObject lapsRemaining;
	public GameObject blackLight;
	public GameObject greenLight;
	public GameObject whiteLight;
	public GameObject yellowLight;
	public GameObject units;
	public GameObject currentLap;
	public GameObject greenFlag;
	public GameObject yellowFlag;
	public GameObject checkeredFlag;

	[NonSerialized] public TextMeshProUGUI sessionName_Text;
	[NonSerialized] public TextMeshProUGUI lapsRemaining_Text;
	[NonSerialized] public TextMeshProUGUI units_Text;
	[NonSerialized] public TextMeshProUGUI currentLap_Text;

	[NonSerialized] public long indexSettings;
	[NonSerialized] public long indexLiveData;

	public void Awake()
	{
		sessionName_Text = sessionName.GetComponent<TextMeshProUGUI>();
		lapsRemaining_Text = lapsRemaining.GetComponent<TextMeshProUGUI>();
		units_Text = units.GetComponent<TextMeshProUGUI>();
		currentLap_Text = currentLap.GetComponent<TextMeshProUGUI>();
	}

	public void Update()
	{
		enable.SetActive( Settings.overlay.raceStatusEnabled && !LiveData.Instance.liveDataIntro.show && ipc.isConnected && LiveData.Instance.isConnected );

		if ( indexSettings != IPC.indexSettings )
		{
			indexSettings = IPC.indexSettings;

			transform.localPosition = new Vector2( Settings.overlay.raceStatusPosition.x, -Settings.overlay.raceStatusPosition.y );
		}

		if ( indexLiveData != IPC.indexLiveData )
		{
			indexLiveData = IPC.indexLiveData;

			sessionName_Text.text = LiveData.Instance.liveDataRaceStatus.sessionNameText;

			lapsRemaining_Text.text = LiveData.Instance.liveDataRaceStatus.lapsRemainingText;

			blackLight.SetActive( LiveData.Instance.liveDataRaceStatus.showBlackLight );
			greenLight.SetActive( LiveData.Instance.liveDataRaceStatus.showGreenLight );
			whiteLight.SetActive( LiveData.Instance.liveDataRaceStatus.showWhiteLight );
			yellowLight.SetActive( LiveData.Instance.liveDataRaceStatus.showYellowLight );

			units_Text.text = LiveData.Instance.liveDataRaceStatus.unitsText;

			currentLap_Text.text = LiveData.Instance.liveDataRaceStatus.currentLapText;

			greenFlag.SetActive( LiveData.Instance.liveDataRaceStatus.showGreenFlag );
			yellowFlag.SetActive( LiveData.Instance.liveDataRaceStatus.showYellowFlag );
			checkeredFlag.SetActive( LiveData.Instance.liveDataRaceStatus.showCheckeredFlag );
		}
	}
}
