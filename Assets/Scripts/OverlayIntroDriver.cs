
using System;

using UnityEngine;

using TMPro;

public class OverlayIntroDriver : MonoBehaviour
{
	public GameObject background;
	public GameObject suit;
	public GameObject car;
	public GameObject helmet;
	public GameObject statsBackground;
	public GameObject carNumber;
	public GameObject position;
	public GameObject qualifyingTime;
	public GameObject driverName;

	[NonSerialized] public TextMeshProUGUI position_Text;
	[NonSerialized] public TextMeshProUGUI qualifyingTime_Text;
	[NonSerialized] public TextMeshProUGUI driverName_Text;

	[NonSerialized] public ImageSettings background_ImageSettings;
	[NonSerialized] public ImageSettings suit_ImageSettings;
	[NonSerialized] public ImageSettings car_ImageSettings;
	[NonSerialized] public ImageSettings helmet_ImageSettings;
	[NonSerialized] public ImageSettings statsBackground_ImageSettings;
	[NonSerialized] public ImageSettings carNumber_ImageSettings;
	[NonSerialized] public TextSettings position_TextSettings;
	[NonSerialized] public TextSettings qualifyingTime_TextSettings;
	[NonSerialized] public TextSettings driverName_TextSettings;

	[NonSerialized] public bool shouldBeVisible;

	public void Awake()
	{
		position_Text = position.GetComponent<TextMeshProUGUI>();
		qualifyingTime_Text = qualifyingTime.GetComponent<TextMeshProUGUI>();
		driverName_Text = driverName.GetComponent<TextMeshProUGUI>();

		background_ImageSettings = background.GetComponent<ImageSettings>();
		suit_ImageSettings = suit.GetComponent<ImageSettings>();
		car_ImageSettings = car.GetComponent<ImageSettings>();
		helmet_ImageSettings = helmet.GetComponent<ImageSettings>();
		statsBackground_ImageSettings = statsBackground.GetComponent<ImageSettings>();
		carNumber_ImageSettings = carNumber.GetComponent<ImageSettings>();
		position_TextSettings = position.GetComponent<TextSettings>();
		qualifyingTime_TextSettings = qualifyingTime.GetComponent<TextSettings>();
		driverName_TextSettings = driverName.GetComponent<TextSettings>();
	}
}
