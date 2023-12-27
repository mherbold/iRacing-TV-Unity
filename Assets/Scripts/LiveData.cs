
using System;

[Serializable]
public class LiveData
{
	public const int MaxNumDrivers = 64;
	public const int MaxNumClasses = 8;
	public const int MaxNumCustom = 6;

	public static LiveData Instance { get; private set; }

	public bool isConnected = false;
	public string systemMessage = string.Empty;

	public LiveDataSteamVr liveDataSteamVr = new();
	public LiveDataControlPanel liveDataControlPanel = new();
	public LiveDataDriver[] liveDataDrivers = new LiveDataDriver[ MaxNumDrivers ];
	public LiveDataRaceStatus liveDataRaceStatus = new();
	public LiveDataLeaderboard[] liveDataLeaderboards = null;
	public LiveDataVoiceOf liveDataVoiceOf = new();
	public LiveDataChyron liveDataChyron = new();
	public LiveDataBattleChyron liveDataBattleChyron = new();
	public LiveDataSubtitle liveDataSubtitle = new();
	public LiveDataIntro liveDataIntro = new();
	public LiveDataStartLights liveDataStartLights = new();
	public LiveDataTrackMap liveDataTrackMap = new();
	public LiveDataPitLane liveDataPitLane = new();
	public LiveDataHud liveDataHud = new();
	public LiveDataTrainer liveDataTrainer = new();
	public LiveDataWebcamStreaming liveDataWebcamStreaming = new();
	public LiveDataCustom[] liveDataCustom = new LiveDataCustom[ MaxNumCustom ];

	public string seriesLogoTextureUrl = string.Empty;
	public string trackLogoTextureUrl = string.Empty;

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
		liveDataSteamVr = liveData.liveDataSteamVr;
		liveDataControlPanel = liveData.liveDataControlPanel;
		liveDataDrivers = liveData.liveDataDrivers;
		liveDataRaceStatus = liveData.liveDataRaceStatus;
		liveDataLeaderboards = liveData.liveDataLeaderboards;
		liveDataVoiceOf = liveData.liveDataVoiceOf;
		liveDataChyron = liveData.liveDataChyron;
		liveDataBattleChyron = liveData.liveDataBattleChyron;
		liveDataSubtitle = liveData.liveDataSubtitle;
		liveDataIntro = liveData.liveDataIntro;
		liveDataStartLights = liveData.liveDataStartLights;
		liveDataTrackMap = liveData.liveDataTrackMap;
		liveDataPitLane = liveData.liveDataPitLane;
		liveDataHud = liveData.liveDataHud;
		liveDataTrainer = liveData.liveDataTrainer;
		liveDataWebcamStreaming = liveData.liveDataWebcamStreaming;
		liveDataCustom = liveData.liveDataCustom;

		seriesLogoTextureUrl = liveData.seriesLogoTextureUrl;
		trackLogoTextureUrl = liveData.trackLogoTextureUrl;
	}
}
