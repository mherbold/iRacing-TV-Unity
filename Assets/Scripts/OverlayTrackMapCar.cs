
using System;

using UnityEngine;

public class OverlayTrackMapCar : MonoBehaviour
{
	public GameObject layer1;
	public GameObject layer2;

	[NonSerialized] public ImageSettings layer1_ImageSettings;
	[NonSerialized] public ImageSettings layer2_ImageSettings;

	public void Awake()
	{
		layer1_ImageSettings = layer1.GetComponent<ImageSettings>();
		layer2_ImageSettings = layer2.GetComponent<ImageSettings>();
	}
}
