
using System;

using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class OverlayHud : MonoBehaviour
{
	public IPC ipc;

	public GameObject enable;
	public GameObject background;
	public GameObject leftSpotterIndicator;
	public GameObject rightSpotterIndicator;
	public TextSettings textLayer1;
	public TextSettings textLayer2;
	public TextSettings textLayer3;
	public TextSettings textLayer4;
	public TextSettings textLayer5;
	public TextSettings textLayer6;
	public TextSettings textLayer7;
	public TextSettings textLayer8;
	public GameObject speechToTextEnable;
	public GameObject speechToTextMaxSizeContainer;
	public GameObject speechToTextPanel;
	public GameObject speechToTextText;

	[NonSerialized] public Image leftSpotterIndicator_Image;
	[NonSerialized] public Image rightSpotterIndicator_Image;
	[NonSerialized] public TextMeshProUGUI textLayer1_Text;
	[NonSerialized] public TextMeshProUGUI textLayer2_Text;
	[NonSerialized] public TextMeshProUGUI textLayer3_Text;
	[NonSerialized] public TextMeshProUGUI textLayer4_Text;
	[NonSerialized] public TextMeshProUGUI textLayer5_Text;
	[NonSerialized] public TextMeshProUGUI textLayer6_Text;
	[NonSerialized] public TextMeshProUGUI textLayer7_Text;
	[NonSerialized] public TextMeshProUGUI textLayer8_Text;
	[NonSerialized] public RectTransform speechToTextMaxSizeContainer_RectTransform;
	[NonSerialized] public VerticalLayoutGroup speechToTextPanel_VerticalLayoutGroup;
	[NonSerialized] public TextMeshProUGUI speechToTextText_Text;

	[NonSerialized] public long indexSettings;
	[NonSerialized] public long indexLiveData;

	public void Awake()
	{
		leftSpotterIndicator_Image = leftSpotterIndicator.GetComponent<Image>();
		rightSpotterIndicator_Image = rightSpotterIndicator.GetComponent<Image>();
		textLayer1_Text = textLayer1.GetComponent<TextMeshProUGUI>();
		textLayer2_Text = textLayer2.GetComponent<TextMeshProUGUI>();
		textLayer3_Text = textLayer3.GetComponent<TextMeshProUGUI>();
		textLayer4_Text = textLayer4.GetComponent<TextMeshProUGUI>();
		textLayer5_Text = textLayer5.GetComponent<TextMeshProUGUI>();
		textLayer6_Text = textLayer6.GetComponent<TextMeshProUGUI>();
		textLayer7_Text = textLayer7.GetComponent<TextMeshProUGUI>();
		textLayer8_Text = textLayer8.GetComponent<TextMeshProUGUI>();
		speechToTextMaxSizeContainer_RectTransform = speechToTextMaxSizeContainer.GetComponent<RectTransform>();
		speechToTextPanel_VerticalLayoutGroup = speechToTextPanel.GetComponent<VerticalLayoutGroup>();
		speechToTextText_Text = speechToTextText.GetComponent<TextMeshProUGUI>();
	}

	public void Update()
	{
		enable.SetActive( LiveData.Instance.liveDataControlPanel.masterOn && Settings.overlay.hudEnabled && ipc.isConnected && LiveData.Instance.isConnected );

		if ( indexSettings != IPC.indexSettings )
		{
			indexSettings = IPC.indexSettings;

			transform.localPosition = new Vector2( Settings.overlay.hudPosition.x, -Settings.overlay.hudPosition.y );

			speechToTextMaxSizeContainer_RectTransform.transform.localPosition = new Vector2( Settings.overlay.hudSpeechToTextPosition.x, -Settings.overlay.hudSpeechToTextPosition.y );

			speechToTextMaxSizeContainer_RectTransform.sizeDelta = Settings.overlay.hudSpeechToTextMaxSize;

			speechToTextPanel_VerticalLayoutGroup.padding = new RectOffset( Settings.overlay.hudSpeechToTextTextPadding.x, Settings.overlay.hudSpeechToTextTextPadding.x, Settings.overlay.hudSpeechToTextTextPadding.y, Settings.overlay.hudSpeechToTextTextPadding.y );
		}

		if ( indexLiveData != IPC.indexLiveData )
		{
			indexLiveData = IPC.indexLiveData;

			textLayer1_Text.text = LiveData.Instance.liveDataHud.textLayer1;
			textLayer1.SetColor( LiveData.Instance.liveDataHud.textLayer1Color );

			textLayer2_Text.text = LiveData.Instance.liveDataHud.textLayer2;
			textLayer2.SetColor( LiveData.Instance.liveDataHud.textLayer2Color );

			textLayer3_Text.text = LiveData.Instance.liveDataHud.textLayer3;
			textLayer3.SetColor( LiveData.Instance.liveDataHud.textLayer3Color );

			textLayer4_Text.text = LiveData.Instance.liveDataHud.textLayer4;
			textLayer4.SetColor( LiveData.Instance.liveDataHud.textLayer4Color );

			textLayer5_Text.text = LiveData.Instance.liveDataHud.textLayer5;
			textLayer5.SetColor( LiveData.Instance.liveDataHud.textLayer5Color );

			textLayer6_Text.text = LiveData.Instance.liveDataHud.textLayer6;
			textLayer6.SetColor( LiveData.Instance.liveDataHud.textLayer6Color );

			textLayer7_Text.text = LiveData.Instance.liveDataHud.textLayer7;
			textLayer7.SetColor( LiveData.Instance.liveDataHud.textLayer7Color );

			textLayer8_Text.text = LiveData.Instance.liveDataHud.textLayer8;
			textLayer8.SetColor( LiveData.Instance.liveDataHud.textLayer8Color );

			if ( LiveData.Instance.liveDataHud.speechToText == string.Empty )
			{
				speechToTextEnable.SetActive( false );
			}
			else
			{
				speechToTextEnable.SetActive( true );

				speechToTextText_Text.text = LiveData.Instance.liveDataHud.speechToText;
			}

			leftSpotterIndicator_Image.enabled = LiveData.Instance.liveDataHud.showLeftSpotterIndicator;
			rightSpotterIndicator_Image.enabled = LiveData.Instance.liveDataHud.showRightSpotterIndicator;
		}
	}
}
