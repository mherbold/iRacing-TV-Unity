
using System;

using UnityEngine;

using TMPro;

public class OverlayBattleChyron : MonoBehaviour
{
	public IPC ipc;

	public GameObject enable;
	public GameObject layer1;
	public GameObject layer2;
	public GameObject layer3;
	public GameObject layer4;
	public GameObject layer5;
	public GameObject layer6;
	public GameObject layer7;
	public GameObject textLayer1;
	public GameObject textLayer2;
	public GameObject textLayer3;
	public GameObject textLayer4;
	public GameObject textLayer5;
	public GameObject textLayer6;
	public GameObject textLayer7;
	public GameObject textLayer8;
	public GameObject textLayer9;
	public GameObject textLayer10;
	public GameObject textLayer11;
	public GameObject textLayer12;
	public GameObject textLayer13;
	public GameObject textLayer14;
	public GameObject textLayer15;

	[NonSerialized] public ImageSettings layer1_ImageSettings;
	[NonSerialized] public ImageSettings layer2_ImageSettings;
	[NonSerialized] public ImageSettings layer3_ImageSettings;
	[NonSerialized] public ImageSettings layer4_ImageSettings;
	[NonSerialized] public ImageSettings layer5_ImageSettings;
	[NonSerialized] public ImageSettings layer6_ImageSettings;
	[NonSerialized] public ImageSettings layer7_ImageSettings;
	[NonSerialized] public TextMeshProUGUI textLayer1_Text;
	[NonSerialized] public TextMeshProUGUI textLayer2_Text;
	[NonSerialized] public TextMeshProUGUI textLayer3_Text;
	[NonSerialized] public TextMeshProUGUI textLayer4_Text;
	[NonSerialized] public TextMeshProUGUI textLayer5_Text;
	[NonSerialized] public TextMeshProUGUI textLayer6_Text;
	[NonSerialized] public TextMeshProUGUI textLayer7_Text;
	[NonSerialized] public TextMeshProUGUI textLayer8_Text;
	[NonSerialized] public TextMeshProUGUI textLayer9_Text;
	[NonSerialized] public TextMeshProUGUI textLayer10_Text;
	[NonSerialized] public TextMeshProUGUI textLayer11_Text;
	[NonSerialized] public TextMeshProUGUI textLayer12_Text;
	[NonSerialized] public TextMeshProUGUI textLayer13_Text;
	[NonSerialized] public TextMeshProUGUI textLayer14_Text;
	[NonSerialized] public TextMeshProUGUI textLayer15_Text;

	[NonSerialized] public long indexSettings;
	[NonSerialized] public long indexLiveData;

	public void Awake()
	{
		layer1_ImageSettings = layer1.GetComponent<ImageSettings>();
		layer2_ImageSettings = layer2.GetComponent<ImageSettings>();
		layer3_ImageSettings = layer3.GetComponent<ImageSettings>();
		layer4_ImageSettings = layer4.GetComponent<ImageSettings>();
		layer5_ImageSettings = layer5.GetComponent<ImageSettings>();
		layer6_ImageSettings = layer6.GetComponent<ImageSettings>();
		layer7_ImageSettings = layer7.GetComponent<ImageSettings>();
		textLayer1_Text = textLayer1.GetComponent<TextMeshProUGUI>();
		textLayer2_Text = textLayer2.GetComponent<TextMeshProUGUI>();
		textLayer3_Text = textLayer3.GetComponent<TextMeshProUGUI>();
		textLayer4_Text = textLayer4.GetComponent<TextMeshProUGUI>();
		textLayer5_Text = textLayer5.GetComponent<TextMeshProUGUI>();
		textLayer6_Text = textLayer6.GetComponent<TextMeshProUGUI>();
		textLayer7_Text = textLayer7.GetComponent<TextMeshProUGUI>();
		textLayer8_Text = textLayer8.GetComponent<TextMeshProUGUI>();
		textLayer9_Text = textLayer9.GetComponent<TextMeshProUGUI>();
		textLayer10_Text = textLayer10.GetComponent<TextMeshProUGUI>();
		textLayer11_Text = textLayer11.GetComponent<TextMeshProUGUI>();
		textLayer12_Text = textLayer12.GetComponent<TextMeshProUGUI>();
		textLayer13_Text = textLayer13.GetComponent<TextMeshProUGUI>();
		textLayer14_Text = textLayer14.GetComponent<TextMeshProUGUI>();
		textLayer15_Text = textLayer15.GetComponent<TextMeshProUGUI>();
	}

	public void Update()
	{
		enable.SetActive( LiveData.Instance.liveDataControlPanel.masterOn && LiveData.Instance.liveDataControlPanel.battleChyronOn && LiveData.Instance.liveDataBattleChyron.show && !LiveData.Instance.liveDataIntro.show && ipc.isConnected && LiveData.Instance.isConnected );

		if ( indexSettings != IPC.indexSettings )
		{
			indexSettings = IPC.indexSettings;

			transform.localPosition = new Vector2( Settings.overlay.battleChyronPosition.x, -Settings.overlay.battleChyronPosition.y );
		}

		if ( indexLiveData != IPC.indexLiveData )
		{
			indexLiveData = IPC.indexLiveData;

			layer1_ImageSettings.carIdx = LiveData.Instance.liveDataBattleChyron.carIdx;
			layer2_ImageSettings.carIdx = LiveData.Instance.liveDataBattleChyron.carIdx;
			layer3_ImageSettings.carIdx = LiveData.Instance.liveDataBattleChyron.carIdx;
			layer4_ImageSettings.carIdx = LiveData.Instance.liveDataBattleChyron.carIdx;
			layer5_ImageSettings.carIdx = LiveData.Instance.liveDataBattleChyron.carIdx;
			layer6_ImageSettings.carIdx = LiveData.Instance.liveDataBattleChyron.carIdx;
			layer7_ImageSettings.carIdx = LiveData.Instance.liveDataBattleChyron.carIdx;

			textLayer1_Text.text = LiveData.Instance.liveDataBattleChyron.textLayer1;
			textLayer2_Text.text = LiveData.Instance.liveDataBattleChyron.textLayer2;
			textLayer3_Text.text = LiveData.Instance.liveDataBattleChyron.textLayer3;
			textLayer4_Text.text = LiveData.Instance.liveDataBattleChyron.textLayer4;
			textLayer5_Text.text = LiveData.Instance.liveDataBattleChyron.textLayer5;
			textLayer6_Text.text = LiveData.Instance.liveDataBattleChyron.textLayer6;
			textLayer7_Text.text = LiveData.Instance.liveDataBattleChyron.textLayer7;
			textLayer8_Text.text = LiveData.Instance.liveDataBattleChyron.textLayer8;
			textLayer9_Text.text = LiveData.Instance.liveDataBattleChyron.textLayer9;
			textLayer10_Text.text = LiveData.Instance.liveDataBattleChyron.textLayer10;
			textLayer11_Text.text = LiveData.Instance.liveDataBattleChyron.textLayer11;
			textLayer12_Text.text = LiveData.Instance.liveDataBattleChyron.textLayer12;
			textLayer13_Text.text = LiveData.Instance.liveDataBattleChyron.textLayer13;
			textLayer14_Text.text = LiveData.Instance.liveDataBattleChyron.textLayer14;
			textLayer15_Text.text = LiveData.Instance.liveDataBattleChyron.textLayer15;
		}
	}
}
