
using UnityEngine;

using TMPro;

public class OverlayPlace : MonoBehaviour
{
	public GameObject place;
	public GameObject carNumber;
	public GameObject driverName;
	public GameObject telemetry;
	public GameObject highlight;
	public GameObject speed;

	public TextMeshProUGUI place_Text;
	public TextMeshProUGUI driverName_Text;
	public TextMeshProUGUI telemetry_Text;
	public TextMeshProUGUI speed_Text;

	public ImageSettings carNumber_ImageSettings;
	public TextSettings driverName_TextSettings;

	public bool shouldBeVisible;

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
