
using System;

using UnityEngine;

using TMPro;

public class OverlayTrackMapCar : MonoBehaviour
{
	public GameObject layer1;
	public GameObject layer2;
	public GameObject currentTarget;
	public GameObject textLayer1;

	[NonSerialized] public ImageSettings layer1_ImageSettings;
	[NonSerialized] public ImageSettings layer2_ImageSettings;
	[NonSerialized] public TextMeshProUGUI textLayer1_Text;

	public void Awake()
	{
		layer1_ImageSettings = layer1.GetComponent<ImageSettings>();
		layer2_ImageSettings = layer2.GetComponent<ImageSettings>();
		textLayer1_Text = textLayer1.GetComponent<TextMeshProUGUI>();
	}
}
