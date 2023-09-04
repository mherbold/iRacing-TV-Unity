
using System;

using UnityEngine;

using TMPro;

public class OverlayTrackMapCar : MonoBehaviour
{
	public GameObject layer1;
	public GameObject layer2;
	public GameObject highlight;
	public GameObject carNumber;

	[NonSerialized] public TextMeshProUGUI carNumber_Text;

	[NonSerialized] public ImageSettings layer1_ImageSettings;
	[NonSerialized] public ImageSettings layer2_ImageSettings;

	public void Awake()
	{
		carNumber_Text = carNumber.GetComponent<TextMeshProUGUI>();

		layer1_ImageSettings = layer1.GetComponent<ImageSettings>();
		layer2_ImageSettings = layer2.GetComponent<ImageSettings>();
	}
}
