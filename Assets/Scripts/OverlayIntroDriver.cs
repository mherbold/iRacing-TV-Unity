
using System;

using UnityEngine;

using TMPro;

public class OverlayIntroDriver : MonoBehaviour
{
	public GameObject layer1;
	public GameObject layer2;
	public GameObject layer3;
	public GameObject layer4;
	public GameObject layer5;
	public GameObject layer6;
	public GameObject position;
	public GameObject qualifyingTime;
	public GameObject driverName;
	public GameObject carNumber;

	[NonSerialized] public TextMeshProUGUI position_Text;
	[NonSerialized] public TextMeshProUGUI qualifyingTime_Text;
	[NonSerialized] public TextMeshProUGUI driverName_Text;
	[NonSerialized] public TextMeshProUGUI carNumber_Text;

	[NonSerialized] public ImageSettings layer1_ImageSettings;
	[NonSerialized] public ImageSettings layer2_ImageSettings;
	[NonSerialized] public ImageSettings layer3_ImageSettings;
	[NonSerialized] public ImageSettings layer4_ImageSettings;
	[NonSerialized] public ImageSettings layer5_ImageSettings;
	[NonSerialized] public ImageSettings layer6_ImageSettings;

	[NonSerialized] public bool shouldBeVisible;

	public void Awake()
	{
		position_Text = position.GetComponent<TextMeshProUGUI>();
		qualifyingTime_Text = qualifyingTime.GetComponent<TextMeshProUGUI>();
		driverName_Text = driverName.GetComponent<TextMeshProUGUI>();
		carNumber_Text = carNumber.GetComponent<TextMeshProUGUI>();

		layer1_ImageSettings = layer1.GetComponent<ImageSettings>();
		layer2_ImageSettings = layer2.GetComponent<ImageSettings>();
		layer3_ImageSettings = layer3.GetComponent<ImageSettings>();
		layer4_ImageSettings = layer4.GetComponent<ImageSettings>();
		layer5_ImageSettings = layer5.GetComponent<ImageSettings>();
		layer6_ImageSettings = layer6.GetComponent<ImageSettings>();
	}
}
