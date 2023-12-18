
using System;

using UnityEngine;

using TMPro;

public class OverlayLeaderboardSlot : MonoBehaviour
{
	public GameObject preferredCar;
	public GameObject currentTarget;
	public TextSettings currentTargetTextLayer1;
	public GameObject layer1;
	public GameObject layer2;
	public GameObject layer3;
	public TextSettings textLayer1;
	public TextSettings textLayer2;
	public TextSettings textLayer3;
	public TextSettings textLayer4;

	[NonSerialized] public ImageSettings preferredCar_ImageSettings;
	[NonSerialized] public ImageSettings currentTarget_ImageSettings;
	[NonSerialized] public TextMeshProUGUI currentTargetTextLayer1_Text;
	[NonSerialized] public ImageSettings layer1_ImageSettings;
	[NonSerialized] public ImageSettings layer2_ImageSettings;
	[NonSerialized] public ImageSettings layer3_ImageSettings;
	[NonSerialized] public TextMeshProUGUI textLayer1_Text;
	[NonSerialized] public TextMeshProUGUI textLayer2_Text;
	[NonSerialized] public TextMeshProUGUI textLayer3_Text;
	[NonSerialized] public TextMeshProUGUI textLayer4_Text;

	public void Awake()
	{
		preferredCar_ImageSettings = preferredCar.GetComponent<ImageSettings>();
		currentTarget_ImageSettings = currentTarget.GetComponent<ImageSettings>();
		currentTargetTextLayer1_Text = currentTargetTextLayer1.GetComponent<TextMeshProUGUI>();
		layer1_ImageSettings = layer1.GetComponent<ImageSettings>();
		layer2_ImageSettings = layer2.GetComponent<ImageSettings>();
		layer3_ImageSettings = layer3.GetComponent<ImageSettings>();
		textLayer1_Text = textLayer1.GetComponent<TextMeshProUGUI>();
		textLayer2_Text = textLayer2.GetComponent<TextMeshProUGUI>();
		textLayer3_Text = textLayer3.GetComponent<TextMeshProUGUI>();
		textLayer4_Text = textLayer4.GetComponent<TextMeshProUGUI>();
	}
}
