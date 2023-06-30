
using System;

[Serializable]
public class LiveDataIntro
{
	public bool show = false;
	public int currentRow = 0;

	public LiveDataIntroDriver[] liveDataIntroDrivers = new LiveDataIntroDriver[ LiveDataLeaderboard.MaxNumPlaces ];
}
