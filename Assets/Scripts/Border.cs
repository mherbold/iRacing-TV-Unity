
using System;

using UnityEngine;
using UnityEngine.UI;

public class Border : MonoBehaviour
{
	public GameObject border;

	[NonSerialized] public static GameObject border_GameObject;
	[NonSerialized] public static Image border_Image;

	public void Awake()
	{
		border_GameObject = border;

		border_Image = border.GetComponent<Image>();

		border_Image.enabled = false;
	}
}
