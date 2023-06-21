
using System;

[Serializable]
public class LiveDataLeaderboard
{
	public const int MaxNumPlaces = 63;

	public bool show = false;
	public bool showSplitter = false;

	public LiveDataPlace[] liveDataPlaces = new LiveDataPlace[ MaxNumPlaces ];
}
