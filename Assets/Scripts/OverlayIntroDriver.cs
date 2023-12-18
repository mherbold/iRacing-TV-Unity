
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
	public GameObject textLayer1;
	public GameObject textLayer2;
	public GameObject textLayer3;
	public GameObject textLayer4;
	public GameObject textLayer5;
	public GameObject textLayer6;

	[NonSerialized] public ImageSettings layer1_ImageSettings;
	[NonSerialized] public ImageSettings layer2_ImageSettings;
	[NonSerialized] public ImageSettings layer3_ImageSettings;
	[NonSerialized] public ImageSettings layer4_ImageSettings;
	[NonSerialized] public ImageSettings layer5_ImageSettings;
	[NonSerialized] public ImageSettings layer6_ImageSettings;
	[NonSerialized] public TextMeshProUGUI textLayer1_Text;
	[NonSerialized] public TextMeshProUGUI textLayer2_Text;
	[NonSerialized] public TextMeshProUGUI textLayer3_Text;
	[NonSerialized] public TextMeshProUGUI textLayer4_Text;
	[NonSerialized] public TextMeshProUGUI textLayer5_Text;
	[NonSerialized] public TextMeshProUGUI textLayer6_Text;

	[NonSerialized] public bool shouldBeVisible;

	public void Awake()
	{
		layer1_ImageSettings = layer1.GetComponent<ImageSettings>();
		layer2_ImageSettings = layer2.GetComponent<ImageSettings>();
		layer3_ImageSettings = layer3.GetComponent<ImageSettings>();
		layer4_ImageSettings = layer4.GetComponent<ImageSettings>();
		layer5_ImageSettings = layer5.GetComponent<ImageSettings>();
		layer6_ImageSettings = layer6.GetComponent<ImageSettings>();
		textLayer1_Text = textLayer1.GetComponent<TextMeshProUGUI>();
		textLayer2_Text = textLayer2.GetComponent<TextMeshProUGUI>();
		textLayer3_Text = textLayer3.GetComponent<TextMeshProUGUI>();
		textLayer4_Text = textLayer4.GetComponent<TextMeshProUGUI>();
		textLayer5_Text = textLayer5.GetComponent<TextMeshProUGUI>();
		textLayer6_Text = textLayer6.GetComponent<TextMeshProUGUI>();
	}
}
