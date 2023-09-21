
using System;

using UnityEngine;

using TMPro;

public class OverlayChyron : MonoBehaviour
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
	public GameObject driverName;
	public GameObject speedLabel;
	public GameObject speed;
	public GameObject gearLabel;
	public GameObject gear;
	public GameObject rpmLabel;
	public GameObject rpm;
	public GameObject ratingLabel;
	public GameObject rating;
	public GameObject licenseLabel;
	public GameObject license;
	public GameObject hometownLabel;
	public GameObject hometown;
	public GameObject randomLabel;
	public GameObject random;

	[NonSerialized] public ImageSettings layer1_ImageSettings;
	[NonSerialized] public ImageSettings layer2_ImageSettings;
	[NonSerialized] public ImageSettings layer3_ImageSettings;
	[NonSerialized] public ImageSettings layer4_ImageSettings;
	[NonSerialized] public ImageSettings layer5_ImageSettings;
	[NonSerialized] public ImageSettings layer6_ImageSettings;
	[NonSerialized] public ImageSettings layer7_ImageSettings;
	[NonSerialized] public TextMeshProUGUI driverName_Text;
	[NonSerialized] public TextMeshProUGUI speedLabel_Text;
	[NonSerialized] public TextMeshProUGUI speed_Text;
	[NonSerialized] public TextMeshProUGUI gearLabel_Text;
	[NonSerialized] public TextMeshProUGUI gear_Text;
	[NonSerialized] public TextMeshProUGUI rpmLabel_Text;
	[NonSerialized] public TextMeshProUGUI rpm_Text;
	[NonSerialized] public TextMeshProUGUI ratingLabel_Text;
	[NonSerialized] public TextMeshProUGUI rating_Text;
	[NonSerialized] public TextMeshProUGUI licenseLabel_Text;
	[NonSerialized] public TextMeshProUGUI license_Text;
	[NonSerialized] public TextMeshProUGUI hometownLabel_Text;
	[NonSerialized] public TextMeshProUGUI hometown_Text;
	[NonSerialized] public TextMeshProUGUI randomLabel_Text;
	[NonSerialized] public TextMeshProUGUI random_Text;

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

		driverName_Text = driverName.GetComponent<TextMeshProUGUI>();
		speedLabel_Text = speedLabel.GetComponent<TextMeshProUGUI>();
		speed_Text = speed.GetComponent<TextMeshProUGUI>();
		gearLabel_Text = gearLabel.GetComponent<TextMeshProUGUI>();
		gear_Text = gear.GetComponent<TextMeshProUGUI>();
		rpmLabel_Text = rpmLabel.GetComponent<TextMeshProUGUI>();
		rpm_Text = rpm.GetComponent<TextMeshProUGUI>();
		ratingLabel_Text = ratingLabel.GetComponent<TextMeshProUGUI>();
		rating_Text = rating.GetComponent<TextMeshProUGUI>();
		licenseLabel_Text = licenseLabel.GetComponent<TextMeshProUGUI>();
		license_Text = license.GetComponent<TextMeshProUGUI>();
		hometownLabel_Text = hometownLabel.GetComponent<TextMeshProUGUI>();
		hometown_Text = hometown.GetComponent<TextMeshProUGUI>();
		randomLabel_Text = randomLabel.GetComponent<TextMeshProUGUI>();
		random_Text = random.GetComponent<TextMeshProUGUI>();
	}

	public void Update()
	{
		enable.SetActive( LiveData.Instance.liveDataControlPanel.masterOn && LiveData.Instance.liveDataControlPanel.chyronOn && LiveData.Instance.liveDataChyron.show && !LiveData.Instance.liveDataIntro.show && ipc.isConnected && LiveData.Instance.isConnected );

		if ( indexSettings != IPC.indexSettings )
		{
			indexSettings = IPC.indexSettings;

			transform.localPosition = new Vector2( Settings.overlay.chyronPosition.x, -Settings.overlay.chyronPosition.y );
		}

		if ( indexLiveData != IPC.indexLiveData )
		{
			indexLiveData = IPC.indexLiveData;

			driverName_Text.text = LiveData.Instance.liveDataChyron.driverNameText;
			speedLabel_Text.text = LiveData.Instance.liveDataChyron.speedLabelText;
			speed_Text.text = LiveData.Instance.liveDataChyron.speedText;
			gearLabel_Text.text = LiveData.Instance.liveDataChyron.gearLabelText;
			gear_Text.text = LiveData.Instance.liveDataChyron.gearText;
			rpmLabel_Text.text = LiveData.Instance.liveDataChyron.rpmLabelText;
			rpm_Text.text = LiveData.Instance.liveDataChyron.rpmText;
			ratingLabel_Text.text = LiveData.Instance.liveDataChyron.ratingLabelText;
			rating_Text.text = LiveData.Instance.liveDataChyron.ratingText;
			licenseLabel_Text.text = LiveData.Instance.liveDataChyron.licenseLabelText;
			license_Text.text = LiveData.Instance.liveDataChyron.licenseText;
			hometownLabel_Text.text = LiveData.Instance.liveDataChyron.hometownLabelText;
			hometown_Text.text = LiveData.Instance.liveDataChyron.hometownText;
			randomLabel_Text.text = LiveData.Instance.liveDataChyron.randomLabelText;
			random_Text.text = LiveData.Instance.liveDataChyron.randomText;

			layer1_ImageSettings.carIdx = LiveData.Instance.liveDataChyron.carIdx;
			layer2_ImageSettings.carIdx = LiveData.Instance.liveDataChyron.carIdx;
			layer3_ImageSettings.carIdx = LiveData.Instance.liveDataChyron.carIdx;
			layer4_ImageSettings.carIdx = LiveData.Instance.liveDataChyron.carIdx;
			layer5_ImageSettings.carIdx = LiveData.Instance.liveDataChyron.carIdx;
			layer6_ImageSettings.carIdx = LiveData.Instance.liveDataChyron.carIdx;
			layer7_ImageSettings.carIdx = LiveData.Instance.liveDataChyron.carIdx;
		}
	}
}
