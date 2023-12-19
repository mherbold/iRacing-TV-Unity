
using System;
using TMPro;
using UnityEngine;

public class OverlayCustom : MonoBehaviour
{
	public IPC ipc;

	public int index;
	public GameObject enable;
	public GameObject layer1;
	public GameObject layer2;
	public GameObject layer3;
	public TextSettings textLayer1;
	public TextSettings textLayer2;
	public TextSettings textLayer3;

	[NonSerialized] public ImageSettings layer1_ImageSettings;
	[NonSerialized] public ImageSettings layer2_ImageSettings;
	[NonSerialized] public ImageSettings layer3_ImageSettings;
	[NonSerialized] public TextMeshProUGUI textLayer1_Text;
	[NonSerialized] public TextMeshProUGUI textLayer2_Text;
	[NonSerialized] public TextMeshProUGUI textLayer3_Text;

	[NonSerialized] public long indexLiveData;

	public void Awake()
	{
		layer1_ImageSettings = layer1.GetComponent<ImageSettings>();
		layer2_ImageSettings = layer2.GetComponent<ImageSettings>();
		layer3_ImageSettings = layer3.GetComponent<ImageSettings>();
		textLayer1_Text = textLayer1.GetComponent<TextMeshProUGUI>();
		textLayer2_Text = textLayer2.GetComponent<TextMeshProUGUI>();
		textLayer3_Text = textLayer3.GetComponent<TextMeshProUGUI>();
	}

	public void Update()
	{
		enable.SetActive( LiveData.Instance.liveDataControlPanel.masterOn && LiveData.Instance.liveDataControlPanel.customLayerOn[ index ] && ipc.isConnected && LiveData.Instance.isConnected );

		if ( indexLiveData != IPC.indexLiveData )
		{
			indexLiveData = IPC.indexLiveData;

			var liveDataCustom = LiveData.Instance.liveDataCustom[ index ];

			layer1_ImageSettings.carIdx = liveDataCustom.carIdx;
			layer2_ImageSettings.carIdx = liveDataCustom.carIdx;
			layer3_ImageSettings.carIdx = liveDataCustom.carIdx;

			textLayer1_Text.text = liveDataCustom.textLayer1;
			textLayer1.SetColor( liveDataCustom.textLayer1Color );

			textLayer2_Text.text = liveDataCustom.textLayer2;
			textLayer2.SetColor( liveDataCustom.textLayer2Color );

			textLayer3_Text.text = liveDataCustom.textLayer3;
			textLayer3.SetColor( liveDataCustom.textLayer3Color );
		}
	}
}
