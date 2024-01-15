
using System;

using UnityEngine;

using TMPro;

public class OverlayRaceStatus : MonoBehaviour
{
	public IPC ipc;

	public GameObject enable;
	public GameObject blackLight;
	public GameObject greenLight;
	public GameObject whiteLight;
	public GameObject yellowLight;
	public GameObject textLayer1;
	public GameObject textLayer2;
	public GameObject textLayer3;
	public GameObject textLayer4;
	public GameObject greenFlag;
	public GameObject yellowFlag;
	public GameObject checkeredFlag;

	[NonSerialized] public TextMeshProUGUI textLayer1_Text;
	[NonSerialized] public TextMeshProUGUI textLayer2_Text;
	[NonSerialized] public TextMeshProUGUI textLayer3_Text;
	[NonSerialized] public TextMeshProUGUI textLayer4_Text;

	[NonSerialized] public long indexSettings;
	[NonSerialized] public long indexLiveData;

	public void Awake()
	{
		textLayer1_Text = textLayer1.GetComponent<TextMeshProUGUI>();
		textLayer2_Text = textLayer2.GetComponent<TextMeshProUGUI>();
		textLayer3_Text = textLayer3.GetComponent<TextMeshProUGUI>();
		textLayer4_Text = textLayer4.GetComponent<TextMeshProUGUI>();
	}

	public void Update()
	{
		enable.SetActive( LiveData.Instance.liveDataControlPanel.masterOn && LiveData.Instance.liveDataControlPanel.raceStatusOn && !LiveData.Instance.liveDataIntro.show && !LiveData.Instance.liveDataRaceResult.show && ipc.isConnected && LiveData.Instance.isConnected );

		if ( indexSettings != IPC.indexSettings )
		{
			indexSettings = IPC.indexSettings;

			transform.localPosition = new Vector2( Settings.overlay.raceStatusPosition.x, -Settings.overlay.raceStatusPosition.y );
		}

		if ( indexLiveData != IPC.indexLiveData )
		{
			indexLiveData = IPC.indexLiveData;

			blackLight.SetActive( LiveData.Instance.liveDataRaceStatus.showBlackLight );
			greenLight.SetActive( LiveData.Instance.liveDataRaceStatus.showGreenLight );
			whiteLight.SetActive( LiveData.Instance.liveDataRaceStatus.showWhiteLight );
			yellowLight.SetActive( LiveData.Instance.liveDataRaceStatus.showYellowLight );

			textLayer1_Text.text = LiveData.Instance.liveDataRaceStatus.textLayer1;
			textLayer2_Text.text = LiveData.Instance.liveDataRaceStatus.textLayer2;
			textLayer3_Text.text = LiveData.Instance.liveDataRaceStatus.textLayer3;
			textLayer4_Text.text = LiveData.Instance.liveDataRaceStatus.textLayer4;

			greenFlag.SetActive( LiveData.Instance.liveDataRaceStatus.showGreenFlag );
			yellowFlag.SetActive( LiveData.Instance.liveDataRaceStatus.showYellowFlag );
			checkeredFlag.SetActive( LiveData.Instance.liveDataRaceStatus.showCheckeredFlag );
		}
	}
}
