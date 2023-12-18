
using System;

using UnityEngine;

[Serializable]
public class LiveDataLeaderboard
{
	public bool show = false;
	public bool showSplitter = false;

	public Vector2 offset = Vector2.zero;
	public Vector2 backgroundSize = Vector2.zero;
	public Vector2 splitterPosition = Vector2.zero;

	public Color classColor = Color.white;
	public string textLayer1 = string.Empty;
	public string textLayer2 = string.Empty;

	public LiveDataLeaderboardSlot[] liveDataLeaderboardSlots = new LiveDataLeaderboardSlot[ LiveData.MaxNumDrivers ];
}
