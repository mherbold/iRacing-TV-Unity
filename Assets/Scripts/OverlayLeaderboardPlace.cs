
using System;

using UnityEngine;

using TMPro;

public class OverlayLeaderboardPlace : MonoBehaviour
{
	public GameObject place;
	public GameObject carNumber;
	public GameObject driverName;
	public GameObject telemetry;
	public GameObject highlight;
	public GameObject speed;

	[NonSerialized] public TextMeshProUGUI place_Text;
	[NonSerialized] public TextMeshProUGUI driverName_Text;
	[NonSerialized] public TextMeshProUGUI telemetry_Text;
	[NonSerialized] public TextMeshProUGUI speed_Text;

	[NonSerialized] public ImageSettings carNumber_ImageSettings;
	[NonSerialized] public TextSettings driverName_TextSettings;

	public void Awake()
	{
		place_Text = place.GetComponent<TextMeshProUGUI>();
		driverName_Text = driverName.GetComponent<TextMeshProUGUI>();
		telemetry_Text = telemetry.GetComponent<TextMeshProUGUI>();
		speed_Text = speed.GetComponent<TextMeshProUGUI>();

		carNumber_ImageSettings = carNumber.GetComponent<ImageSettings>();
		driverName_TextSettings = driverName.GetComponent<TextSettings>();
	}
}
