
using System;

using UnityEngine;

using TMPro;

public class OverlayRaceResultsSlot : MonoBehaviour
{
	public GameObject layer1;
	public GameObject layer2;
	public GameObject position;
	public GameObject driverName;
	public GameObject telemetry;

	[NonSerialized] public TextMeshProUGUI position_Text;
	[NonSerialized] public TextMeshProUGUI driverName_Text;
	[NonSerialized] public TextMeshProUGUI telemetry_Text;

	[NonSerialized] public ImageSettings layer1_ImageSettings;
	[NonSerialized] public ImageSettings layer2_ImageSettings;
	[NonSerialized] public TextSettings driverName_TextSettings;

	public void Awake()
	{
		position_Text = position.GetComponent<TextMeshProUGUI>();
		driverName_Text = driverName.GetComponent<TextMeshProUGUI>();
		telemetry_Text = telemetry.GetComponent<TextMeshProUGUI>();

		layer1_ImageSettings = layer1.GetComponent<ImageSettings>();
		layer2_ImageSettings = layer2.GetComponent<ImageSettings>();
		driverName_TextSettings = driverName.GetComponent<TextSettings>();
	}
}
