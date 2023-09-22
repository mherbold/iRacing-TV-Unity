﻿
using System;

[Serializable]
public class LiveDataControlPanel
{
	public bool masterOn;
	public bool raceStatusOn;
	public bool leaderboardOn;
	public bool trackMapOn;
	public bool pitLaneOn;
	public bool startLightsOn;
	public bool voiceOfOn;
	public bool chyronOn;
	public bool subtitlesOn;
	public bool introOn;

	public bool[] customLayerOn;
}
