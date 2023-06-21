
using System;

using UnityEngine;

using TMPro;

public class OverlayRaceStatus : MonoBehaviour
{
	public GameObject enable;
	public GameObject sessionName;
	public GameObject lapsRemaining;
	public GameObject blackLight;
	public GameObject greenLight;
	public GameObject whiteLight;
	public GameObject yellowLight;
	public GameObject units;
	public GameObject currentLap;

	[NonSerialized] public TextMeshProUGUI sessionName_Text;
	[NonSerialized] public TextMeshProUGUI lapsRemaining_Text;
	[NonSerialized] public TextMeshProUGUI units_Text;
	[NonSerialized] public TextMeshProUGUI currentLap_Text;

	public void Awake()
	{
		sessionName_Text = sessionName.GetComponent<TextMeshProUGUI>();
		lapsRemaining_Text = lapsRemaining.GetComponent<TextMeshProUGUI>();
		units_Text = units.GetComponent<TextMeshProUGUI>();
		currentLap_Text = currentLap.GetComponent<TextMeshProUGUI>();
	}

	public void Start()
	{
		SettingsUpdated();
	}

	public void SettingsUpdated()
	{
		transform.localPosition = new Vector2( Settings.overlay.raceStatusOverlayPosition.x, -Settings.overlay.raceStatusOverlayPosition.y );

		enable.SetActive( Settings.overlay.raceStatusOverlayEnabled );
	}

	public void LiveDataUpdated()
	{
		sessionName_Text.text = LiveData.Instance.liveDataRaceStatus.sessionNameText;

		lapsRemaining_Text.text = LiveData.Instance.liveDataRaceStatus.lapsRemainingText;

		blackLight.SetActive( LiveData.Instance.liveDataRaceStatus.showBlackLight );
		greenLight.SetActive( LiveData.Instance.liveDataRaceStatus.showGreenLight );
		whiteLight.SetActive( LiveData.Instance.liveDataRaceStatus.showWhiteLight );
		yellowLight.SetActive( LiveData.Instance.liveDataRaceStatus.showYellowLight );

		units_Text.text = LiveData.Instance.liveDataRaceStatus.unitsText;

		currentLap_Text.text = LiveData.Instance.liveDataRaceStatus.currentLapText;
	}
}
