
using System;

using UnityEngine;

[Serializable]
public class LiveDataRaceResultSlot
{
	public bool show = false;
	public bool showPreferredCar = false;

	public Vector2 offset = Vector2.zero;

	public string textLayer1 = string.Empty;
	public Color textLayer1Color = Color.white;

	public string textLayer2 = string.Empty;
	public Color textLayer2Color = Color.white;

	public string textLayer3 = string.Empty;
	public Color textLayer3Color = Color.white;

	public string textLayer4 = string.Empty;
	public Color textLayer4Color = Color.white;
}
