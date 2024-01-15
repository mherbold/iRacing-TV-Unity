
using System;

using UnityEngine;

[Serializable]
public class LiveDataRaceResult
{
	public bool show = false;

	public Vector2 backgroundSize = Vector2.zero;

	public Color classColor = Color.white;
	public string textLayer1 = string.Empty;
	public string textLayer2 = string.Empty;

	public LiveDataRaceResultSlot[] liveDataRaceResultSlots = new LiveDataRaceResultSlot[ LiveData.MaxNumDrivers ];
}
