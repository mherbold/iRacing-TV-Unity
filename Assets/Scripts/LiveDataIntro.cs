
using System;

[Serializable]
public class LiveDataIntro
{
	public bool show = false;

	public LiveDataIntroDriver[] liveDataIntroDrivers = new LiveDataIntroDriver[ LiveData.MaxNumDrivers ];
}
