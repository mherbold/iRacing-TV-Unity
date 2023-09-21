
using System;

using UnityEngine;

using TMPro;

public class OverlayLeaderboardSlot : MonoBehaviour
{
	public GameObject preferredCar;
	public GameObject layer1;
	public GameObject layer2;
	public GameObject layer3;
	public GameObject highlight;
	public TextSettings position;
	public TextSettings carNumber;
	public TextSettings driverName;
	public TextSettings telemetry;
	public GameObject speed;

	[NonSerialized] public TextMeshProUGUI position_Text;
	[NonSerialized] public TextMeshProUGUI carNumber_Text;
	[NonSerialized] public TextMeshProUGUI driverName_Text;
	[NonSerialized] public TextMeshProUGUI telemetry_Text;
	[NonSerialized] public TextMeshProUGUI speed_Text;

	[NonSerialized] public ImageSettings preferredCar_ImageSettings;
	[NonSerialized] public ImageSettings layer1_ImageSettings;
	[NonSerialized] public ImageSettings layer2_ImageSettings;
	[NonSerialized] public ImageSettings layer3_ImageSettings;
	[NonSerialized] public ImageSettings highlight_ImageSettings;

	public void Awake()
	{
		position_Text = position.GetComponent<TextMeshProUGUI>();
		carNumber_Text = carNumber.GetComponent<TextMeshProUGUI>();
		driverName_Text = driverName.GetComponent<TextMeshProUGUI>();
		telemetry_Text = telemetry.GetComponent<TextMeshProUGUI>();
		speed_Text = speed.GetComponent<TextMeshProUGUI>();

		preferredCar_ImageSettings = preferredCar.GetComponent<ImageSettings>();
		layer1_ImageSettings = layer1.GetComponent<ImageSettings>();
		layer2_ImageSettings = layer2.GetComponent<ImageSettings>();
		layer3_ImageSettings = layer3.GetComponent<ImageSettings>();
		highlight_ImageSettings = highlight.GetComponent<ImageSettings>();
	}
}
