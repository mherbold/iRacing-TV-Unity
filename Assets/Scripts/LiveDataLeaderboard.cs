
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
	public string className = string.Empty;
	public string classNameShort = string.Empty;

	public LiveDataLeaderboardSlot[] liveDataLeaderboardSlots = new LiveDataLeaderboardSlot[ LiveData.MaxNumDrivers ];
}
