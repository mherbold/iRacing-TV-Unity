
using System;

using UnityEngine;

[Serializable]
public class LiveDataLeaderboardSlot
{
	public bool show = false;
	public bool showHighlight = false;
	public bool showPreferredCar = false;

	public Vector2 offset = Vector2.zero;

	public string positionText = string.Empty;
	public Color positionColor = Color.white;

	public string carNumberTextureUrl = string.Empty;
	public string carTextureUrl = string.Empty;
	public string helmetTextureUrl = string.Empty;
	public string driverTextureUrl = string.Empty;

	public string carNumberText = string.Empty;
	public Color carNumberColor = Color.white;

	public string driverNameText = string.Empty;
	public Color driverNameColor = Color.white;

	public string telemetryText = string.Empty;
	public Color telemetryColor = Color.white;

	public string speedText = string.Empty;
}
