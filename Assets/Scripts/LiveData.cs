﻿
using System;

[Serializable]
public class LiveData
{
	public static LiveData Instance { get; private set; }

	public bool isConnected = false;

	public LiveDataRaceStatus liveDataRaceStatus = new();
	public LiveDataLeaderboard liveDataLeaderboard = new();
	public LiveDataVoiceOf liveDataVoiceOf = new();
	public LiveDataSubtitle liveDataSubtitle = new();
	public LiveDataIntro liveDataIntro = new();

	public string seriesLogoTextureUrl = string.Empty;

	static LiveData()
	{
		Instance = new LiveData();
	}

	private LiveData()
	{
		Instance = this;
	}

	public void Update( LiveData liveData )
	{
		liveDataRaceStatus = liveData.liveDataRaceStatus;
		liveDataLeaderboard = liveData.liveDataLeaderboard;
		liveDataVoiceOf = liveData.liveDataVoiceOf;
		liveDataSubtitle = liveData.liveDataSubtitle;
		liveDataIntro = liveData.liveDataIntro;

		seriesLogoTextureUrl = liveData.seriesLogoTextureUrl;
	}
}
