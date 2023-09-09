
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
	public GameObject fuel;
	public GameObject lapsToLeader;
	public GameObject rpm;
	public GameObject speed;
	public GameObject gear;
	public GameObject gapTimeFront;
	public GameObject gapTimeBack;
	public GameObject speechToTextEnable;
	public GameObject speechToTextMaxSizeContainer;
	public GameObject speechToTextPanel;
	public GameObject speechToTextText;

	[NonSerialized] public Image leftSpotterIndicator_Image;
	[NonSerialized] public Image rightSpotterIndicator_Image;
	[NonSerialized] public TextMeshProUGUI fuel_Text;
	[NonSerialized] public TextMeshProUGUI lapsToLeader_Text;
	[NonSerialized] public TextMeshProUGUI rpm_Text;
	[NonSerialized] public TextMeshProUGUI speed_Text;
	[NonSerialized] public TextMeshProUGUI gear_Text;
	[NonSerialized] public TextMeshProUGUI gapTimeFront_Text;
	[NonSerialized] public TextMeshProUGUI gapTimeBack_Text;
	[NonSerialized] public RectTransform speechToTextMaxSizeContainer_RectTransform;
	[NonSerialized] public VerticalLayoutGroup speechToTextPanel_VerticalLayoutGroup;
	[NonSerialized] public TextMeshProUGUI speechToTextText_Text;

	[NonSerialized] public long indexSettings;
	[NonSerialized] public long indexLiveData;

	public void Awake()
	{
		leftSpotterIndicator_Image = leftSpotterIndicator.GetComponent<Image>();
		rightSpotterIndicator_Image = rightSpotterIndicator.GetComponent<Image>();

		fuel_Text = fuel.GetComponent<TextMeshProUGUI>();
		lapsToLeader_Text = lapsToLeader.GetComponent<TextMeshProUGUI>();
		rpm_Text = rpm.GetComponent<TextMeshProUGUI>();
		speed_Text = speed.GetComponent<TextMeshProUGUI>();
		gear_Text = gear.GetComponent<TextMeshProUGUI>();
		gapTimeFront_Text = gapTimeFront.GetComponent<TextMeshProUGUI>();
		gapTimeBack_Text = gapTimeBack.GetComponent<TextMeshProUGUI>();
		speechToTextMaxSizeContainer_RectTransform = speechToTextMaxSizeContainer.GetComponent<RectTransform>();
		speechToTextPanel_VerticalLayoutGroup = speechToTextPanel.GetComponent<VerticalLayoutGroup>();
		speechToTextText_Text = speechToTextText.GetComponent<TextMeshProUGUI>();
	}

	public void Update()
	{
		enable.SetActive( LiveData.Instance.liveDataControlPanel.masterOn && LiveData.Instance.liveDataControlPanel.hudOn && ipc.isConnected && LiveData.Instance.isConnected );

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

			fuel_Text.text = LiveData.Instance.liveDataHud.fuel;
			fuel_Text.color = LiveData.Instance.liveDataHud.fuelColor;

			lapsToLeader_Text.text = LiveData.Instance.liveDataHud.lapsToLeader;

			rpm_Text.text = LiveData.Instance.liveDataHud.rpm;
			rpm_Text.color = LiveData.Instance.liveDataHud.rpmColor;

			speed_Text.text = LiveData.Instance.liveDataHud.speed;

			gear_Text.text = LiveData.Instance.liveDataHud.gear;

			gapTimeFront_Text.text = LiveData.Instance.liveDataHud.gapTimeFront;
			gapTimeFront_Text.color = LiveData.Instance.liveDataHud.gapTimeFrontColor;

			gapTimeBack_Text.text = LiveData.Instance.liveDataHud.gapTimeBack;
			gapTimeBack_Text.color = LiveData.Instance.liveDataHud.gapTimeBackColor;

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
